using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [Serializable]
    public class SourceBase : BaseNode, ISource
    {
        [SerializeField] private string _name;
        [SerializeField] private double maxAvailability;
        [SerializeField] private ConnectibleRelativeBase positiveConnectible;
        [SerializeField] private ConnectibleRelativeBase negativeConnectible;
        [SerializeField]
        [SerializeReference] private SourceBaseSO baseSO;

        public SourceBase(SourceBaseSO baseSO) : base(baseSO)
        {
            maxAvailability = UnityEngine.Random.Range(0, 31);
        }

        public override string Name { get => _name; set => _name = value; }
        public double MaxAvailability { get => maxAvailability; set => maxAvailability = value; }

        public IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        public IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
        public override INodeSO BaseSO { get => baseSO; set => baseSO = value as SourceBaseSO; }
    }
}