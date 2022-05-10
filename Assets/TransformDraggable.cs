using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private float screenScale = 0.01f;

    public bool dragOtherTransform = false;
    public Transform transformToDrag;
    public PointerEventData.InputButton dragButton = PointerEventData.InputButton.Left;

    public float dragSmoothness = 1;
    private Vector3 targetPos;
    public Vector3 TargetPosition
    {
        get => targetPos;
        set
        {
            targetPos = value;
            if (!tracking || finishedDragging)
                BeginDrag();
        }
    }

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
            targetPos += (Vector3)eventData.delta * screenScale;


        //Camera.main.transform.position = new Vector3(pos.x, pos.y, cameraPos.z);

    }

    void Start()
    {

        if (transformToDrag == null)
            transformToDrag = transform;

        //targetPos = transformToDrag.position;
    }
    bool tracking = false;
    bool finishedDragging = true;

    private void Update()
    {
        if (finishedDragging && (transform.position - targetPos).sqrMagnitude < 1e-3)
            tracking = false;

        if (!tracking) return;

        transformToDrag.position = Vector3.Lerp(transformToDrag.position, targetPos, Time.deltaTime / dragSmoothness * 15);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragOtherTransform)
        {
            transformToDrag.GetComponent<IBeginDragHandler>().OnBeginDrag(eventData);
            return;
        }
        screenScale = eventData.enterEventCamera.orthographicSize * 2 / eventData.enterEventCamera.pixelHeight;
        BeginDrag();

    }

    private void BeginDrag()
    {
        targetPos = transformToDrag.position;
        tracking = true;
        finishedDragging = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragOtherTransform)
        {
            transformToDrag.GetComponent<IEndDragHandler>().OnEndDrag(eventData);
            return;
        }
        finishedDragging = true;
    }

    public void StopTracking()
    {
        tracking = false;
    }
}
