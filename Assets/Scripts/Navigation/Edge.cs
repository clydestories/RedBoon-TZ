using System;
using UnityEngine;

[Serializable]
public struct Edge
{
    public Rectangle First;
    public Rectangle Second;
    public Vector2 Start;
    public Vector2 End;

    public Edge(Rectangle first, Rectangle second, Vector2 start, Vector2 end)
    {
        First = first;
        Second = second;
        Start = start;
        End = end;
    }
}
