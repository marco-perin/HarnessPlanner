using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScrollHandler : MonoBehaviour, IScrollHandler
{
    public Camera cameraToScroll;
    public float scrollSpeed = 1;
    public float zoomSmoothness = 1;

    public float sizeMin = 1;
    public float sizeMax = 100;

    public float sizeTarget = 10;


    // Start is called before the first frame update
    void Start()
    {
        if (cameraToScroll == null)
            cameraToScroll = Camera.main;

        sizeTarget = cameraToScroll.orthographicSize;
    }

    public bool IsScolling { get; set; }

    private void Update()
    {
        if (Mathf.Abs(cameraToScroll.orthographicSize - sizeTarget) < 1e-2)
            IsScolling = false;

        if (!IsScolling) return;

        cameraToScroll.orthographicSize = Mathf.Lerp(cameraToScroll.orthographicSize, sizeTarget, Time.deltaTime * 30 / zoomSmoothness);
    }

    public void OnScroll(PointerEventData eventData)
    {
        IsScolling = true;
        sizeTarget *= 1 - eventData.scrollDelta.y * scrollSpeed / 10;
        sizeTarget = Mathf.Clamp(sizeTarget, sizeMin, sizeMax);
    }
}
