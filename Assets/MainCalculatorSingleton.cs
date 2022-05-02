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

    private void Start()
    {
        connectionConductorData = MaterialDataManager.Instance.harnessDataSO.availableConductorsData.availableConductors.OrderByDescending(ac => float.Parse(ac.Awg)).First(ac => float.Parse(ac.Awg) <= float.Parse(connectionConductorData.Awg));
    }

    public void CalculateEverything()
    {
        ClearAllLinkData();
        CalculateCurrents();
        var fullConnections = CalculateConnections();

        foreach (var c in fullConnections)
            Debug.Log($"Connection [{c.nodeA.Name}-{c.nodeB.Name}]: {c.Length}m @ {c.conductorData.Awg}awg");


        Debug.Log($"Link datas: {string.Join("\n", MainConnectionsManagerSingleton.Instance.ActiveConnections.Select(c => (c.LinkInfo as LinkInfo)?.ToString() ?? ""))}");
    }

    public void CalculateCurrents()
    {
        Debug.Assert(nodesParent != null);

        var graphicInstances = GetAllGraphicInstances();

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

            //#region LENGHTS_CALCS

            //var toPath = string.Join("-", path.Select(l => "" + l.Length + "m"));
            //Debug.Log($"Path to node {baseWrapped.Name} ( {path.Sum(l => l.Length)}m ): {toPath}");

            //#endregion LENGHTS_CALCS
        }

        AssignPowerDataToLinks(currentPerLinkDict);

    }

    public List<FullPathData> CalculateConnections()
    {
        var graphicInstances = GetAllGraphicInstances();

        Dictionary<INode, List<IPinData>> countedConnections = new();

        var result = new List<FullPathData>();

        // Foreach node
        foreach (var graphicInstance in graphicInstances)
        {
            result.AddRange(ProcessGraphicInstanceForConnections(graphicInstance, countedConnections));

            Debug.Log($"Link datas: {string.Join("\n", MainConnectionsManagerSingleton.Instance.ActiveConnections.Select(c => (c.LinkInfo as LinkInfo)?.ToString() ?? ""))}");
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

        Debug.Log($" Node {currentNodeAsBasePinnedSO.Name} has {currentNodeAsBasePinnedSO.Connections.Count()} connections and {connectedNodes.Count()} connected nodes.");
        foreach (var connectedNode in connectedNodes
                .Where(connectedNode =>
                    currentNodeAsBasePinnedSO.Connections
                    .Any(nodd => nodd.ConnectedNode == (connectedNode.BaseWrapped as INode))
                )
                )
        {
            var connectedNodeAsBaseWithPinnedSO = connectedNode.BaseWrapped as IBaseNodeWithPinnedSO;

            var validConnectedNodePins = (connectedNodeAsBaseWithPinnedSO.BaseSO as IPinnedObjectSO).PinConfiguration.PinDataArray
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
            Debug.Log($" - Node {connectedNodeAsBaseWithPinnedSO.Name} has {validConnectedNodePins.Count()} connections.");
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

                Debug.Log($"Link datas after add: {string.Join("\n", MainConnectionsManagerSingleton.Instance.ActiveConnections.Select(c => (c.LinkInfo as LinkInfo)?.ToString() ?? ""))}");
                result.Add(new()
                {
                    nodeA = currentNodeAsBasePinnedSO,
                    nodeB = connectedNodeAsBaseWithPinnedSO,
                    conductorData = connectionConductorData,
                    Length = path.Sum(x => x.Length)
                });

            }
        }

        return result;

    }

    private List<IGraphicInstance> GetAllGraphicInstances()
    {
        return nodesParent
          .GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>()
          .Where(mbContainer => mbContainer.GraphicInstance.BaseWrapped is IBaseNodeWithPinnedSO)
          .Select(mb => mb.GraphicInstance)
          .ToList();
    }

    private void ClearAllLinkData()
    {
        var links = MainConnectionsManagerSingleton.
            Instance.ActiveConnections;
        Debug.Log($"Clearing {links.Count()} links");

        foreach (var c in links)
        {
            //c.LinkInfo.PowerData = null;
            //if (c.LinkInfo != null)
            //    c.LinkInfo.ClearLineData();
            //else
            c.LinkInfo = null;
        }
    }

    private static void AssignPowerDataToLinks(Dictionary<INodeLinkBase, double> currentPerLink)
    {
        foreach (var link in currentPerLink.Keys)
        {
            var condData = MaterialDataManager.Instance.harnessDataSO.availableConductorsData.availableConductors.OrderBy(c => c.MaxCurrent).First(c => c.MaxCurrent >= currentPerLink[link]);

            //if (link.LinkInfo == null)
            link.LinkInfo = new LinkInfo
            {
                PowerData = new FullLineData()
                {
                    ConductorData = condData,
                    Current = currentPerLink[link]
                }
            };
        }
    }
    private void AddConnectionsToPathLinks(IEnumerable<INodeLinkBase> path)
    {
        Debug.Log($"Iterating over {path.Count()} items");

        var newData = new FullLineData()
        {
            ConductorData = connectionConductorData,
            Current = 69
        };

        foreach (var link_ref in path)
        {
            var link = MainConnectionsManagerSingleton.Instance.ActiveConnections.FirstOrDefault(c => c.Id == link_ref.Id);

            Debug.Assert(link != null);

            if (link.LinkInfo == null)
                link.LinkInfo = new LinkInfo
                {

                    //if (link.LinkInfo.PowerData == null)
                    PowerData = new FullLineData()
                    {
                        Current = 69
                    }
                };


            if (link.LinkInfo.LineData == null)
                link.LinkInfo.LineData = new List<FullLineData>()
                {
                    newData
                };
            else
                link.LinkInfo.LineData = link.LinkInfo.LineData.Append(newData).ToList();

            //Debug.Log("Adding LinkInfo LineData");
            //link.LinkInfo.AddLineData(new FullLineData()
            //{
            //    ConductorData = connectionConductorData,
            //    Current = 69
            //});
        }
    }

    private static IEnumerable<IGraphicInstance> GetNodesConnectedToGraphicInstance(IGraphicInstance graphicInstance, out Dictionary<IGraphicInstance, List<INodeLinkBase>> visitedNodesWithPathDict, bool filterNodesConnections = false)
    {
        var connectedNodes = MainConnectionsManagerSingleton.Instance.GetNodesConnectedToNodeWithPaths(graphicInstance, out visitedNodesWithPathDict);
        //Debug.Log($"{connections.Count()} connections before ibaseNodew/pinnedso filter");
        return connectedNodes.Where(c => c.BaseWrapped is IBaseNodeWithPinnedSO ipso && (!filterNodesConnections || ipso.Connections.Any(conn => conn.ConnectedNode == (graphicInstance.BaseWrapped as IBaseNodeWithPinnedSO))));
    }

}

public class FullPathData
{
    public IBaseNodeWithPinnedSO nodeA;
    public IBaseNodeWithPinnedSO nodeB;
    public ConductorData conductorData;
    public double Length;

}
