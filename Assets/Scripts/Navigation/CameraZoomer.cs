using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraZoomer : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            if (_eventSystem.IsPointerOverGameObject() == false)
            {
                _camera.orthographicSize -= Input.mouseScrollDelta.y;

                if (_camera.orthographicSize < 0.1f)
                {
                    _camera.orthographicSize = 0.1f;
                }
            }
        }
    }
}
