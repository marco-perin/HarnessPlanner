using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

public class MainCalculatorSingleton : Singleton<MainCalculatorSingleton>
{
    public Transform nodesParent;
    public string BatteryNodeName;

    public ConductorData connectionConductorData;

    public List<FullPathData> fullPathsConnections;
    public Dictionary<IConnectionNode, double> cmaPerLink;

    private void Start()
    {
        connectionConductorData = MaterialDataManager.Instance.harnessDataSO.availableConductorsData.availableConductors.OrderByDescending(ac => float.Parse(ac.Awg)).First(ac => float.Parse(ac.Awg) <= float.Parse(connectionConductorData.Awg));
    }

    public void CalculateEverything()
    {
        ClearAllLinkData();
        Dictionary<IConnectionNode, double> cmaPerLink = CalculateCurrents();
        AssignCMAs(cmaPerLink);
        fullPathsConnections = CalculateConnections();

        //foreach (var c in fullConnections)
        //    Debug.Log($"Connection [{c.nodeA.Name}-{c.nodeB.Name}]: {c.Length}m @ {c.conductorData.Awg}awg");


        //Debug.Log($"Link datas: {string.Join("\n", MainConnectionsManagerSingleton.Instance.ActiveConnections.Select(c => (c.LinkInfo as LinkInfo)?.ToString() ?? ""))}");
    }

    private void AssignCMAs(Dictionary<IConnectionNode, double> cmaPerLink)
    {
        foreach ((var node, double cma) in cmaPerLink)
        {
            var n = CanvasUtilsManager.GetAllGraphicInstanceWithType<IGraphicConnectionNodeBase>().SingleOrDefault(gi => gi.Id == node.Id);
            Debug.Assert(n != null, "Cannot find graphic instance with the id of our connection node");

            var nodeBase = n.BaseWrapped as ConnectionNodeBase;
            Debug.Assert(nodeBase != null, "Our connection node is not a connection node");

            if (nodeBase.NodeInfo == null)
                nodeBase.NodeInfo = new ConnectionNodeInfo();

            nodeBase.NodeInfo.CMA = cma;
        }
    }

    public Dictionary<IConnectionNode, double> CalculateCurrents()
    {
        Debug.Assert(nodesParent != null);

        Dictionary<IConnectionNode, double> result = new();

        var graphicInstances = GetAllGraphicinstancesWithBasePinnedSO();

        var battGI = graphicInstances.SingleOrDefault(gi => gi.BaseWrapped is ISource isource && isource.Name == BatteryNodeName);
        var batt = battGI.BaseWrapped as ISource;

        Debug.Assert(batt != null);
        Debug.Log("Selected Source: " + batt.Name);

        var connections = GetNodesConnectedToGraphicInstance(battGI, out var visitedNodesWithPathDict, true);

        Dictionary<INodeLinkBase, double> currentPerLinkDict = new();

        foreach (var c in connections)
        {
            var path = visitedNodesWithPathDict[c];
            var baseWrapped = c.BaseWrapped as IBaseNodeWithPinnedSO;
            Debug.Assert(path != null);

            foreach (var link in path)
            {
                if (!currentPerLinkDict.ContainsKey(link))
                    currentPerLinkDict.Add(link, 0);

                currentPerLinkDict[link] += (baseWrapped is ISink sink) ? sink.Consumption : 0;
            }
        }

        Dictionary<INodeLinkBase, ConductorData> conductorDataPerLink = AssignPowerDataToLinks(currentPerLinkDict);

        return CalculateCMAS(conductorDataPerLink);
    }

    private Dictionary<IConnectionNode, double> CalculateCMAS(Dictionary<INodeLinkBase, ConductorData> conductorDataPerLink)
    {
        List<IConnectionNode> visitedNodes = new();
        Dictionary<IConnectionNode, double> cmaPerNode = new();

        foreach (var link in conductorDataPerLink.Keys)
        {

            if (link.FromNode.BaseWrapped is IConnectionNode nodeA)
                if (CalculateCMAPerConnectionNode(nodeA, conductorDataPerLink, visitedNodes, out double cma))
                    cmaPerNode[nodeA] = cma;

            if (link.ToNode.BaseWrapped is IConnectionNode nodeB)
                if (CalculateCMAPerConnectionNode(nodeB, conductorDataPerLink, visitedNodes, out double cma))
                    cmaPerNode[nodeB] = cma;

        }

        return cmaPerNode;
    }

