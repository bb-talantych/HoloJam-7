using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    public float zoomSpeed = 10;
    public float zoomSmoothness = 4;
    public float minZoom = -4;
    public float maxZoom = 4;
    public Camera _camera;
    private float currentZoom;
    

    void Start()
    {
        currentZoom = _camera.orthographicSize;
    }


    void Update()
    {
        currentZoom = Mathf.Clamp(currentZoom - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, currentZoom, zoomSmoothness*Time.deltaTime); 
    }
}
