using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Navigator : MonoBehaviour, IPathFinder
{
    [SerializeField] private float _step;

    private float _targetRayPoint = 999;
    private Vector2 _targetDetectOffset = Vector2.one / 10;

    public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges)
    {
        List<Edge> edgesAsList = edges.ToList();
        Vector2 currentPosition = A;
        List<Vector2> result = new List<Vector2>
        {
            currentPosition
        };

        for (int i = 0; i < edgesAsList.Count; i++)
        {
            if (CanReach(currentPosition, C, edgesAsList, i))
            {
                break;
            }

            int bestEdgeCrosses = 0;
            Vector2 bestTarget = GetBestTargetFromPoint(currentPosition, edgesAsList, i, out bestEdgeCrosses);

            if (bestEdgeCrosses > 0)//Dont work yet
            {
                Ray ray = new Ray(currentPosition, bestTarget - currentPosition);
                Vector2 target = ray.GetPoint(_targetRayPoint);

                if (HasCleanCross(edgesAsList[i + bestEdgeCrosses], currentPosition, target, out int targetIndex))
                {
                    Vector2 currentCorner = edgesAsList[i + bestEdgeCrosses].Second.Corners[targetIndex];
                    Vector2 nextCorner = edgesAsList[i + bestEdgeCrosses].Second.Corners[(targetIndex + 1) % edgesAsList[i + bestEdgeCrosses].Second.Corners.Count];

                    currentPosition = GetCrossPoint(currentPosition, bestTarget, currentCorner, nextCorner);
                }
            }
            else
            {
                Vector2 edgeCenter = Vector2.Lerp(edgesAsList[i].Start, edgesAsList[i].End, 0.5f);
                Ray ray = new Ray(currentPosition, edgeCenter - currentPosition);
                Vector2 target = ray.GetPoint(_targetRayPoint);
                Vector2 endOfSearchLine = Vector2.zero;

                if (HasCleanCross(edgesAsList[i], currentPosition, target, out int targetIndex))
                {
                    Vector2 currentCorner = edgesAsList[i].Second.Corners[targetIndex];
                    Vector2 nextCorner = edgesAsList[i].Second.Corners[(targetIndex + 1) % edgesAsList[i].Second.Corners.Count];

                    endOfSearchLine = GetCrossPoint(currentPosition, target, currentCorner, nextCorner);
                }

                Vector2 checkOrigin = edgeCenter;
                Vector2 bestOrigin = checkOrigin;
                int bestEdgeCrossesOverall = 0;

                if (i < edgesAsList.Count)
                {
                    bool directPathFound = false;

                    while (checkOrigin != endOfSearchLine)
                    {
                        if (CanReach(checkOrigin, C, edgesAsList, i + 1))
                        {
                            bestOrigin = checkOrigin;
                            bestTarget = C;
                            directPathFound = true;
                            break;
                        }

                        Vector2 newTarget = GetBestTargetFromPoint(checkOrigin, edgesAsList, i + 1, out bestEdgeCrosses);

                        if (bestEdgeCrosses >= bestEdgeCrossesOverall)
                        {
                            bestTarget = newTarget;
                            bestEdgeCrossesOverall = bestEdgeCrosses;
                            bestOrigin = checkOrigin;
                        }

                        checkOrigin = Vector2.MoveTowards(checkOrigin, endOfSearchLine, _step);
                    }

                    if (directPathFound)
                    {
                        result.Add(bestOrigin);
                        break;
                    }

                    if (bestEdgeCrossesOverall == 0)
                    {
                        bestTarget = edgeCenter;
                    }
                }
                else
                {
                    bestTarget = C;
                }

                if (bestTarget == C)
                {
                    break;
                }

                Ray newRay = new Ray(bestOrigin, bestTarget - bestOrigin);
                Vector2 currentTarget = newRay.GetPoint(_targetRayPoint);

                if (HasCleanCross(edgesAsList[i + bestEdgeCrossesOverall], currentPosition, target, out int currentTargetIndex))
                {
                    Vector2 currentCorner = edgesAsList[i + bestEdgeCrossesOverall].Second.Corners[currentTargetIndex];
                    Vector2 nextCorner = edgesAsList[i + bestEdgeCrossesOverall].Second.Corners[(currentTargetIndex + 1) % edgesAsList[i].Second.Corners.Count];

                    currentPosition = GetCrossPoint(currentPosition, bestTarget, currentCorner, nextCorner);
                }
            }

            result.Add(currentPosition);
        }

        result.Add(C);
        return result;
    }

    private bool HasCleanCross(Edge edge, Vector2 from, Vector2 to, out int targetIndex)
    {
        targetIndex = -1;

        for (int j = 0; j < edge.Second.Corners.Count; j++)
        {
            Vector2 currentCorner = edge.Second.Corners[j];
            Vector2 nextCorner = edge.Second.Corners[(j + 1) % edge.Second.Corners.Count];

            if (IsCrossing(from, to, currentCorner, nextCorner))
            {
                if (IsCrossing(currentCorner, nextCorner, edge.Start, edge.End) == false)
                {
                    targetIndex = j;
                }
            }
        }

        if (targetIndex != -1)
        {
            return true;
        }

        return false;
    }

    private bool AreCollinear(Vector2 v1p1, Vector2 v1p2, Vector2 v2p1, Vector2 v2p2)
    {
        if (v1p1.x == v1p2.x)
        {
            if (v2p1.x == v2p2.x)
            {
                if (v1p1.x == v2p1.x)
                {
                    return true;
                }
            }
        }
        else
        {
            if (v2p1.y == v2p2.y)
            {
                if (v1p1.y == v2p1.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsCrossing(Vector2 v1p1, Vector2 v1p2, Vector2 v2p1, Vector2 v2p2)
    {
        Vector3 intersectionPoint = Vector3.zero;
        Vector2 rayDirection = v1p2 - v1p1;
        rayDirection.Normalize();

        float det = rayDirection.x * (v2p2.y - v2p1.y) - rayDirection.y * (v2p2.x - v2p1.x);

        float t = ((v2p1.x - v1p1.x) * (v2p2.y - v2p1.y) - (v2p1.y - v1p1.y) * (v2p2.x - v2p1.x)) / det;
        float u = -((v1p1.x - v2p1.x) * rayDirection.y - (v1p1.y - v2p1.y) * rayDirection.x) / det;

        return (u >= 0 && u <= 1 && t >= 0);
    }

    private Vector2 GetCrossPoint(Vector2 v1p1, Vector2 v1p2, Vector2 v2p1, Vector2 v2p2)
    {
        Vector3 intersectionPoint = Vector3.zero;
        Vector2 rayDirection = v1p2 - v1p1;
        rayDirection.Normalize();

        float det = rayDirection.x * (v2p2.y - v2p1.y) - rayDirection.y * (v2p2.x - v2p1.x);

        if (Mathf.Abs(det) < float.Epsilon)
        {
            return Vector3.zero;
        }

        float t = ((v2p1.x - v1p1.x) * (v2p2.y - v2p1.y) - (v2p1.y - v1p1.y) * (v2p2.x - v2p1.x)) / det;
        float u = -((v1p1.x - v2p1.x) * rayDirection.y - (v1p1.y - v2p1.y) * rayDirection.x) / det;

        if (t >= 0 && u >= 0 && u <= 1)
        {
            intersectionPoint = v1p1 + t * rayDirection;
        }

        return intersectionPoint;
    }

    private Vector2 GetBestTargetFromPoint(Vector2 currentPosition, List<Edge> edgesAsList, int i, out int bestPathEdgeCrosses)
    {
        Vector2 edgeTarget = edgesAsList[i].Start;
        bool EdgeIsHorizontal;
        Vector2 bestPoint = currentPosition;
        Vector2 bestTarget = edgeTarget;
        bestPathEdgeCrosses = 0;
        float edgeStart;
        float edgeEnd;

        if (edgesAsList[i].Start.x == edgesAsList[i].End.x)
        {
            EdgeIsHorizontal = false;
            edgeStart = edgesAsList[i].Start.y;
            edgeEnd = edgesAsList[i].End.y;
        }
        else
        {
            EdgeIsHorizontal = true;
            edgeStart = edgesAsList[i].Start.x;
            edgeEnd = edgesAsList[i].End.x;
        }

        while ((edgeTarget - edgesAsList[i].End).magnitude > _step)
        {
            Ray ray = new Ray(currentPosition, edgeTarget - currentPosition);
            Vector2 target = ray.GetPoint(_targetRayPoint);

            int currentEdgeCrosses = 0;

            CheckEdgeCrossage(currentPosition, target, edgesAsList, i, ref currentEdgeCrosses);//Dont work yet

            if (currentEdgeCrosses >= bestPathEdgeCrosses)
            {
                bestPathEdgeCrosses = currentEdgeCrosses;
                bestPoint = currentPosition;
                bestTarget = edgeTarget;
            }

            if (EdgeIsHorizontal)
            {
                if (edgeStart < edgeEnd)
                {
                    edgeTarget.x += _step;
                }
                else
                {
                    edgeTarget.x -= _step;
                }

            }
            else
            {
                if (edgeStart < edgeEnd)
                {
                    edgeTarget.y += _step;
                }
                else
                {
                    edgeTarget.y -= _step;
                }
            }
        }

        return bestTarget;
    }

    private bool CanReach(Vector2 currnentPosition, Vector2 targetPosition, List<Edge> edges, int from)
    {
        foreach (Edge edge in edges)
        {
            if (edges.IndexOf(edge) < from)
            {
                continue;
            }

            for (int i = 0; i < edge.First.Corners.Count; i++)
            {
                Vector2 currentCorner = edge.First.Corners[i];
                Vector2 nextCorner = edge.First.Corners[(i + 1) % edge.First.Corners.Count];

                if (IsCrossing(currnentPosition, targetPosition, currentCorner, nextCorner))
                {
                    if (AreCollinear(currentCorner, nextCorner, edge.Start, edge.End) == false)
                    {
                        if (edges.IndexOf(edge) - 1 >= 0)
                        {
                            if (AreCollinear(currentCorner, nextCorner, edges[edges.IndexOf(edge) - 1].Start, edges[edges.IndexOf(edge) - 1].End) == false)
                            {
                                if (IsCrossing(targetPosition - _targetDetectOffset, targetPosition + _targetDetectOffset, currentCorner, nextCorner) == false)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (IsCrossing(currnentPosition, targetPosition, edge.Start, edge.End) == false)
                        {
                            return false;
                        }
                    }
                }
            }

            for (int i = 0; i < edge.Second.Corners.Count; i++)
            {
                Vector2 currentCorner = edge.Second.Corners[i];
                Vector2 nextCorner = edge.Second.Corners[(i + 1) % edge.Second.Corners.Count];

                if (IsCrossing(currnentPosition, targetPosition, currentCorner, nextCorner))
                {
                    if (AreCollinear(currentCorner, nextCorner, edge.Start, edge.End) == false)
                    {
                        if (edges.IndexOf(edge) - 1 >= 0)
                        {
                            if (AreCollinear(currentCorner, nextCorner, edges[edges.IndexOf(edge) - 1].Start, edges[edges.IndexOf(edge) - 1].End) == false)
                            {
                                if (IsCrossing(targetPosition - _targetDetectOffset, targetPosition + _targetDetectOffset, currentCorner, nextCorner) == false)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (IsCrossing(currnentPosition, targetPosition, edge.Start, edge.End) == false)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private void CheckEdgeCrossage(Vector2 currentPosition, Vector2 target, List<Edge> edgesAsList, int index, ref int bestPathEdgeCrosses)//Dont work yet
    {
        return;
        if (index + 1 < edgesAsList.Count)
        {
            if (IsCrossing(currentPosition, target, edgesAsList[index + 1].Start, edgesAsList[index + 1].End))
            {
                bestPathEdgeCrosses++;
                CheckEdgeCrossage(currentPosition, target, edgesAsList, index + 1, ref bestPathEdgeCrosses);
            }
        }
    }
}
