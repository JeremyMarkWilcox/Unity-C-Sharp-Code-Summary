using UnityEngine;
using Cinemachine;

public class ZoomController : MonoBehaviour
{
    public float zoomSpeed = 20f;
    public float minZoom = 70f;
    public float maxZoom = 120f;

    private float currentZoomLevel = 5.0f; 
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoomLevel -= zoomInput * zoomSpeed;
        currentZoomLevel = Mathf.Clamp(currentZoomLevel, minZoom, maxZoom);
        UpdateZoom();
    }

    void UpdateZoom()
    {
        if (virtualCamera != null)
        {
            float newFieldOfView = Mathf.Lerp(minZoom, maxZoom, currentZoomLevel / maxZoom);
            virtualCamera.m_Lens.FieldOfView = newFieldOfView;
        }
    }
}
