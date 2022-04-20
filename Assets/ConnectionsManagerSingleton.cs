using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using UnityEngine;

public enum ConnectionState
{
    None = 0,
    Started = 1,
    Connected = 2
}

public class ConnectionsManagerSingleton : Singleton<ConnectionsManagerSingleton>
{
    public ConnectionState connectionState;


    public ConnectibleManager connectFrom;
    public ConnectibleManager connectTo;


    public List<GraphicConnection> connections;


    public void ResetConnectionState()
    {
        connectionState = ConnectionState.None;
    }

    public void Connect(ConnectibleManager connManager, Transform connection_transform = null)
    {
        switch (connectionState)
        {
            case ConnectionState.None:

                connectFrom = connManager;

                connectionState++;
                break;
            case ConnectionState.Started:

                connectTo = connManager;

                connectionState++;
                break;

        }
    }

    public void Update()
    {
        if (connectionState == ConnectionState.Connected)
        {
            Debug.Log($"Connecting {((INode)connectFrom.GraphicInstance.BaseWrapped).BaseSO.Name} to {((INode)connectTo.GraphicInstance.BaseWrapped).BaseSO.Name}");

            connections.Add(new GraphicConnection(connectFrom, connectTo));

            // Finish up connection 
            connectionState = ConnectionState.None;
        }
    }


    private void OnDrawGizmos()
    {
        foreach (GraphicConnection c in connections)
        {
            Gizmos.DrawLine(c.From.transform.position + Vector3.forward * 0.01f, c.To.transform.position + Vector3.forward * 0.01f) ;
        }
    }


    [Serializable]
    public class GraphicConnection
    {
        public ConnectibleManager From;
        public ConnectibleManager To;

        public GraphicConnection(ConnectibleManager from, ConnectibleManager to)
        {
            From = from;
            To = to;
        }
    }
}
