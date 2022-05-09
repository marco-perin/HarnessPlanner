using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class ConnectorBase : BaseNodeWithPinnedSO<ConnectorNodeBaseSO>, IConnectorNode
    {
        [SerializeField] private string variant;
        [SerializeField] private ConnectorTypeEnum connectorType;

        public ConnectorBase(ConnectorNodeBaseSO baseSO) : base(baseSO)
        {
            if (baseSO.Variants.Length > 0)
                variant = baseSO.Variants[0];
        }

        public string Variant { get => variant; set => variant = value; }
        public ConnectorTypeEnum ConnectorType { get => connectorType; set => connectorType = value; }
    }
}
