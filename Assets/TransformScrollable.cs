using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TransformScrollable : MonoBehaviour, IScrollHandler
{
    public Transform transformToScroll;
    private IScrollHandler scrollHandler;

    void Start()
    {
        if (transformToScroll == null)
            transformToScroll = transform.parent;

        scrollHandler = transformToScroll.GetComponent<IScrollHandler>();
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (transformToScroll == transform || eventData == null) return;

        if (scrollHandler == null)
            scrollHandler = transformToScroll.GetComponent<IScrollHandler>();

        scrollHandler.OnScroll(eventData);
    }

}
