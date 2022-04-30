using System;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class BaseNodeWithPinnedSO<TSO> : BaseNode<TSO>, IBaseNodeWithPinnedSO where TSO : BaseObjectSO
    {
        [SerializeField] private List<NodeConnectionTo> connections;

        public BaseNodeWithPinnedSO(TSO baseSO) : base(baseSO)
        {
            Connections = new List<NodeConnectionTo>();
        }

        public IEnumerable<INodeConnectionTo> Connections { get => connections; set => connections = value.Select(c => c as NodeConnectionTo).ToList(); }

    }
}