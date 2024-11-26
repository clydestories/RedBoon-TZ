using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoomer : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            _camera.orthographicSize -= Input.mouseScrollDelta.y;

            if (_camera.orthographicSize < 0)
            {
                _camera.orthographicSize = 0;
            }
        }
    }
}
