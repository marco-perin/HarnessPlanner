using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviourGraphicInstanced, IDragHandler, IBeginDragHandler
{
    public float dragSpeed = 1;

    private Vector3 targetPosition;
    private float screenScale = 0.01f;

    void Start()
    {
        targetPosition = GraphicInstance.Position;
    }

    void Update()
    {
        GraphicInstance.Position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dragSpeed * 10);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging " + gameObject.name);
        if (eventData.pointerCurrentRaycast.isValid)
            targetPosition = (Vector2)eventData.pointerCurrentRaycast.worldPosition;
        else
            targetPosition += (Vector3)eventData.delta * screenScale;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        screenScale = eventData.enterEventCamera.orthographicSize * 2 / eventData.enterEventCamera.pixelHeight;
    }
}
