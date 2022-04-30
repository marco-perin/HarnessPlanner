using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private float screenScale = 0.01f;

    public bool dragOtherTransform = false;
    public Transform transformToDrag;
    public PointerEventData.InputButton dragButton = PointerEventData.InputButton.Left;
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != dragButton) return;
        //var cameraPos = Camera.main.transform.position;

        //var pos = cameraPos;

        //if (eventData.pointerCurrentRaycast.isValid)
        //    transform.position = (Vector2)eventData.pointerCurrentRaycast.worldPosition;
        //else
        if (dragOtherTransform)
        {
            transformToDrag.GetComponent<IDragHandler>().OnDrag(eventData);
        }
        else
            transform.position += (Vector3)eventData.delta * screenScale;


        //Camera.main.transform.position = new Vector3(pos.x, pos.y, cameraPos.z);

    }
    void Start()
    {

        if (transformToDrag == null)
            transformToDrag = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragOtherTransform)
            transformToDrag.GetComponent<IBeginDragHandler>().OnBeginDrag(eventData);
        else
            screenScale = eventData.enterEventCamera.orthographicSize * 2 / eventData.enterEventCamera.pixelHeight;
    }
}
