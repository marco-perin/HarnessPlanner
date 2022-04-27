using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [Serializable]
    public class SourceBase : BaseNodeWithPinnedSO<SourceBaseSO>, ISource
    {
        [SerializeField] private double maxAvailability;
        //[SerializeField] private ConnectibleRelativeBase positiveConnectible;
        //[SerializeField] private ConnectibleRelativeBase negativeConnectible;


        public SourceBase(SourceBaseSO baseSO) : base(baseSO)
        {
            maxAvailability = UnityEngine.Random.Range(0, 31);
        }

        public double MaxAvailability { get => maxAvailability; set => maxAvailability = value; }

        //public IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        //public IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
    }
}