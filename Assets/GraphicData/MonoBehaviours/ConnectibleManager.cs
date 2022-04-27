using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Types;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectibleManager : MonoBehaviourGraphicInstanced, IPointerClickHandler
{
    [Header("Runtime Variables")]
    public bool connecting;

    public bool IsConnecting { get => connecting; }

    private void Start()
    {
        InputManager.Instance.AddKeyDownAction(KeyCode.C, () => StartConnecting());
        InputManager.Instance.AddKeyDownAction(KeyCode.V, () => StopConnecting());
    }

    private void StartConnecting()
    {
        MainConnectionsManagerSingleton.Instance.ResetConnectionState();
        connecting = true;
    }

    private void StopConnecting()
    {
        MainConnectionsManagerSingleton.Instance.ResetConnectionState();
        connecting = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsConnecting) return;

        float dragDistance = Vector2.Distance(eventData.pressPosition, eventData.position);
        float dragThreshold = 10f;
        bool isDrag = dragDistance > dragThreshold;

        if (isDrag)
            return;

        MainConnectionsManagerSingleton.Instance.Connect(this);
    }
}
