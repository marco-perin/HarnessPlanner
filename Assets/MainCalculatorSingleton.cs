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
        CalculateCurrents();
        var fullConnections = CalculateConnections();

        foreach (var c in fullConnections)
            Debug.Log($"Connection [{c.nodeA.Name}-{c.nodeB.Name}]: {c.Length}m @ {c.conductorData.Awg}awg");
    }

    public void CalculateCurrents()
    {
        Debug.Assert(nodesParent != null);

        var graphicInstances = GetAllGraphicInstances();

        var battGI = graphicInstances.SingleOrDefault(gi => gi.BaseWrapped is ISource isource && isource.Name == BatteryNodeName);
        var batt = battGI.BaseWrapped as ISource;

        Debug.Assert(batt != null);
        Debug.Log("Selected Source: " + batt.Name);

        var connections = GetNodesConnectedToGraphicInstance(battGI, out var visitedNodesWithPathDict);

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

    private static void AssignPowerDataToLinks(Dictionary<INodeLinkBase, double> currentPerLink)
    {
        foreach (var link in currentPerLink.Keys)
        {
            var condData = MaterialDataManager.Instance.harnessDataSO.availableConductorsData.availableConductors.OrderBy(c => c.MaxCurrent).First(c => c.MaxCurrent >= currentPerLink[link]);

            link.LinkInfo = new LinkInfo()
            {
                PowerData = new FullLineData()
                {
                    ConductorData = condData,
                    Current = currentPerLink[link]
                }
            };
        }
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
        }

        return result;

    }

    private List<FullPathData> ProcessGraphicInstanceForConnections(IGraphicInstance graphicInstance, Dictionary<INode, List<IPinData>> countedConnections)
    {
        List<FullPathData> result = new();

        var pinnSO = graphicInstance.BaseWrapped as IBaseNodeWithPinnedSO;

        if (pinnSO.Connections.Select(c => c.PinFromData.PinType).All(pt => pt == PinTypeEnum.Power || pt == PinTypeEnum.Ground))
            return result;

        var connectedNodes = GetNodesConnectedToGraphicInstance(graphicInstance, out var visitedNodesWithPathDict);

        foreach (var connectedNode in connectedNodes)
        {
            var inode = connectedNode.BaseWrapped as IBaseNodeWithPinnedSO;

            if (inode.Connections.Count() <= 0) continue;


            //Debug.Log($" - Node {inode.Name} has {inode.Connections.Count()} connections.");
            foreach (var conn in inode.Connections)
            {
                if (!countedConnections.ContainsKey(inode))
                    countedConnections[inode] = new() { conn.PinToData };
                else
                {
                    if (countedConnections[inode].Contains(conn.PinToData))
                        continue;

                    countedConnections[inode].Add(conn.PinToData);
                }

                var path = visitedNodesWithPathDict[connectedNode];

                AddConnectionsToPathLinks(path);

                result.Add(new()
                {
                    nodeA = pinnSO,
                    nodeB = inode,
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

    private void AddConnectionsToPathLinks(IEnumerable<INodeLinkBase> path)
    {
        foreach (var link in path)
        {
            if (link.LinkInfo == null)
                link.LinkInfo = new LinkInfo();

            if (link.LinkInfo.LineData == null)
                link.LinkInfo.LineData = new List<FullLineData>();

            link.LinkInfo.LineData = link.LinkInfo.LineData
                .Append(new FullLineData()
                {
                    ConductorData = connectionConductorData,
                    Current = 0
                });
        }
    }

    private static IEnumerable<IGraphicInstance> GetNodesConnectedToGraphicInstance(IGraphicInstance graphicInstance, out Dictionary<IGraphicInstance, IEnumerable<INodeLinkBase>> visitedNodesWithPathDict)
    {
        var connections = MainConnectionsManagerSingleton.Instance.GetNodesConnectedToNodeWithPaths(graphicInstance, out visitedNodesWithPathDict);
        return connections.Where(c => c.BaseWrapped is IBaseNodeWithPinnedSO ipso && ipso.Connections.Any(conn => conn.ConnectedNode == (graphicInstance.BaseWrapped as IBaseNodeWithPinnedSO))).ToList();
    }

}

public class FullPathData
{
    public IBaseNodeWithPinnedSO nodeA;
    public IBaseNodeWithPinnedSO nodeB;
    public ConductorData conductorData;
    public double Length;

}
