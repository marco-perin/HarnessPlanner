using System;
using Assets.CoreData.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.CoreData.ScriptableObjects
{

    [Serializable]
    public class SinkBase : BaseNode<SinkBaseSO>, ISink
    {
        [SerializeField] private double consumption;

        public SinkBase(SinkBaseSO baseSO) : base(baseSO)
        {
            consumption = Random.Range(0, 31);
        }

        public double Consumption { get => consumption; set => consumption = value; }

        //public virtual IConnectibleRelative PositiveConnectible { get => baseSO.PositiveConnectible; set => baseSO.PositiveConnectible = value as ConnectibleRelativeBase; }
        //public virtual IConnectibleRelative NegativeConnectible { get => baseSO.NegativeConnectible; set => baseSO.NegativeConnectible = value as ConnectibleRelativeBase; }

    }
}