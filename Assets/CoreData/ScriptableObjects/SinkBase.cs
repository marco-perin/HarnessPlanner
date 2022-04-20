using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.CoreData.ScriptableObjects
{
    [Serializable]
    public class SinkBase : BaseNode, ISink
    {
        [SerializeField] private string _name;
        [SerializeField] private double consumption;
        [SerializeField]
        [SerializeReference] private SinkBaseSO baseSO;

        public SinkBase() : this(null) { }

        public SinkBase(SinkBaseSO baseSO) : base(baseSO)
        {
            consumption = Random.Range(0, 31);
        }

        public override string Name { get => _name; set => _name = value; }
        public double Consumption { get => consumption; set => consumption = value; }

        public virtual IConnectibleRelative PositiveConnectible { get => baseSO.PositiveConnectible; set => baseSO.PositiveConnectible = value as ConnectibleRelativeBase; }
        public virtual IConnectibleRelative NegativeConnectible { get => baseSO.NegativeConnectible; set => baseSO.NegativeConnectible = value as ConnectibleRelativeBase; }

        public override INodeSO BaseSO { get => baseSO; set => baseSO = value as SinkBaseSO; }
    }
}