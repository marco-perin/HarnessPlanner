using System.Collections;
using System.Collections.Generic;
using Assets.GraphicData.Interfaces;
//using Assets.UXData.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviourGraphicInstanced, IDragHandler, IBeginDragHandler//, IDraggable
{
    public float dragSpeed = 1;
    Vector3 targetPosition;

    public void StartInteraction(Vector3 param)
    {

    }

    //public void Drag(Vector3 dx)
    //{

    //    Debug.Log("Start Drag");
    //    targetPosition += dx;
    //}

    // Start is called before the first frame update
    void Start()
    {

        targetPosition = GraphicInstance.Position;
    }

    // Update is called once per frame
    void Update()
    {
        //if ((targetPosition - transform.position).sqrMagnitude > 1e-3)
        //transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime);
        //errq = (targetPosition - GraphicInstance.Position).sqrMagnitude;
        //GraphicInstance.Position = targetPosition;
        GraphicInstance.Position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dragSpeed * 10);
    }

    public void OnDrag(PointerEventData eventData)
    {


        Debug.Log("Dragging " + gameObject.name);
        if (eventData.pointerCurrentRaycast.isValid)
            targetPosition = (Vector2)eventData.pointerCurrentRaycast.worldPosition;
        else
            targetPosition += (Vector3)eventData.delta * scale;
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Debug.Log($"Entering OnPOinterenter on go {gameObject.name}");
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(targetPosition, 5f * transform.localScale.x);
    //}
    //private float distance;
    //private Plane dragPlane;

    private float scale = 0.01f;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //distance = eventData.pointerCurrentRaycast.distance;
        //var n = eventData.pointerCurrentRaycast.worldPosition - eventData.enterEventCamera.transform.position;
        //dragPlane = new Plane(n, distance);
        //scale = 0.01f;
        scale = eventData.enterEventCamera.orthographicSize * 2 / eventData.enterEventCamera.pixelHeight;
        //Debug.Log($"Scale = {eventData.enterEventCamera.orthographicSize * 2} / {eventData.enterEventCamera.pixelHeight}  = {scale}[pxpu]");
        //Debug.Log($"Scale = {eventData.enterEventCamera.orthographicSize * 2} / {eventData.enterEventCamera.pixelWidth}  = {eventData.enterEventCamera.orthographicSize / eventData.enterEventCamera.pixelWidth}[pxpu]");
    }
}
