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

    void Update()
    {
        if (!Input.anyKeyDown) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            connecting = true;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            ConnectionsManagerSingleton.Instance.ResetConnectionState();
            connecting = false;
        }

    }

    public void Click()
    {
        if (IsConnecting)
            ConnectionsManagerSingleton.Instance.Connect(this, null);

    }

}
