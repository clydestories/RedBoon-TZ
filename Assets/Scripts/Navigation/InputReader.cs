using System;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private List<KeyCode> _moveOnHoldKeys;

    public event Action<Vector3> Moved;
    public event Action<float> Zoomed;

    private void LateUpdate()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Zoomed?.Invoke(Input.mouseScrollDelta.y);
        }

        foreach (KeyCode keyCode in _moveOnHoldKeys)
        {
            if (Input.GetKey(keyCode))
            {
                Moved?.Invoke(Input.mousePosition);
            }
        }
    }
}
