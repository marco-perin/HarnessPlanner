using System;
using Assets.CoreData.Interfaces;
// TODO: Remove this and use only core data
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class FullLineData : IFullLineData
    {
        [SerializeField] private ConductorData conductorData;
        [SerializeField] private double current;

        public IConductorData ConductorData { get => conductorData; set => conductorData = value as ConductorData; }
        public double Current { get => current; set => current = value; }
    }
}
