using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigationDataInputReader : MonoBehaviour
{
    [SerializeField] private NavigationDataHolderSO _navData;
    [SerializeField] private RectangleCell _rectangleCellPrefab;
    [SerializeField] private EdgeCell _edgeCellPrefab;
    [SerializeField] private LayoutGroup _rectangleCellContainer;
    [SerializeField] private LayoutGroup _edgeCellContainer;
    [SerializeField] private InputField _startPointX;
    [SerializeField] private InputField _startPointY;
    [SerializeField] private InputField _finishPointX;
    [SerializeField] private InputField _finishPointY;
    [SerializeField] private TMP_InputField _rectangleIDToRemove;
    [SerializeField] private TMP_InputField _edgeIDToRemove;

    private List<RectangleCell> _rectangleCells;
    private List<EdgeCell> _edgeCells;

    private void Awake()
    {
        _rectangleCells = new List<RectangleCell>();
        _edgeCells = new List<EdgeCell>();
    }

    private void OnEnable()
    {
        _startPointX.onValueChanged.AddListener(UpdateStartPositionX);
        _startPointY.onValueChanged.AddListener(UpdateStartPositionY);
        _finishPointX.onValueChanged.AddListener(UpdateFinishPositionX);
        _finishPointY.onValueChanged.AddListener(UpdateFinishPositionY);
    }

    private void Start()
    {
        Initialize();
    }

    private void OnDisable()
    {
        _startPointX.onValueChanged.RemoveListener(UpdateStartPositionX);
        _startPointY.onValueChanged.RemoveListener(UpdateStartPositionY);
        _finishPointX.onValueChanged.RemoveListener(UpdateFinishPositionX);
        _finishPointY.onValueChanged.RemoveListener(UpdateFinishPositionY);
    }

    public void OnAddEdge()
    {
        if (_navData.Rectangles.Count >= 2)
        {
            EdgeCell edgeCell = Instantiate(_edgeCellPrefab, _edgeCellContainer.transform);
            Edge edge = new Edge(_navData.Rectangles[0], _navData.Rectangles[1], new Vector2(), new Vector2());
            _navData.AddEdge(edge);
            edgeCell.Construct(edge.Start.x, edge.Start.y, edge.End.x, edge.End.y, 0, 1, _edgeCells.Count);
            _edgeCells.Add(edgeCell);
            edgeCell.StartXField.ValueChanged += ChangeEdgeStartX;
            edgeCell.StartYField.ValueChanged += ChangeEdgeStartY;
            edgeCell.EndXField.ValueChanged += ChangeEdgeEndX;
            edgeCell.EndYField.ValueChanged += ChangeEdgeEndY;
            edgeCell.FirstRectangleIdField.ValueChanged += ChangeEdgeFirst;
            edgeCell.SecondRectangleIdField.ValueChanged += ChangeEdgeSecond;
        }
    }

    public void OnAddEdge(Edge edge)
    {
        EdgeCell edgeCell = Instantiate(_edgeCellPrefab, _edgeCellContainer.transform);
        edgeCell.Construct(edge.Start.x, edge.Start.y, edge.End.x, edge.End.y, _navData.Rectangles.IndexOf(edge.First), _navData.Rectangles.IndexOf(edge.Second), _edgeCells.Count);
        _edgeCells.Add(edgeCell);
        edgeCell.StartXField.ValueChanged += ChangeEdgeStartX;
        edgeCell.StartYField.ValueChanged += ChangeEdgeStartY;
        edgeCell.EndXField.ValueChanged += ChangeEdgeEndX;
        edgeCell.EndYField.ValueChanged += ChangeEdgeEndY;
        edgeCell.FirstRectangleIdField.ValueChanged += ChangeEdgeFirst;
        edgeCell.SecondRectangleIdField.ValueChanged += ChangeEdgeSecond;
    }

    public void OnRemoveEdge()
    {
        if (int.TryParse(_edgeIDToRemove.text, out int result))
        {
            if (_navData.TryRemoveEdge(result))
            {
                Destroy(_edgeCells[result].gameObject);
                _edgeCells.RemoveAt(result);
            }
        }
    }

    private void ChangeEdgeStartX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(id, new Vector2(result, _navData.Edges[id].Start.y), _navData.Edges[id].End, _navData.Edges[id].First, _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeStartY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(id, new Vector2(_navData.Edges[id].Start.x, result), _navData.Edges[id].End, _navData.Edges[id].First, _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeEndX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(id, _navData.Edges[id].Start, new Vector2(result, _navData.Edges[id].End.y), _navData.Edges[id].First, _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeEndY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(id, _navData.Edges[id].Start, new Vector2(_navData.Edges[id].End.x, result), _navData.Edges[id].First, _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeFirst(string value, int id)
    {
        if (int.TryParse(value, out int result))
        {
            if (result < _navData.Rectangles.Count && result >= 0)
            {
                _navData.ChangeEdge(id, _navData.Edges[id].Start, _navData.Edges[id].End, _navData.Rectangles[result], _navData.Edges[id].Second);
            }
        }
    }

    private void ChangeEdgeSecond(string value, int id)
    {
        if (int.TryParse(value, out int result))
        {
            if (result < _navData.Rectangles.Count && result >= 0)
            {
                _navData.ChangeEdge(id, _navData.Edges[id].Start, _navData.Edges[id].End, _navData.Edges[id].First, _navData.Rectangles[result]);
            }
        }
    }

    public void OnAddRectangle()
    {
        RectangleCell rectangleCell = Instantiate(_rectangleCellPrefab, _rectangleCellContainer.transform);
        Rectangle rectangle = new Rectangle(new Vector2(-1, -1), new Vector2(1, 1));
        _navData.AddRectangle(rectangle);
        rectangleCell.Construct(rectangle.Min.x, rectangle.Min.y, rectangle.Max.x, rectangle.Max.y, _rectangleCells.Count);
        _rectangleCells.Add(rectangleCell);
        rectangleCell.MinXField.ValueChanged += ChangeRectangleMinX;
        rectangleCell.MinYField.ValueChanged += ChangeRectangleMinY;
        rectangleCell.MaxXField.ValueChanged += ChangeRectangleMaxX;
        rectangleCell.MaxYField.ValueChanged += ChangeRectangleMaxY;
    }

    public void OnAddRectangle(Rectangle rectangle)
    {
        RectangleCell rectangleCell = Instantiate(_rectangleCellPrefab, _rectangleCellContainer.transform);
        rectangleCell.Construct(rectangle.Min.x, rectangle.Min.y, rectangle.Max.x, rectangle.Max.y, _rectangleCells.Count);
        _rectangleCells.Add(rectangleCell);
        rectangleCell.MinXField.ValueChanged += ChangeRectangleMinX;
        rectangleCell.MinYField.ValueChanged += ChangeRectangleMinY;
        rectangleCell.MaxXField.ValueChanged += ChangeRectangleMaxX;
        rectangleCell.MaxYField.ValueChanged += ChangeRectangleMaxY;
    }

    public void ChangeRectangleMinX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(id, result, _navData.Rectangles[id].Min.y, _navData.Rectangles[id].Max.x, _navData.Rectangles[id].Max.y);
        }
    }

    public void ChangeRectangleMinY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(id, _navData.Rectangles[id].Min.x, result, _navData.Rectangles[id].Max.x, _navData.Rectangles[id].Max.y);
        }
    }

    public void ChangeRectangleMaxX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(id, _navData.Rectangles[id].Min.x, _navData.Rectangles[id].Min.y, result, _navData.Rectangles[id].Max.y);
        }
    }

    public void ChangeRectangleMaxY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(id, _navData.Rectangles[id].Min.x, _navData.Rectangles[id].Min.y, _navData.Rectangles[id].Max.x, result);
        }
    }

    public void OnRemoveRectangle()
    {
        if (int.TryParse(_rectangleIDToRemove.text, out int result))
        {
            if (_navData.TryRemoveRectangle(result))
            {
                Destroy(_rectangleCells[result].gameObject);
                _rectangleCells.RemoveAt(result);
            }
        }
    }

    private void UpdateStartPositionX(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeStartPositionX(result);
        }
    }

    private void UpdateStartPositionY(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeStartPositionY(result);
        }
    }

    private void UpdateFinishPositionX(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeFinishPositionX(result);
        }
    }

    private void UpdateFinishPositionY(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeFinishPositionY(result);
        }
    }

    private void Initialize()
    {
        _startPointX.text = _navData.StartPosition.x.ToString();
        _startPointY.text = _navData.StartPosition.y.ToString();
        _finishPointX.text = _navData.FinishPosition.x.ToString();
        _finishPointY.text = _navData.FinishPosition.y.ToString();

        foreach (Rectangle rectangle in _navData.Rectangles)
        {
            OnAddRectangle(rectangle);
        }

        foreach (Edge edge in _navData.Edges)
        {
            OnAddEdge(edge);
        }
    }
}
