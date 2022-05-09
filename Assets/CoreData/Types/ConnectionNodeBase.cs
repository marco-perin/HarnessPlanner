using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class ConnectionNodeBase : BaseNode<ConnectionNodeBaseSO>, IConnectionNode
    {
#nullable enable
        [SerializeField]
        private ConnectionNodeInfo? nodeInfo;
#nullable disable

        public ConnectionNodeBase(ConnectionNodeBaseSO baseSO) : base(baseSO)
        {
        }

        public IConnectionNodeInfo NodeInfo { get => nodeInfo; set => nodeInfo = value as ConnectionNodeInfo; }

        public override TNode Clone<TNode>()
        {
            var a = base.Clone<TNode>();

            a.BaseSO = (ConnectionNodeBaseSO)this.BaseSO;

            return a;
        }
    }

    [Serializable]
    public class ConnectionNodeInfo : IConnectionNodeInfo
    {
        [SerializeField]
        private double cma;

        public double CMA { get => cma; set => cma = value; }
    }
}