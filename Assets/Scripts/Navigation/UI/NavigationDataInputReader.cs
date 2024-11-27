using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigationDataInputReader : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private NavigationDataHolderSO _navData;
    [Header("Prefabs")]
    [SerializeField] private RectangleCell _rectangleCellPrefab;
    [SerializeField] private EdgeCell _edgeCellPrefab;
    [Header("Containers")]
    [SerializeField] private LayoutGroup _rectangleCellContainer;
    [SerializeField] private LayoutGroup _edgeCellContainer;
    [Header("InputFields")]
    [SerializeField] private InputField _startPointX;
    [SerializeField] private InputField _startPointY;
    [SerializeField] private InputField _finishPointX;
    [SerializeField] private InputField _finishPointY;
    [SerializeField] private TMP_InputField _rectangleIDToRemove;
    [SerializeField] private TMP_InputField _edgeIDToRemove;
    [Header("Alert errors")]
    [SerializeField] private string _lessThanRequiredRectanglesEdgeSpawnAlert;

    private List<RectangleCell> _rectangleCells;
    private List<EdgeCell> _edgeCells;

    private int _minRectangleAmountToCreateEdge = 2;
    private int _defaultFirstRectangleId = 0;
    private int _defaultSecondRectangleId = 1;
    private Vector2 _defaultRectangleMin = new Vector2(-1, -1);
    private Vector2 _defaultRectangleMax = new Vector2(1, 1);

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
        if (_navData.Rectangles.Count >= _minRectangleAmountToCreateEdge)
        {
            EdgeCell edgeCell = Instantiate(_edgeCellPrefab, _edgeCellContainer.transform);
            Edge edge = new Edge(
                _navData.Rectangles[_defaultFirstRectangleId], 
                _navData.Rectangles[_defaultSecondRectangleId], 
                new Vector2(), 
                new Vector2());
            
            _navData.AddEdge(edge);
            edgeCell.Construct(
                edge.Start.x, 
                edge.Start.y, 
                edge.End.x, 
                edge.End.y, 
                _defaultFirstRectangleId, 
                _defaultSecondRectangleId, 
                _edgeCells.Count);

            _edgeCells.Add(edgeCell);

            edgeCell.StartXField.ValueChanged += ChangeEdgeStartX;
            edgeCell.StartYField.ValueChanged += ChangeEdgeStartY;
            edgeCell.EndXField.ValueChanged += ChangeEdgeEndX;
            edgeCell.EndYField.ValueChanged += ChangeEdgeEndY;
            edgeCell.FirstRectangleIdField.ValueChanged += ChangeEdgeFirst;
            edgeCell.SecondRectangleIdField.ValueChanged += ChangeEdgeSecond;
        }
        else
        {
            AlertSystem.Instance.SendAlert(_lessThanRequiredRectanglesEdgeSpawnAlert + " " + _minRectangleAmountToCreateEdge.ToString());
        }
    }

    public void OnRemoveEdge()
    {
        if (int.TryParse(_edgeIDToRemove.text, out int result))
        {
            if (_navData.TryRemoveEdge(result))
            {
                Destroy(_edgeCells[result].gameObject);
                _edgeCells.RemoveAt(result);

                for (int i = 0; i < _edgeCells.Count; i++)
                {
                    _edgeCells[i].CellId = i;
                }
            }
        }
    }

    public void OnAddRectangle()
    {
        RectangleCell rectangleCell = Instantiate(_rectangleCellPrefab, _rectangleCellContainer.transform);
        Rectangle rectangle = new Rectangle(_defaultRectangleMin, _defaultRectangleMax);

        _navData.AddRectangle(rectangle);
        rectangleCell.Construct(rectangle.Min.x, rectangle.Min.y, rectangle.Max.x, rectangle.Max.y, _rectangleCells.Count);
        _rectangleCells.Add(rectangleCell);

        rectangleCell.MinXField.ValueChanged += ChangeRectangleMinX;
        rectangleCell.MinYField.ValueChanged += ChangeRectangleMinY;
        rectangleCell.MaxXField.ValueChanged += ChangeRectangleMaxX;
        rectangleCell.MaxYField.ValueChanged += ChangeRectangleMaxY;
    }

    public void OnRemoveRectangle()
    {
        if (int.TryParse(_rectangleIDToRemove.text, out int result))
        {
            if (_navData.TryRemoveRectangle(result))
            {
                Destroy(_rectangleCells[result].gameObject);
                _rectangleCells.RemoveAt(result);
                
                for (int i = 0; i < _rectangleCells.Count; i++)
                {
                    _rectangleCells[i].CellId = i;
                }
            }
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

    private void OnAddRectangle(Rectangle rectangle)
    {
        RectangleCell rectangleCell = Instantiate(_rectangleCellPrefab, _rectangleCellContainer.transform);
        rectangleCell.Construct(rectangle.Min.x, rectangle.Min.y, rectangle.Max.x, rectangle.Max.y, _rectangleCells.Count);
        _rectangleCells.Add(rectangleCell);

        rectangleCell.MinXField.ValueChanged += ChangeRectangleMinX;
        rectangleCell.MinYField.ValueChanged += ChangeRectangleMinY;
        rectangleCell.MaxXField.ValueChanged += ChangeRectangleMaxX;
        rectangleCell.MaxYField.ValueChanged += ChangeRectangleMaxY;
    }

    private void OnAddEdge(Edge edge)
    {
        int firstRectangleIndex = -1;
        int secondRectangleIndex = -1;

        for (int i = 0; i < _navData.Rectangles.Count; i++)
        {
            if (_navData.Rectangles[i].Equals(edge.First))
            {
                firstRectangleIndex = i;
            }

            if (_navData.Rectangles[i].Equals(edge.Second))
            {
                secondRectangleIndex = i;
            }

            if (firstRectangleIndex != -1 && secondRectangleIndex != -1)
            {
                break;
            }
        }

        EdgeCell edgeCell = Instantiate(_edgeCellPrefab, _edgeCellContainer.transform);
        edgeCell.Construct(
            edge.Start.x,
            edge.Start.y,
            edge.End.x,
            edge.End.y,
            firstRectangleIndex,
            secondRectangleIndex,
            _edgeCells.Count);
        _edgeCells.Add(edgeCell);

        edgeCell.StartXField.ValueChanged += ChangeEdgeStartX;
        edgeCell.StartYField.ValueChanged += ChangeEdgeStartY;
        edgeCell.EndXField.ValueChanged += ChangeEdgeEndX;
        edgeCell.EndYField.ValueChanged += ChangeEdgeEndY;
        edgeCell.FirstRectangleIdField.ValueChanged += ChangeEdgeFirst;
        edgeCell.SecondRectangleIdField.ValueChanged += ChangeEdgeSecond;
    }

    private void UpdateStartPositionX(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeStartPosition(result, _navData.StartPosition.y);
        }
    }

    private void UpdateStartPositionY(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeStartPosition(_navData.StartPosition.x, result);
        }
    }

    private void UpdateFinishPositionX(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeFinishPosition(result, _navData.FinishPosition.y);
        }
    }

    private void UpdateFinishPositionY(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeFinishPosition(_navData.FinishPosition.x, result);
        }
    }

    private void ChangeEdgeStartX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(
                id, 
                new Vector2(result, _navData.Edges[id].Start.y), 
                _navData.Edges[id].End, 
                _navData.Edges[id].First, 
                _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeStartY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(
                id, 
                new Vector2(_navData.Edges[id].Start.x, result), 
                _navData.Edges[id].End, 
                _navData.Edges[id].First, 
                _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeEndX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(
                id, 
                _navData.Edges[id].Start, 
                new Vector2(result, 
                _navData.Edges[id].End.y), 
                _navData.Edges[id].First, 
                _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeEndY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeEdge(
                id, 
                _navData.Edges[id].Start, 
                new Vector2(_navData.Edges[id].End.x, result), 
                _navData.Edges[id].First, 
                _navData.Edges[id].Second);
        }
    }

    private void ChangeEdgeFirst(string value, int id)
    {
        if (int.TryParse(value, out int result))
        {
            if (result < _navData.Rectangles.Count && result >= 0)
            {
                _navData.ChangeEdge(
                    id, 
                    _navData.Edges[id].Start, 
                    _navData.Edges[id].End, 
                    _navData.Rectangles[result], 
                    _navData.Edges[id].Second);
            }
        }
    }

    private void ChangeEdgeSecond(string value, int id)
    {
        if (int.TryParse(value, out int result))
        {
            if (result < _navData.Rectangles.Count && result >= 0)
            {
                _navData.ChangeEdge(
                    id, 
                    _navData.Edges[id].Start, 
                    _navData.Edges[id].End, 
                    _navData.Edges[id].First, 
                    _navData.Rectangles[result]);
            }
        }
    }
    private void ChangeRectangleMinX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(
                id,
                result,
                _navData.Rectangles[id].Min.y,
                _navData.Rectangles[id].Max.x,
                _navData.Rectangles[id].Max.y);
        }
    }

    private void ChangeRectangleMinY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(
                id,
                _navData.Rectangles[id].Min.x,
                result,
                _navData.Rectangles[id].Max.x,
                _navData.Rectangles[id].Max.y);
        }
    }

    private void ChangeRectangleMaxX(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(
                id,
                _navData.Rectangles[id].Min.x,
                _navData.Rectangles[id].Min.y,
                result,
                _navData.Rectangles[id].Max.y);
        }
    }

    private void ChangeRectangleMaxY(string value, int id)
    {
        if (float.TryParse(value, out float result))
        {
            _navData.ChangeRectangle(
                id,
                _navData.Rectangles[id].Min.x,
                _navData.Rectangles[id].Min.y,
                _navData.Rectangles[id].Max.x,
                result);
        }
    }
}
