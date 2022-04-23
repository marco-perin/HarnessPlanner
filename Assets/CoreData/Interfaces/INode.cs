using System;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Interfaces
{
    public interface IBaseType : IWithId { }
    public interface INode : IBaseType, INamed
    {
        INodeSO BaseSO { get; set; }
        TNode Clone<TNode>() where TNode : class, INode;
    }

    [Serializable]
    public class BaseNode<TSO> : BaseNode where TSO : BaseObjectSO
    {
        [SerializeField]
        [SerializeReference]
        protected TSO baseSO;

        public BaseNode(TSO baseSO) : base(baseSO) { }

        public BaseNode(BaseNode<TSO> node) : this(node.baseSO)
        {
            Id = node.Id;
            Name = node.Name;
        }

        public override INodeSO BaseSO { get => baseSO; set => baseSO = value as TSO; }

        public override TNode Clone<TNode>()
        {
            var newObj = new BaseNode<TSO>(this);

            return newObj as TNode;
        }

    }

    [Serializable]
    public abstract class BaseNode : INode
    {
        [SerializeField] protected string _id;
        [SerializeField] protected string _name;

        public BaseNode(INodeSO baseSO)
        {
            BaseSO = baseSO;
            if (BaseSO != null)
            {
                Name = baseSO.Name;
            }

            Id = Guid.NewGuid().ToString();
        }

        public abstract INodeSO BaseSO { get; set; }
        public string Name { get => _name; set => _name = value; }
        public string Id { get => _id; set => _id = value; }
        public abstract TNode Clone<TNode>() where TNode : class, INode;
    }

    public interface INodeSO : IWithPrefab, INamed { }
    //public interface INodeSOWithPrefab : INodeSO, IWithPrefab { }
}