using System;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Interfaces
{
    public interface IBaseType { }
    public interface INode : IBaseType, INamed
    {
        INodeSO BaseSO { get; set; }
    }

    [Serializable]
    public class BaseNode<TSO> : BaseNode where TSO : BaseObjectSO
    {
        [SerializeField]
        [SerializeReference]
        protected TSO baseSO;

        public BaseNode(TSO baseSO) : base(baseSO) { }

        public override INodeSO BaseSO { get => baseSO; set => baseSO = value as TSO; }
    }

    [Serializable]
    public abstract class BaseNode : INode
    {
        [SerializeField] protected string _name;

        public BaseNode(INodeSO baseSO)
        {
            BaseSO = baseSO;
            if (BaseSO != null)
            {
                Name = baseSO.Name;
            }
        }

        public abstract INodeSO BaseSO { get; set; }
        public string Name { get => _name; set => _name = value; }
    }

    public interface INodeSO : IWithPrefab, INamed { }
    //public interface INodeSOWithPrefab : INodeSO, IWithPrefab { }
}