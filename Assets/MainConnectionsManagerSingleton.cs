using System;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
using UnityEngine;

public enum ConnectionState
{
    None = 0,
    Started = 1,
    Connected = 2
}

public class MainConnectionsManagerSingleton : Singleton<MainConnectionsManagerSingleton>
{
    public ConnectionState connectionState;

    public GraphicHarnessSettingsSO harnessSettings;
    public ConnectibleManager connectFrom;
    public ConnectibleManager connectTo;


    public List<GraphicConnection> connections;

    public Transform connectionsParent;

    public void ResetConnectionState()
    {
        continousConnection = false;
        connectionState = ConnectionState.None;
    }

    private bool continousConnection = false;

    private void Start()
    {
        InputManager.Instance.AddKeyDownAction(KeyCode.LeftShift, () =>
        {
            continousConnection = true;
            if (connectFrom != null)
                connectionState = ConnectionState.Started;
        });

        InputManager.Instance.AddKeyUpAction(KeyCode.LeftShift, () =>
        {
            continousConnection = false;
            connectionState = ConnectionState.None;
        });
    }

    public IEnumerable<INodeLinkBase> ActiveConnections
    {
        get => connectionsParent
                        .GetComponentsInChildren<GraphicalSOSync>()
                        .Where(mbgi => mbgi.GraphicInstance is NodeLinkBaseGraphicBaseWrapper nlbgi)
                        .Select(mbgi => mbgi.GraphicInstance.BaseWrapped as NodeLinkBase);
    }

    public List<INode> GetNodesConnectedToNode(INode currentNode)
    {

        var result = GetConnectedNodesRecursive(currentNode);

        //if (incidentEdges.Any())
        //    Debug.Log($"incidentEdges = {incidentEdges.Select(e => $"[{e.FromNode.BaseWrapped.Id}] <-> [{e.ToNode.BaseWrapped.Id}]").Aggregate((n, curr) => n + "\n" + curr)}");
        if (result.Any())
            Debug.Log($"connectedNodes = {result.Select(n => n.Name).Aggregate((curr, newNode) => $"{curr}, [{newNode}]")}");
        return result;
    }

    private List<INode> GetConnectedNodesRecursive(INode currentNode)
    {
        // Temp dict
        Dictionary<string, INode> visitedNodes = new();

        return GetConnectedNodesRecursive(currentNode, ref visitedNodes);
    }

    private List<INode> GetConnectedNodesRecursive(INode currentNode, ref Dictionary<string, INode> visitedNodes)
    {
        var edges = ActiveConnections.ToList();
        var incidentEdges = edges
            .Where(e => e.FromNode.Id == currentNode.Id).ToList();

        var reversedIncidentEdges = edges
                .Where(e => e.ToNode.Id == currentNode.Id).ToList();

        reversedIncidentEdges = reversedIncidentEdges.Select(e => e.SwappedEdges).ToList();

        incidentEdges = incidentEdges.Union(reversedIncidentEdges).ToList();

        var result = new List<INode>();

        // Happens only if the user clicks on a non connected node
        if (!incidentEdges.Any()) return result;

        foreach (var edge in incidentEdges)
        {
            // Node Already visited
            if (visitedNodes.ContainsKey(edge.ToNode.Id))
                continue;

            // Node not visited, add it
            INode toNode = edge.ToNode.BaseWrapped as INode;
            result.Add(toNode);
            visitedNodes[edge.ToNode.Id] = toNode;

            result.AddRange(GetConnectedNodesRecursive(toNode, ref visitedNodes));
        }

        return result;
    }

    public void Connect(ConnectibleManager connManager)
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
            //Debug.Log($"Connecting {((INode)connectFrom.GraphicInstance.BaseWrapped).BaseSO.Name} to {((INode)connectTo.GraphicInstance.BaseWrapped).BaseSO.Name}");
            var from = connectFrom;
            var to = connectTo;

            CreateConnection(from, to);
            if (!continousConnection)
                // Finish up connection 
                connectionState = ConnectionState.None;
            else
            {
                connectFrom = connectTo;
                connectionState = ConnectionState.Started;
            }
        }
    }

    private void CreateConnection(ConnectibleManager from, ConnectibleManager to)
    {

        IGraphicInstance wrapper = new NodeLinkBaseGraphicBaseWrapper()
        {
            BaseWrapped = new NodeLinkBase(harnessSettings.DefaultLinkPrefab)
            {
                Length = 0,
                FromNode = from.GraphicInstance,
                ToNode = to.GraphicInstance
            },
            Position = Vector3.forward * harnessSettings.ConnectionsPlaceHeight
        };

        HarnessPlacer.CreateGraphicWrapper(wrapper, connectionsParent);
    }


    private void OnDrawGizmos()
    {
        foreach (GraphicConnection c in connections)
        {
            Gizmos.DrawLine(c.From.transform.position + Vector3.forward * 0.01f, c.To.transform.position + Vector3.forward * 0.01f);
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
