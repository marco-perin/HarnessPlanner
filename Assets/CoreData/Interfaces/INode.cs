using System;

namespace Assets.CoreData.Interfaces
{
    public interface IBaseType : IWithId { }
    public interface INode : IBaseType, INamed, IEquatable<INode>
    {
        INodeSO BaseSO { get; set; }
        TNode Clone<TNode>() where TNode : class, INode;
    }
    public interface INodeSO : IWithPrefab, INamed { }

}