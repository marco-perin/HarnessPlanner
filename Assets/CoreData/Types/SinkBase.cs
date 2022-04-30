using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.CoreData.Types
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
