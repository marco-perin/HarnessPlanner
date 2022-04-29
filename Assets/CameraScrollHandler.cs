using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScrollHandler : MonoBehaviour, IScrollHandler
{
    public Camera cameraToScroll;
    public float scrollSpeed = 1;

    public float sizeMin = 1;
    public float sizeMax = 100;


    // Start is called before the first frame update
    void Start()
    {
        if (cameraToScroll == null)
            cameraToScroll = Camera.main;

    }

    public void OnScroll(PointerEventData eventData)
    {
        cameraToScroll.orthographicSize -= eventData.scrollDelta.y * scrollSpeed / 10;
        cameraToScroll.orthographicSize = Mathf.Clamp(cameraToScroll.orthographicSize, sizeMin, sizeMax);
    }
}
