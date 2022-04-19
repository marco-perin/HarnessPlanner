using Assets.CoreData.Interfaces;
using System;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class LinkConnectionAbsoluteBase : ILinkConnection<IConnectibleAbsolute>
    {
        [SerializeField] private ConnectibleAbsoluteBase connectibleFrom;
        [SerializeField] private ConnectibleAbsoluteBase connectibleTo;

        public IConnectibleAbsolute ConnectibleFrom { get => connectibleFrom; set => connectibleFrom = value as ConnectibleAbsoluteBase; }
        public IConnectibleAbsolute ConnectibleTo { get => connectibleTo; set => connectibleTo = value as ConnectibleAbsoluteBase; }
    }
}
