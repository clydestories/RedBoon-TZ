using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _offset;
    private Vector3 _origin;
    private bool _isDragging = false;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            _offset = _camera.ScreenToWorldPoint(Input.mousePosition) - _camera.transform.position;
            if (_isDragging == false)
            {
                _isDragging = true;
                _origin = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            _camera.transform.position = _origin - _offset;
        }
    }
}
