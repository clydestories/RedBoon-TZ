using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Navigation/NavigationData", fileName = "NavigationData")]
public class NavigationDataHolderSO : ScriptableObject
{
    private Vector2 _startPosition;
    private Vector2 _finishPosition;
    private List<Rectangle> _rectangles;
    private List<Edge> _edges;

    public event Action FiguresChanged;

    public Vector2 StartPosition => _startPosition;
    public Vector2 FinishPosition => _finishPosition;
    public List<Edge> Edges => _edges.ToList();
    public List<Rectangle> Rectangles => _rectangles.ToList();

    public void ChangeStartPositionX(float newX)
    {
        _startPosition.x = newX;
        FiguresChanged?.Invoke();
    }

    public void ChangeStartPositionY(float newY)
    {
        _startPosition.y = newY;
        FiguresChanged?.Invoke();
    }

    public void ChangeFinishPositionX(float newX)
    {
        _finishPosition.x = newX;
        FiguresChanged?.Invoke();
    }

    public void ChangeFinishPositionY(float newY)
    {
        _finishPosition.y = newY;
        FiguresChanged?.Invoke();
    }

    public void AddRectangle(Rectangle rectangle)
    {
        _rectangles.Add(rectangle);
        FiguresChanged?.Invoke();
    }

    public bool TryRemoveRectangle(int index)
    {
        if (index < _rectangles.Count && index >= 0)
        {
            _rectangles.RemoveAt(index);
            FiguresChanged?.Invoke();
            return true;
        }

        return false;
    }

    public void ChangeRectangle(int index, float minX, float minY, float maxX, float maxY)
    {
        _rectangles[index] = new Rectangle(new Vector2(minX, minY), new Vector2(maxX, maxY));
        FiguresChanged?.Invoke();
    }

    public void AddEdge(Edge edge)
    {
        _edges.Add(edge);
        FiguresChanged?.Invoke();
    }

    public void ChangeEdge(int index, Vector2 start, Vector2 end, Rectangle first, Rectangle second)
    {
        _edges[index] = new Edge(first, second, start, end);
        FiguresChanged?.Invoke();
    }

    public bool TryRemoveEdge(int index)
    {
        if (index < _edges.Count && index >= 0)
        {
            _edges.RemoveAt(index);
            FiguresChanged?.Invoke();
            return true;
        }

        return false;
    }
}
