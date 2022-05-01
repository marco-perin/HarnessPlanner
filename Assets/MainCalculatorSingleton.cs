using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

public class MainCalculatorSingleton : Singleton<MainCalculatorSingleton>
{
    public Transform nodesParent;
    public string BatteryNodeName;


    public void CalculateCurrents()
    {
        Debug.Assert(nodesParent != null);

        var graphicInstances = nodesParent
            .GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>()
            .Where(mbContainer => mbContainer.GraphicInstance.BaseWrapped is IBaseNodeWithPinnedSO)
            .Select(mb => mb.GraphicInstance)
            .ToList();

        var battGI = graphicInstances.SingleOrDefault(gi => gi.BaseWrapped is ISource isource && isource.Name == BatteryNodeName);
        var batt = battGI.BaseWrapped as ISource;

        Debug.Assert(batt != null);
        Debug.Log("Battery: " + batt.Name);

        var connections = MainConnectionsManagerSingleton.Instance.GetNodesConnectedToNodeWithPaths(battGI, out var visitedNodes);

        connections = connections.Where(c => c.BaseWrapped is IBaseNodeWithPinnedSO ipso && ipso.Connections.Any(conn => conn.ConnectedNode == batt)).ToList();

        Dictionary<INodeLinkBase, double> currentPerLink = new();

        foreach (var c in connections)
        {
            var path = visitedNodes[c];
            var baseWrapped = c.BaseWrapped as IBaseNodeWithPinnedSO;
            Debug.Assert(path != null);

            foreach (var link in path)
            {
                if (!currentPerLink.ContainsKey(link))
                    currentPerLink.Add(link, 0);

                currentPerLink[link] += (baseWrapped is ISink sink) ? sink.Consumption : 0;
            }

            #region LENGHTS_CALCS

            var toPath = string.Join("-", path.Select(l => "" + l.Length + "m"));
            Debug.Log($"Path to node {baseWrapped.Name} ( {path.Sum(l => l.Length)}m ): {toPath}");

            #endregion LENGHTS_CALCS
        }

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

        //foreach (var l in currentPerLink.Keys)
        //{
        //    var minAWG = MaterialDataManager.Instance.harnessDataSO.availableConductorsData.availableConductors.OrderBy(c => c.MaxCurrent).First(c => c.MaxCurrent >= currentPerLink[l]).Awg;
        //    Debug.Log($"Current for Link [{(l.FromNode.BaseWrapped is INode fn ? fn.Name : "Node")}-{(l.ToNode.BaseWrapped is INode tn ? tn.Name : "Node")}]: {currentPerLink[l]}A -> {minAWG}awg");
        //}
    }
}
