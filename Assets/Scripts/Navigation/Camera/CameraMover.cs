using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraMover : MonoBehaviour
{
    [SerializeField] private InputReader _input;

    private Camera _camera;
    private Vector3 _offset;
    private Vector3 _origin;
    private bool _isDragging = false;
    private bool _isMoving = false;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        _input.Moved += Move;
    }

    private void OnDisable()
    {
        _input.Moved -= Move;
    }

    private void LateUpdate()
    {
        _isDragging = _isMoving;

        if (_isDragging)
        {
            _camera.transform.position = _origin - _offset;
        }

        _isMoving = false;
    }

    private void Move(Vector3 mousePosition)
    {
        _isMoving = true;
        _offset = _camera.ScreenToWorldPoint(mousePosition) - _camera.transform.position;
        if (_isDragging == false)
        {
            _isDragging = true;
            _origin = _camera.ScreenToWorldPoint(mousePosition);
        }
    }
}
