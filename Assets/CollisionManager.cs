using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.UXData.Interfaces;
using UnityEngine;

enum EventSystemState
{
    Idle,
    Dragging
}

public enum InteractableActionType
{
    None = 0,
    Start,
    End,
    Focus,
    UnFocus,
    Drag,
    Click
}

public class CollisionManager : MonoBehaviour
{
    public Transform lastInteractable;
    public Transform lastInteractableForDragging;
    public string lastInteractableName;

    [Range(0, 3)]
    public int mouseButton = 0;

    public Vector3 prevMouseWorldPos = Vector3.zero;

    public bool evaluatingDragging = false;
    public bool dragging = false;

    public float draggingThreshold = 10f;
    public LayerMask hitLayerMask;

    public Transform LastInteractable
    {
        get => lastInteractable;
        set
        {
            lastInteractable = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //prevMouseWorldPos = transform.position;
    }

    public Transform currInteractable;
    public Transform currentOrLastInteractable;
    public Transform currentOrLastInteractableForDragging;

    bool isOkForClick;
    public Vector3 dx = Vector3.zero;

    // Update is called once per frame
    void Update()
    {


        var ray = GetMouseRay();
        var pos = GetMousePosOnWorld(ray);

        if (dragging)
            dx = pos - prevMouseWorldPos;
        else
            dx += pos - prevMouseWorldPos;

        bool hasCollided = Physics.Raycast(ray, out RaycastHit hitInfo);
        bool hasStarted = false;

        //bool hasCollided = Physics.Raycast(ray, out RaycastHit hitInfo, hitLayerMask);

        //var allCollisions = Physics.RaycastAll(ray);

        //RaycastHit? hitInfo = null;

        //bool hasCollided = allCollisions?.Length > 0;

        //// TODO: This is inefficient AF, change this
        //if (hasCollided) allCollisions = allCollisions.OrderByDescending(c => c.distance).ToArray();

        //if (hasCollided) hitInfo = allCollisions[0];


        //Debug.Log("Collided with obj " + hitInfo.transform.gameObject.name);
        //var currInteractable = hasCollided ? hitInfo?.transform.GetComponent<IInteractable>() : null;
        currInteractable = hasCollided ? hitInfo.transform : null;

        currentOrLastInteractable = hasCollided && !dragging ? currInteractable : LastInteractable;
        currentOrLastInteractableForDragging = hasCollided && !evaluatingDragging ? currInteractable : lastInteractableForDragging;

        if (currentOrLastInteractable == null)
        {
            Debug.Log("currentOrLastInteractable was null");
            return;
        }

        if (LastInteractable != currentOrLastInteractable)
        {
            if (!dragging && currentOrLastInteractable != null)
                LastInteractable = currentOrLastInteractable;
        }

        if (lastInteractableForDragging != currentOrLastInteractableForDragging)
        {
            if (!evaluatingDragging && currentOrLastInteractableForDragging != null)
                lastInteractableForDragging = currentOrLastInteractableForDragging;
        }

        if (hasCollided)
            if (Input.GetMouseButtonDown(mouseButton))
            {
                currInteractable.InteractForTransform(InteractableActionType.Start, pos);

                //if (currentOrLastInteractable is IClickable<Vector3>)
                //    (currentOrLastInteractable as IClickable<Vector3>).Click(pos);
                hasStarted = true;
                isOkForClick = true;
                dx = Vector3.zero;
            }

        if (Input.GetMouseButtonUp(mouseButton))
        {
            currentOrLastInteractable.InteractForTransform(InteractableActionType.End, pos);
            if (isOkForClick)
                currentOrLastInteractable.InteractForTransform(InteractableActionType.Click, pos);


        }

        if (Input.GetMouseButton(mouseButton)
            && !hasStarted
            )
        {
            evaluatingDragging = true;
            if (dragging || dx.magnitude > draggingThreshold)
            {
                dragging = currentOrLastInteractable.InteractForTransform(InteractableActionType.Drag, dx);
                evaluatingDragging = false;
            }
        }
        else { dragging = false; }

        if (dragging)
            isOkForClick = false;

        if (currInteractable == null)
            currentOrLastInteractable.InteractForTransform(InteractableActionType.UnFocus);

        prevMouseWorldPos = pos;
    }

    private Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private Vector3 GetMousePosOnWorld(Ray ray)
    {
        return ray.GetPoint(-Camera.main.transform.position.z);
    }
}

public static class IInteractableExtensions
{
    public static bool InteractForTransform(this Transform transform, InteractableActionType actionType, Vector3? param = null)
    {

        Debug.Assert(transform != null);

        param ??= Vector3.zero;
        return transform.GetComponents<IInteractable>().InteractForList(actionType, param.Value);
    }

    public static bool InteractForList(this IEnumerable<IInteractable> interactables, InteractableActionType actionType, Vector3 param)
    {
        Debug.Assert(interactables != null);
        Debug.Assert(interactables.Count() > 0);

        switch (actionType)
        {
            case InteractableActionType.Start:
                foreach (var interactable in interactables)
                    interactable.StartInteraction(param);
                break;
            case InteractableActionType.End:
                foreach (var interactable in interactables)
                    interactable.Endinteraction();
                break;

            case InteractableActionType.Focus:
                foreach (var interactable in interactables)
                    interactable.Focus();
                break;

            case InteractableActionType.UnFocus:
                foreach (var interactable in interactables)
                    interactable.UnFocus();
                break;

            case InteractableActionType.Drag:
                foreach (var interactable in interactables)
                    if (interactable.Drag(param)) return true;
                break;

            case InteractableActionType.Click:
                foreach (var interactable in interactables)
                    interactable.Click(param);
                break;

        }
        return false;
    }

    public static void StartInteraction(this IInteractable interactable, Vector3 pos)
    {

        Debug.Assert(interactable != null);

        switch (interactable)
        {
            case IInteractionStartableV3 iv3:
                Debug.Assert(pos != null);
                iv3.StartInteraction(pos);
                break;
            case IInteractionStartable i:
                i.StartInteraction();
                break;
        }

        //if (interactable is IInteractionStartableV3)
        //    (interactable as IInteractionStartableV3).StartInteraction(pos);

        //else if (interactable is IInteractionStartable)
        //    (interactable as IInteractionStartable).StartInteraction();

    }

    public static void Click(this IInteractable interactable, Vector3 pos)
    {
        Debug.Assert(interactable != null);

        switch (interactable)
        {
            case IClickableV3 iv3:
                iv3.Click(pos);
                break;
            case IClickable i:
                i.Click();
                break;
        }
        //    if (interactable is IClickableV3)
        //        (interactable as IClickableV3).Click(pos);

        //    else if (interactable is IClickable)
        //        (interactable as IClickable).Click();

    }

    public static void Endinteraction(this IInteractable interactable)
    {
        Debug.Assert(interactable != null);

        if (interactable is IInteractionEndable)
            (interactable as IInteractionEndable).EndInteraction();
    }

    public static void Focus(this IInteractable interactable)
    {
        Debug.Assert(interactable != null);

        if (interactable is IHoverable)
            (interactable as IHoverable).Focus();
    }

    public static void UnFocus(this IInteractable interactable)
    {
        if (interactable is IHoverable)
            (interactable as IHoverable).UnFocus();
    }

    public static bool Drag(this IInteractable interactable, Vector3 dx)
    {
        if (interactable is IDraggable)
        {
            (interactable as IDraggable).Drag(dx);
            return true;
        }

        return false;
    }
}
