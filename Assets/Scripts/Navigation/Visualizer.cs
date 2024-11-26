using UnityEngine;

public class Visualizer : MonoBehaviour
{
    private readonly Color _purple = new Color(1, 0, 1);

    [SerializeField] private Material _lineMaterial;
    [SerializeField] private NavigationDataHolderSO _navData;
    [SerializeField] private float _lineWidth;
    [SerializeField] private float _pointOffset;

    private void OnEnable()
    {
        _navData.FiguresChanged += DrawNavigationField;
    }

    private void OnDisable()
    {
        _navData.FiguresChanged -= DrawNavigationField;
    }

    private void DrawNavigationField()
    {
        ClearLines();

        DrawPoint(_navData.StartPosition, Color.yellow);
        DrawPoint(_navData.FinishPosition, Color.yellow);

        foreach (Rectangle rectangle in _navData.Rectangles)
        {
            DrawRectangle(rectangle, Color.red);
        }

        foreach (Edge edge in _navData.Edges)
        {
            LineRenderer line = DrawLine(edge.Start, edge.End, Color.green);
            line.sortingOrder = 5;
        }
    }

    public LineRenderer DrawLine(Vector2 start, Vector2 end, Color color)
    {
        LineRenderer line = CreateLine(color);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        return line;
    }

    public void DrawRectangle(Rectangle rectangle, Color color)
    {
        for (int i = 0; i < rectangle.corners.Count; i++)
        {
            DrawLine(rectangle.corners[i], rectangle.corners[(i + 1) % rectangle.corners.Count], color);
        }
    }

    public void DrawPoint(Vector2 center, Color color)
    {
        LineRenderer line = CreateLine(color);
        line.SetPosition(0, new Vector3(center.x - _pointOffset, center.y, 0));
        line.SetPosition(1, new Vector3(center.x + _pointOffset, center.y, 0));
        line.sortingOrder = 10;
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

    public void ClearLines()
    {
        foreach (LineRenderer line in GetComponentsInChildren<LineRenderer>())
        {
            Destroy(line.gameObject);
        }
    }
}
