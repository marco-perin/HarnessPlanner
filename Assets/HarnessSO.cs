using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Harness", menuName = "SO/Harness")]
public class HarnessSO : ScriptableObject
{
    public new string name;
    public string id;
    public string description;
    public List<ConnectorData> connectors;
    public List<HarnessNode<ConnectibleBase>> nodes;
    public List<Connection> connections;
}

[Serializable]
public class ConnectorData : HarnessNode<Pin>
{
    public string name;
    public ConnectorSO connector;

    public List<Pin> Pins
    {
        get => connectibles;
        set => connectibles = value;
    }
}

[Serializable]
public class Connection
{
    public string connectionId;
    public string ConnectionFromId;
    public List<string> ConnectionToId;
}
