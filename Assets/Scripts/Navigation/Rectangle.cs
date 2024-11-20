using System.Collections.Generic;
using UnityEngine;

public struct Rectangle
{
    public Vector2 Min;
    public Vector2 Max;
    public List<Vector2> edges;

    public Rectangle(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
        edges = new List<Vector2>
        {
            new Vector2 (min.x, min.y),
            new Vector2 (max.x, max.y),
            new Vector2 (min.x, max.y),
            new Vector2 (max.x, min.y)
        };
    }
}
