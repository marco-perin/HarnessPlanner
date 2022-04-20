using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Types;
using Assets.UXData.Interfaces;
using UnityEngine;



public class ConnectibleManager : MonoBehaviourGraphicInstanced, IClickable
{
    [Header("Runtime Variables")]
    public bool connecting;

    public bool IsConnecting { get => connecting; }

    private void Start()
    {
        InputManager.Instance.AddAction(KeyCode.C, () => StartConnecting());
        InputManager.Instance.AddAction(KeyCode.D, () => StopConnecting());
    }

    private void StartConnecting()
    {
        ConnectionsManagerSingleton.Instance.ResetConnectionState();
        connecting = true;
    }

    private void StopConnecting()
    {
        ConnectionsManagerSingleton.Instance.ResetConnectionState();
        connecting = false;
    }

    public void Click()
    {
        if (IsConnecting)
            ConnectionsManagerSingleton.Instance.Connect(this, null);

    }

}