    private static bool CalculateCMAPerConnectionNode(IConnectionNode node, Dictionary<INodeLinkBase, ConductorData> conductorDataPerLink, List<IConnectionNode> visitedNodes, out double cma)
    {
        cma = 0;

        if (visitedNodes.Contains(node)) return false;

        visitedNodes.Add(node);

        var incidentLinks = MainConnectionsManagerSingleton.GetWithIDIncidentEdges(node, conductorDataPerLink.Keys);

        cma = incidentLinks.Select(l =>
                                conductorDataPerLink.Single(ll => ll.Key.Id == l.Id))
                           .Sum(l => l.Value.CMA);

        return true;
    }

    public List<FullPathData> CalculateConnections()
    {
        var graphicInstances = GetAllGraphicinstancesWithBasePinnedSO();

        Dictionary<INode, List<IPinData>> countedConnections = new();

        var result = new List<FullPathData>();

        // Foreach node
        foreach (var graphicInstance in graphicInstances)
        {
            result.AddRange(ProcessGraphicInstanceForConnections(graphicInstance, countedConnections));

            //Debug.Log($"Link datas: {string.Join("\n", MainConnectionsManagerSingleton.Instance.ActiveConnections.Select(c => (c.LinkInfo as LinkInfo)?.ToString() ?? ""))}");
        }

        return result;

    }

    private List<FullPathData> ProcessGraphicInstanceForConnections(IGraphicInstance graphicInstance, Dictionary<INode, List<IPinData>> countedConnections)
    {
        List<FullPathData> result = new();

        var currentNodeAsBasePinnedSO = graphicInstance.BaseWrapped as IBaseNodeWithPinnedSO;

        // Filter out nodes that only have power pins
        if (currentNodeAsBasePinnedSO.Connections
            .Select(c => c.PinFromData.PinType)
            .Where(pt => pt != PinTypeEnum.Power && pt != PinTypeEnum.Ground)
            .Count() == 0
            )
            return result;

        var connectedNodes = GetNodesConnectedToGraphicInstance(graphicInstance, out var visitedNodesWithPathDict);

        //Debug.Log($" Node {currentNodeAsBasePinnedSO.Name} has {currentNodeAsBasePinnedSO.Connections.Count()} connections and {connectedNodes.Count()} connected nodes.");
        foreach (var connectedNode in connectedNodes
                .Where(connectedNode =>
                    currentNodeAsBasePinnedSO.Connections
                    .Any(nodd => nodd.ConnectedNode == (connectedNode.BaseWrapped as INode))
                )
                )
        {
            var connectedNodeAsBaseWithPinnedSO = connectedNode.BaseWrapped as IBaseNodeWithPinnedSO;

            var validConnectedNodePins = (connectedNodeAsBaseWithPinnedSO.BaseSO as IPinnedObjectSO)
                .PinConfiguration.PinDataArray
            .Where(pin =>
                    pin.PinType != PinTypeEnum.Power &&
                    pin.PinType != PinTypeEnum.Ground
                    )
            .Where(pin =>
                    currentNodeAsBasePinnedSO.Connections
                        .Any(c => c.PinToData.Equals(pin))
                    )
            ;

            //Debug.Log($" - Node {inode.Name}?");
            //Debug.Log($" - Node {connectedNodeAsBaseWithPinnedSO.Name} has {validConnectedNodePins.Count()} connections.");
            if (validConnectedNodePins.Count() == 0)
                continue;


            foreach (var connectedPin in validConnectedNodePins)
            {
                //Debug.Log($" -- Node {inode.Name} conn {conn.PinFromData.Name}");
                if (!countedConnections.ContainsKey(connectedNodeAsBaseWithPinnedSO))
                    countedConnections[connectedNodeAsBaseWithPinnedSO] = new() { connectedPin };
                else
                {
                    if (countedConnections[connectedNodeAsBaseWithPinnedSO].Contains(connectedPin))
                        continue;

                    countedConnections[connectedNodeAsBaseWithPinnedSO].Add(connectedPin);
                }

                var path = visitedNodesWithPathDict[connectedNode];

                //Debug.Log($"path lenght: {path.Count()}");

                AddConnectionsToPathLinks(path);

                //Debug.Log($"Link datas after add: {string.Join("\n", MainConnectionsManagerSingleton.Instance.ActiveConnections.Select(c => (c.LinkInfo as LinkInfo)?.ToString() ?? ""))}");
                result.Add(new()
                {
                    nodeA = currentNodeAsBasePinnedSO,
                    pinA = currentNodeAsBasePinnedSO.Connections.First(c => c.PinToData.Equals(connectedPin)).PinFromData,
                    nodeB = connectedNodeAsBaseWithPinnedSO,
                    pinB = connectedPin,
                    conductorData = connectionConductorData,
                    Length = path.Sum(x => x.Length)
                });

            }
        }

        return result;

    }

