using UnityEngine;

public class EdgeCell : IdBoundCell
{
    [SerializeField] private IdBoundInputField _startXField;
    [SerializeField] private IdBoundInputField _startYField;
    [SerializeField] private IdBoundInputField _endXField;
    [SerializeField] private IdBoundInputField _endYField;
    [SerializeField] private IdBoundInputField _firstRectangleIdField;
    [SerializeField] private IdBoundInputField _secondRectangleIdField;

    public IdBoundInputField StartXField => _startXField;
    public IdBoundInputField StartYField => _startYField;
    public IdBoundInputField EndXField => _endXField;
    public IdBoundInputField EndYField => _endYField;
    public IdBoundInputField FirstRectangleIdField => _firstRectangleIdField;
    public IdBoundInputField SecondRectangleIdField => _secondRectangleIdField;

    public void Construct(float startX, float startY, float endX, float endY, int firstRectangleId, int secondRectangleId, int index)
    {
        _startXField.InputField.text = startX.ToString();
        _startYField.InputField.text = startY.ToString();
        _endXField.InputField.text = endX.ToString();
        _endYField.InputField.text = endY.ToString();
        _firstRectangleIdField.InputField.text = firstRectangleId.ToString();
        _secondRectangleIdField.InputField.text = secondRectangleId.ToString();
        Id = index;
        IdText.text = Id.ToString();
    }
}
