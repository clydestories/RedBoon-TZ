using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraZoomer : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private InputReader _input;

    private Camera _camera;
    private float _minCameraSize = 0.1f;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        _input.Zoomed += Zoom;
    }

    private void OnDisable()
    {
        _input.Zoomed -= Zoom;
    }

    private void Zoom(float amount)
    {
        if (_eventSystem.IsPointerOverGameObject() == false)
        {
            _camera.orthographicSize -= amount;

            if (_camera.orthographicSize < _minCameraSize)
            {
                _camera.orthographicSize = _minCameraSize;
            }
        }
    }
}
