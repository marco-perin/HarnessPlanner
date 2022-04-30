using UnityEngine;
using UnityEngine.EventSystems;

public class GraphicInstanceDraggable : MonoBehaviourGraphicInstanced, IDragHandler, IBeginDragHandler
{
    public float dragSpeed = 1;

    public Vector3 targetPosition;
    private float screenScale = 0.01f;

    public bool dragOtherTransform = false;
    public Transform transformToDrag;
    public PointerEventData.InputButton dragButton = PointerEventData.InputButton.Left;

    public float SnapGridSize { get => ProgramManagerSingleton.Instance.HarnessSettingsSO.SnapGridSize; }

    Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    void Start()
    {
        if (transformToDrag == null)
            transformToDrag = transform.parent;

        targetPosition = GraphicInstance.Position;
    }

    void Update()
    {
        if (dragOtherTransform) return;

        GraphicInstance.Position = Vector3.Lerp(Position, targetPosition, Time.deltaTime * dragSpeed * 10);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != dragButton) return;
        if (!dragOtherTransform)
        {
            if (eventData.pointerCurrentRaycast.isValid)
            {
                targetPosition = transform.parent.InverseTransformPoint((Vector2)eventData.pointerCurrentRaycast.worldPosition);
                targetPosition.x = Mathf.Round(targetPosition.x / SnapGridSize) * SnapGridSize;
                targetPosition.y = Mathf.Round(targetPosition.y / SnapGridSize) * SnapGridSize;
            }
            else
                targetPosition += transform.parent.InverseTransformVector((Vector3)eventData.delta * screenScale);
        }
        else
            transformToDrag.GetComponent<IDragHandler>().OnDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragOtherTransform)
            transformToDrag.GetComponent<IBeginDragHandler>().OnBeginDrag(eventData);
        else
            screenScale = eventData.enterEventCamera.orthographicSize * 2 / eventData.enterEventCamera.pixelHeight;
    }
}
