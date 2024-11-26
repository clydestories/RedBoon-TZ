using UnityEngine;

public class RectangleCell : IdBoundCell
{
    [SerializeField] private IdBoundInputField _minXField;
    [SerializeField] private IdBoundInputField _minYField;
    [SerializeField] private IdBoundInputField _maxXField;
    [SerializeField] private IdBoundInputField _maxYField;

    public IdBoundInputField MinXField => _minXField;
    public IdBoundInputField MinYField => _minYField;
    public IdBoundInputField MaxXField => _maxXField;
    public IdBoundInputField MaxYField => _maxYField;

    public void Construct(float minX, float minY, float maxX, float maxY, int index)
    {
        _minXField.InputField.text = minX.ToString();
        _minYField.InputField.text = minY.ToString();
        _maxXField.InputField.text = maxX.ToString();
        _maxYField.InputField.text = maxY.ToString();
        Id = index;
    }
}
