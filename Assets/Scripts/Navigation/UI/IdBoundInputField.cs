using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(InputField))]
public class IdBoundInputField : MonoBehaviour
{
    [SerializeField] private IdBoundCell _cell;

    private InputField _inputField;

    public event Action<string, int> ValueChanged;

    public InputField InputField => _inputField;

    private void Awake()
    {
        _inputField = GetComponent<InputField>();
    }

    private void OnEnable()
    {
        _inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        _inputField.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        ValueChanged?.Invoke(value, _cell.CellId);
    }
}
