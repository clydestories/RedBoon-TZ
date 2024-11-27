using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private NavigationDataHolderSO _navData;
    [SerializeField] private Execution _execution;
    [SerializeField] private Material _lineMaterial;
    [Header("Settings")]
    [SerializeField] private float _lineWidth;
    [SerializeField] private float _pointOffset;
    [SerializeField] private Color _pointColor;
    [SerializeField] private Color _rectangleColor;
    [SerializeField] private Color _edgeColor;
    [SerializeField] private Color _polylineColor;

    private int _edgeSortingOrder = 5;
    private int _pointSortingOrder = 10;
    private int _polylineSortingOrder = 15;

    private void OnEnable()
    {
        _navData.FiguresChanged += DrawNavigationField;
        _execution.ResultsReceived += DrawPolyline;
    }

    private void OnDisable()
    {
        _navData.FiguresChanged -= DrawNavigationField;
        _execution.ResultsReceived -= DrawPolyline;
    }

    public LineRenderer DrawLine(Vector2 start, Vector2 end, Color color)
    {
        LineRenderer line = CreateLine(color);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        return line;
    }

    public void DrawPolyline(List<Vector2> points)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            LineRenderer line = DrawLine(points[i], points[i + 1], _polylineColor);
            line.sortingOrder = _polylineSortingOrder;
        }
    }

    public void ClearLines()
    {
        foreach (LineRenderer line in GetComponentsInChildren<LineRenderer>())
        {
            Destroy(line.gameObject);
        }
    }

    private void DrawNavigationField()
    {
        ClearLines();

        DrawPoint(_navData.StartPosition, _pointColor);
        DrawPoint(_navData.FinishPosition, _pointColor);

        foreach (Rectangle rectangle in _navData.Rectangles)
        {
            DrawRectangle(rectangle, _rectangleColor);
        }

        foreach (Edge edge in _navData.Edges)
        {
            LineRenderer line = DrawLine(edge.Start, edge.End, _edgeColor);
            line.sortingOrder = _edgeSortingOrder;
        }
    }

    private void DrawRectangle(Rectangle rectangle, Color color)
    {
        for (int i = 0; i < rectangle.Corners.Count; i++)
        {
            DrawLine(rectangle.Corners[i], rectangle.Corners[(i + 1) % rectangle.Corners.Count], color);
        }
    }

    private void DrawPoint(Vector2 center, Color color)
    {
        LineRenderer line = CreateLine(color);
        line.SetPosition(0, new Vector3(center.x - _pointOffset, center.y, 0));
        line.SetPosition(1, new Vector3(center.x + _pointOffset, center.y, 0));
        line.sortingOrder = _pointSortingOrder;
    }

    private LineRenderer CreateLine(Color color)
    {
        GameObject line = new GameObject();
        line.transform.SetParent(transform);
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.material = _lineMaterial;
        return lineRenderer;
    }
}
