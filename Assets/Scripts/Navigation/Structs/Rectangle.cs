using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Rectangle
{
    private List<Vector2> _corners;

    public Vector2 Min;
    public Vector2 Max;

    public List<Vector2> Corners => _corners;

    public Rectangle(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
        _corners = new List<Vector2>
        {
            new Vector2 (min.x, min.y),
            new Vector2 (min.x, max.y),
            new Vector2 (max.x, max.y),
            new Vector2 (max.x, min.y)
        };
    }

    public bool Equals(Rectangle rectangle)
    {
        if (rectangle.Min == Min && rectangle.Max == Max)
        {
            return true;
        }

        return false;
    }
}
