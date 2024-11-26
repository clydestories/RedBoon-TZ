using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [SerializeField] private Material _lineMaterial;
    [SerializeField] private float _lineWidth;
    [SerializeField] private float _pointOffset;

    public void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        LineRenderer line = CreateLine(color);
        line.SetPosition(0, start);
        line.SetPosition(1, end);
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
    }

    private LineRenderer CreateLine(Color color)
    {
        GameObject line = new GameObject();
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.startWidth = _lineWidth;
        lineRenderer.endWidth = _lineWidth;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.material = _lineMaterial;
        return lineRenderer;
    }
}
