using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] private float _step;
    [SerializeField] private Visualizer _visual;

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
            //Проверка на проход к C напрямую
            if (CanReach(currentPosition, C, edgesAsList, 0))
            {
                break;
            }

            //Из текущей точки проверяет все возможные пути через ближайший edge. Если есть такой, который проходит через 1+ последующий edge - пробегает по области от edge До конца
            int bestEdgeCrosses = 0;

            Vector2 bestTarget = GetBestTargetFromPoint(currentPosition, edgesAsList, i, out bestEdgeCrosses);

            Debug.Log($"bestTarget {bestTarget}");
            
            if (bestEdgeCrosses > 0)
            {
                Ray ray = new Ray(currentPosition, bestTarget - currentPosition);
                Vector2 target = ray.GetPoint(999);

                for (int j = i; j < edgesAsList[i + bestEdgeCrosses].Second.corners.Count; j++)
                {
                    if (IsCrossing(currentPosition, target, edgesAsList[i + bestEdgeCrosses].Second.corners[j], edgesAsList[i + bestEdgeCrosses].Second.corners[(j + 1) % edgesAsList[i + bestEdgeCrosses].Second.corners.Count]))
                    {
                        if (IsCrossing(edgesAsList[i + bestEdgeCrosses].Second.corners[j], edgesAsList[i + bestEdgeCrosses].Second.corners[(j + 1) % edgesAsList[i + bestEdgeCrosses].Second.corners.Count], edgesAsList[i + bestEdgeCrosses].Start, edgesAsList[i + bestEdgeCrosses].End) == false)
                        {
                            currentPosition = GetCrossPoint(currentPosition, bestTarget, edgesAsList[i + bestEdgeCrosses].Second.corners[j], edgesAsList[i + bestEdgeCrosses].Second.corners[(j + 1) % edgesAsList[i + bestEdgeCrosses].Second.corners.Count]);
                        }
                    }
                }
            }
            else
            {
                Vector2 edgeCenter = Vector2.Lerp(edgesAsList[i].Start, edgesAsList[i].End, 0.5f);
                Ray ray = new Ray(currentPosition, edgeCenter - currentPosition);
                Vector2 target = ray.GetPoint(999);
                Vector2 endOfSearchLine = Vector2.zero;

                for (int j = 0; j < edgesAsList[i].Second.corners.Count; j++)
                {
                    if (IsCrossing(currentPosition, target, edgesAsList[i].Second.corners[j], edgesAsList[i].Second.corners[(j + 1) % edgesAsList[i].Second.corners.Count]))
                    {
                        if (IsParallel(edgesAsList[i].Second.corners[j], edgesAsList[i].Second.corners[(j + 1) % edgesAsList[i].Second.corners.Count], edgesAsList[i].Start, edgesAsList[i].End) == false)
                        {
                            endOfSearchLine = GetCrossPoint(currentPosition, target, edgesAsList[i].Second.corners[j], edgesAsList[i].Second.corners[(j + 1) % edgesAsList[i].Second.corners.Count]);
                        }
                    }
                }
                
                Vector2 checkOrigin = edgeCenter;
                Vector2 bestOrigin = checkOrigin;
                int bestEverEdgeCrosses = 0;

                if (i < edgesAsList.Count - 1)
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

                        if (bestEdgeCrosses >= bestEverEdgeCrosses)
                        {
                            bestTarget = newTarget;
                            bestEverEdgeCrosses = bestEdgeCrosses;
                            bestOrigin = checkOrigin;
                        }

                        checkOrigin = Vector2.MoveTowards(checkOrigin, endOfSearchLine, _step);
                    }

                    if (directPathFound)
                    {
                        result.Add(bestOrigin);
                        break;
                    }

                    Debug.Log($"bestEverEdgeCrosses {bestEverEdgeCrosses}");

                    if (bestEverEdgeCrosses == 0)
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
                Vector2 currentTarget = newRay.GetPoint(999);

                for (int j = 0; j < edgesAsList[i + bestEverEdgeCrosses].Second.corners.Count; j++)
                {
                    if (IsCrossing(currentPosition, target, edgesAsList[i + bestEverEdgeCrosses].Second.corners[j], edgesAsList[i + bestEverEdgeCrosses].Second.corners[(j + 1) % edgesAsList[i].Second.corners.Count]))
                    {
                        if (IsParallel(edgesAsList[i + bestEverEdgeCrosses].Second.corners[j], edgesAsList[i + bestEverEdgeCrosses].Second.corners[(j + 1) % edgesAsList[i].Second.corners.Count], edgesAsList[i + bestEverEdgeCrosses].Start, edgesAsList[i + bestEverEdgeCrosses].End) == false)
                        {
                            currentPosition = GetCrossPoint(currentPosition, bestTarget, edgesAsList[i + bestEverEdgeCrosses].Second.corners[j], edgesAsList[i + bestEverEdgeCrosses].Second.corners[(j + 1) % edgesAsList[i].Second.corners.Count]);
                        }
                    }
                }
            }

            result.Add(currentPosition);
        }

        result.Add(C);
        return result;
    }

    private bool IsParallel(Vector2 v1p1, Vector2 v1p2, Vector2 v2p1, Vector2 v2p2)
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

        while ((edgeTarget - edgesAsList[i].End).magnitude > 0.1f)
        {
            Ray ray = new Ray(currentPosition, edgeTarget - currentPosition);
            Vector2 target = ray.GetPoint(999);

            int currentEdgeCrosses = 0;

            CheckEdgeCrossage(currentPosition, target, edgesAsList, i, ref currentEdgeCrosses);

            //_visual.DrawLine(currentPosition, edgeTarget, Color.green);

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

            for (int i = 0; i < edge.First.corners.Count; i++)
            {
                if (IsCrossing(currnentPosition, targetPosition, edge.First.corners[i], edge.First.corners[(i + 1) % edge.First.corners.Count]))
                {
                        if (IsParallel(edge.First.corners[i], edge.First.corners[(i + 1) % edge.First.corners.Count], edge.Start, edge.End) == false)
                        {
                            if (edges.IndexOf(edge) - 1 >= 0)
                            {
                                if (IsParallel(edge.First.corners[i], edge.First.corners[(i + 1) % edge.First.corners.Count], edges[edges.IndexOf(edge) - 1].Start, edges[edges.IndexOf(edge) - 1].End) == false)
                                {
                                    if (IsCrossing(targetPosition - Vector2.one / 10, targetPosition + Vector2.one / 10, edge.First.corners[i], edge.First.corners[(i + 1) % edge.First.corners.Count]) == false)
                                    {
                                        return false;
                                    }
                                }
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

    private void CheckEdgeCrossage(Vector2 currentPosition, Vector2 target, List<Edge> edgesAsList, int index, ref int bestPathEdgeCrosses)
    {
        bestPathEdgeCrosses = 0;
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
