using System.Collections.Generic;
using UnityEngine;

public class Navigator : IPathFinder
{
    public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges)
    {
        Vector2 currentPosition = A;
        List<Vector2> result = new List<Vector2>
        {
            currentPosition
        };

        foreach (Edge edge in edges)
        {
            bool isDirect = true;

            foreach (Edge edge1 in edges)
            {
                if (IsCrossing(currentPosition, C, edge.Start, edge.End) == false)
                {
                    isDirect = false;
                    break;
                }
            }

            if (isDirect)
            {
                break;
            }

            currentPosition = edge.Start;
            result.Add(currentPosition);
        }

        result.Add(C);
        return result;
    }

    private bool IsCrossing(Vector2 v1p1, Vector2 v1p2, Vector2 v2p1, Vector2 v2p2)
    {
        float d = (v1p1.x - v1p2.x) * (v2p1.y - v2p2.y) - (v1p1.y - v1p2.y) * (v2p1.x - v2p2.x);

        if (d == 0)
        {
            return false;
        }

        float u = ((v2p1.x - v1p1.x) * (v1p1.y - v1p2.y) - (v2p1.y - v1p1.y) * (v1p1.x - v1p2.x)) / d;
        float t = ((v2p1.x - v1p1.x) * (v2p1.y - v2p2.y) - (v2p1.y - v1p1.y) * (v2p1.x - v2p2.x)) / d;

        return (u >= 0 && u <= 1 && t >= 0 && t <= 1);
    }
}
