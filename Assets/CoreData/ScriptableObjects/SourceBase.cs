using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    public class SourceBase : INode, INamed, ISource
    {
        [SerializeField] private string _name;
        [SerializeField] private double maxAvailability;
        [SerializeField] private ConnectibleRelativeBase positiveConnectible;
        [SerializeField] private ConnectibleRelativeBase negativeConnectible;
        [SerializeField] private SourceBaseSO baseSO;

        public string Name { get => _name; set => _name = value; }
        public double MaxAvailability { get => maxAvailability; set => maxAvailability = value; }

        public IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        public IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
        public INodeSO BaseSO { get => baseSO; set => baseSO = value as SourceBaseSO; }
    }
}