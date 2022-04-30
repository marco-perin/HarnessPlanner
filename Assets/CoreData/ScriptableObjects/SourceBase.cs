using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class SourceBase : BaseNodeWithPinnedSO<SourceBaseSO>, ISource
    {
        [SerializeField] private double maxAvailability;

        public SourceBase(SourceBaseSO baseSO) : base(baseSO)
        {
            maxAvailability = UnityEngine.Random.Range(0, 31);
        }

        public double MaxAvailability { get => maxAvailability; set => maxAvailability = value; }

    }
}