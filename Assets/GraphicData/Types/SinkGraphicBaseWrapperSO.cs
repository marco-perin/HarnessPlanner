using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    public class SinkGraphicBaseWrapperSO : SinkGraphicSO, ISinkBaseWrapper
    {
        public new ISink BaseWrapped { get => baseWrapped; set => baseWrapped = value as SinkBaseSO; }
        public override IConnectibleRelative PositiveConnectible { get => BaseWrapped.PositiveConnectible; set => BaseWrapped.PositiveConnectible = value; }
        public override IConnectibleRelative NegativeConnectible { get => BaseWrapped.NegativeConnectible; set => BaseWrapped.NegativeConnectible = value; }
    
    }
}
