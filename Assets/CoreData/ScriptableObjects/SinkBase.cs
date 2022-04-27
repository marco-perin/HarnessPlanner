using System;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.CoreData.ScriptableObjects
{

    [Serializable]
    public class SinkBase : BaseNodeWithPinnedSO<SinkBaseSO>, ISink
    {
        [SerializeField] private double consumption;

        public SinkBase(SinkBaseSO baseSO) : base(baseSO)
        {
            consumption = Random.Range(0, 31);
        }

        public double Consumption { get => consumption; set => consumption = value; }

        public override TNode Clone<TNode>()
        {
            var a = base.Clone<SinkBase>();

            return a as TNode;
        }
    }
}

namespace Assets.CoreData.Types
{
    [Serializable]
    public class NodeConnectionTo : INodeConnectionTo
    {
        [SerializeReference]
        [SerializeField] private BaseNode connectedNode;

        //[SerializeReference]
        [SerializeField] private PinData pinData;

        //[SerializeReference]
        [SerializeField] private PinData pinFromData;

        public INode ConnectedNode { get => connectedNode; set => connectedNode = value as BaseNode; }
        public IPinData PinToData { get => pinData; set => pinData = value as PinData; }
        public IPinData PinFromData { get => pinFromData; set => pinFromData = value as PinData; }
    }
}
