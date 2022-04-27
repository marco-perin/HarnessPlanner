using System.Collections;
using System.Collections.Generic;

namespace Assets.CoreData.Interfaces
{
    public interface ISink : IBaseNodeWithPinnedSO //, IBipole<IConnectibleRelative>
    {
        double Consumption { get; set; }
    }

    public interface IBaseNodeWithPinnedSO : INode
    {
        IEnumerable<INodeConnectionTo> Connections { get; set; }
    }

    public interface INodeConnectionTo
    {
        IPinData PinFromData { get; set; }
        INode ConnectedNode { get; set; }
        IPinData PinToData { get; set; }
    }

    public interface ISinkWithSignal : ISink, ITerminalWithSignal { };
}