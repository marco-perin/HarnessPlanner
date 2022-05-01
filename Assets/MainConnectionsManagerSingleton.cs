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

    //public GraphicHarnessSettingsSO harnessSettings;
    public ConnectibleManager connectFrom;
    public ConnectibleManager connectTo;


    public List<GraphicConnection> connections;

    public Transform connectionsParent;

    private bool continousConnection = false;


    public HarnessSettingSOLoaded HarnessSettings => ProgramManagerSingleton.Instance.HarnessSettingsSO;
    public GraphicHarnessSettingsSO HarnessSettingsAddressable => ProgramManagerSingleton.Instance.HarnessSettingsSOAddressables;

    public void ResetConnectionState()
    {
        continousConnection = false;
        connectionState = ConnectionState.None;
    }


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

    internal void DeleteConnection(GraphicalSOSync connGO, INodeLinkBase conn)
    {
        Destroy(connGO.gameObject);
    }

    public IEnumerable<GraphicalSOSync> ActiveConnectionsGO
    {
        get => connectionsParent
                        .GetComponentsInChildren<GraphicalSOSync>()
                        .Where(mbgi => mbgi.GraphicInstance is NodeLinkBaseGraphicBaseWrapper);
    }

    public IEnumerable<INodeLinkBase> ActiveConnections
    {
        get => ActiveConnectionsGO.Select(mbgi => mbgi.GraphicInstance.BaseWrapped as NodeLinkBase);
    }

    #region GRAPH_ALGORITHMS

    public IEnumerable<IGraphicInstance> GetNodesConnectedToNode(IGraphicInstance currentNode)
    {

        var result = GetConnectedNodesRecursive(currentNode);

        //if (incidentEdges.Any())
        //    Debug.Log($"incidentEdges = {incidentEdges.Select(e => $"[{e.FromNode.BaseWrapped.Id}] <-> [{e.ToNode.BaseWrapped.Id}]").Aggregate((n, curr) => n + "\n" + curr)}");
        //if (result.Any())
        //    Debug.Log($"connectedNodes = {result.Select(n => (n.BaseWrapped as INode).Name).Aggregate((curr, newNode) => $"{curr}, [{newNode}]")}");

        return result;
    }


    /// <summary>
    /// Returns the node connected to the provided <paramref name="currentNode"/>,
    /// returning a dictionary representing the paths taken up to a certain node.
    /// </summary>
    /// <param name="currentNode"></param>
    /// <param name="visitedNodesWithPathDict"></param>
    /// <returns></returns>
    public IEnumerable<IGraphicInstance> GetNodesConnectedToNodeWithPaths(IGraphicInstance currentNode, out Dictionary<IGraphicInstance, IEnumerable<INodeLinkBase>> visitedNodesWithPathDict)
    {
        return GetConnectedNodesWithPaths(currentNode, out visitedNodesWithPathDict);
    }

    private IEnumerable<IGraphicInstance> GetConnectedNodesRecursive(IGraphicInstance currentNode)
    {
        return GetConnectedNodesWithPaths(currentNode, out _);
    }

    private IEnumerable<IGraphicInstance> GetConnectedNodesWithPaths(IGraphicInstance currentNode, out Dictionary<IGraphicInstance, IEnumerable<INodeLinkBase>> visitedNodesWithPathDict)
    {
        visitedNodesWithPathDict = new();

        // ATTENTION: Without the following line the algorithm resets the nodes name, except for the currentNode, I still don't understand why.
        //            ( And also the program takes into account the starting node )
        visitedNodesWithPathDict.Add(currentNode, new List<INodeLinkBase>());

        return GetConnectedNodesRecursive(currentNode, ref visitedNodesWithPathDict);
    }

    private IEnumerable<IGraphicInstance> GetConnectedNodesRecursive(IGraphicInstance currentNode, ref Dictionary<IGraphicInstance, IEnumerable<INodeLinkBase>> visitedNodes)
    {
        List<INodeLinkBase> previousEdges = new();
        return GetConnectedNodesRecursive(currentNode, ref visitedNodes, previousEdges.AsEnumerable());
    }

    private IEnumerable<IGraphicInstance> GetConnectedNodesRecursive(IGraphicInstance currentNode, ref Dictionary<IGraphicInstance, IEnumerable<INodeLinkBase>> visitedNodesWithPathDict, IEnumerable<INodeLinkBase> previousEdges)
    {
        var edges = ActiveConnections.ToList();
        var incidentEdges = edges
            .Where(e => e.FromNode.Id == currentNode.Id);//.ToList();

        var reversedIncidentEdges = edges
                .Where(e => e.ToNode.Id == currentNode.Id);//.ToList();

        reversedIncidentEdges = reversedIncidentEdges.Select(e => e.SwappedEdges);//.ToList();

        incidentEdges = incidentEdges.Union(reversedIncidentEdges);//.ToList();

        var result = new List<IGraphicInstance>();

        // Happens only if the user clicks on a non connected node
        if (!incidentEdges.Any()) return result;

        foreach (var edge in incidentEdges)
        {
            IGraphicInstance toNode = edge.ToNode;

            // Node Already visited
            if (visitedNodesWithPathDict.ContainsKey(toNode))
                continue;

            // Node not visited, add it
            result.Add(toNode);
            var edgesUpToHere = previousEdges.Append(edge);
            visitedNodesWithPathDict[toNode] = edgesUpToHere;

            result.AddRange(GetConnectedNodesRecursive(toNode, ref visitedNodesWithPathDict, edgesUpToHere));
        }
        //if (result.Any())
        //    Debug.Log(
        //        $"connectedNodes @{(currentNode.BaseWrapped is INode inode ? inode.Name : "Node")} :  {result.Select(n => (n.BaseWrapped is INode inode ? inode.Name : "Name")).Aggregate("", (curr, newNode) => curr + "[" + newNode + "]")}");

        return result;
    }

    #endregion GRAPH_ALGORITHMS
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

            bool validConnection = ValidateConnection(from, to);

            if (validConnection)
                CreateConnection(from, to);

            if (!continousConnection || !validConnection)
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
            BaseWrapped = new NodeLinkBase(HarnessSettings.DefaultLinkPrefab)
            {
                Length = 0,
                FromNode = from.GraphicInstance,
                ToNode = to.GraphicInstance,
                BaseSOAddressableTyped = HarnessSettingsAddressable.DefaultLinkPrefab
            },
            Position = Vector3.forward * HarnessSettings.ConnectionsPlaceHeight
        };

        HarnessPlacer.CreateGraphicWrapper(wrapper, connectionsParent);

    }

    private bool ValidateConnection(ConnectibleManager from, ConnectibleManager to)
    {
        bool valid = true;

        // Check for preventing connections to same node
        valid &= IsNotFromSameNode(from, to);
        valid &= IsNotFromSinkToSink(from, to);
        valid &= IsNotAlreadyConnected(from, to);

        return valid;
    }

    private bool IsNotAlreadyConnected(ConnectibleManager from, ConnectibleManager to)
    {
        return !GetNodesConnectedToNode(from.GraphicInstance).Contains(to.GraphicInstance);
    }

    private bool IsNotFromSinkToSink(ConnectibleManager from, ConnectibleManager to)
    {
        return
            from.GraphicInstance.BaseWrapped is not ISink
            ||
            to.GraphicInstance.BaseWrapped is not ISink;

    }

    private bool IsNotFromSameNode(ConnectibleManager from, ConnectibleManager to)
    {
        return from.GraphicInstance != to.GraphicInstance;
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