    private List<IGraphicInstance> GetAllGraphicinstancesWithBasePinnedSO()
    {
        return CanvasUtilsManager.GetAllGraphicInstanceWithBaseType<IBaseNodeWithPinnedSO>();
    }


    private void ClearAllLinkData()
    {
        var links = MainConnectionsManagerSingleton.
            Instance.ActiveConnections;
        //Debug.Log($"Clearing {links.Count()} links");

        foreach (var c in links)
        {
            //c.LinkInfo.PowerData = null;
            //if (c.LinkInfo != null)
            //    c.LinkInfo.ClearLineData();
            //else
            c.LinkInfo = null;
        }
    }

    private static Dictionary<INodeLinkBase, ConductorData> AssignPowerDataToLinks(Dictionary<INodeLinkBase, double> currentPerLink)
    {
        Dictionary<INodeLinkBase, ConductorData> conductorDataPerLink = new();

        foreach (var link_ref in currentPerLink.Keys)
        {
            var link = MainConnectionsManagerSingleton.Instance.ActiveConnections.FirstOrDefault(c => c.Id == link_ref.Id);
            Debug.Assert(link != null);

            var condData = MaterialDataManager.Instance.harnessDataSO.availableConductorsData.availableConductors
                .OrderBy(c => c.MaxCurrent)
                .FirstOrDefault(c => c.MaxCurrent >= currentPerLink[link_ref]);

            if (condData == null)
                condData = new()
                {
                    Awg = "0",
                    CMA = 100000,
                    MaxCurrent = 150
                };

            conductorDataPerLink[link_ref] = condData;

            //if (link.LinkInfo == null)
            link.LinkInfo = new LinkInfo
            {
                PowerData = new FullLineData()
                {
                    ConductorData = condData,
                    Current = currentPerLink[link_ref]
                }
            };
        }

        return conductorDataPerLink;
    }

    private void AddConnectionsToPathLinks(IEnumerable<INodeLinkBase> path)
    {
        //Debug.Log($"Iterating over {path.Count()} items");

        var newData = new FullLineData()
        {
            ConductorData = connectionConductorData,
            Current = 0
        };

        foreach (var link_ref in path)
        {
            // TODO: Check why this is necessary
            var link = MainConnectionsManagerSingleton.Instance.ActiveConnections.FirstOrDefault(c => c.Id == link_ref.Id);

            Debug.Assert(link != null);

            if (link.LinkInfo == null)
                link.LinkInfo = new LinkInfo();

            if (link.LinkInfo.LineData == null)
                link.LinkInfo.LineData = new List<FullLineData>()
                {
                    newData
                };
            else
                link.LinkInfo.AddLineData(newData);

            //Debug.Log("Adding LinkInfo LineData");
        }
    }


    public static IEnumerable<IGraphicInstance> GetNodesConnectedToGraphicInstance(IGraphicInstance graphicInstance, out Dictionary<IGraphicInstance, List<INodeLinkBase>> visitedNodesWithPathDict, bool filterNodesConnections = false)
    {
        var connectedNodes = MainConnectionsManagerSingleton.Instance.GetNodesConnectedToNodeWithPaths(graphicInstance, out visitedNodesWithPathDict);
        //Debug.Log($"{connections.Count()} connections before ibaseNodew/pinnedso filter");
        return connectedNodes.Where(c => c.BaseWrapped is IBaseNodeWithPinnedSO ipso && (!filterNodesConnections || ipso.Connections.Any(conn => conn.ConnectedNode == (graphicInstance.BaseWrapped as IBaseNodeWithPinnedSO))));
    }


}

public class FullPathData : ITableRowData
{
    public IBaseNodeWithPinnedSO nodeA;
    public IPinData pinA;
    public IBaseNodeWithPinnedSO nodeB;
    public IPinData pinB;
    public ConductorData conductorData;
    public double Length;

    public static string GetTableRowHeader(char separator)
    {
        return string.Join(separator, new string[] { "Node A", "Pin A Nr", "Pin A Name", "Node B", "Pin B Nr", "Pin B Name", "Lenght", IConductorData.GetTableRowHeader(separator) });
    }
    public string GetTableRowString(char separator)
    {
        return string.Join(separator, new string[] { nodeA.Name, pinA.PinNumber.ToString(), pinA.Name, nodeB.Name, pinB.PinNumber.ToString(), pinB.Name, Length.ToString(), conductorData.GetTableRowString(separator) });
    }

}
