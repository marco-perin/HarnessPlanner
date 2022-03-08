using System;
using UnityEngine;

public class ConnectibleBase : IConnectible
{

    protected string _id;
    protected string _parentNodeId;
    public Vector3 position;

    public string Id
    {
        get => _id;
        set => _id = value;
    }
    //public HarnessNode<IConnectible> parentNode { get => parentNodeId; set => parent; }
}

public interface IConnectible
{
    string Id { get; set; }
    //HarnessNode<IConnectible> getParentNode(Dictionary<string, IConnectible>);
}