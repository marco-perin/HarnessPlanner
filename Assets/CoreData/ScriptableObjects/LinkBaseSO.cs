using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.CoreData.Types
{
    public class LinkBaseSO : BaseObjectSO, ILink<IConnectibleRelative>
    {
        [SerializeField] private protected LinkConnectionRelativeBase[] linkConnections;

        public ILinkConnection<IConnectibleRelative>[] LinkConnections { get => linkConnections; set => linkConnections = value as LinkConnectionRelativeBase[]; }

        [Serializable]
        public class LinkConnectionRelativeBase : ILinkConnection<IConnectibleRelative>
        {
            [SerializeField] private ConnectibleRelativeBase connectibleFrom;
            [SerializeField] private ConnectibleRelativeBase connectibleTo;

            public IConnectibleRelative ConnectibleFrom { get => connectibleFrom; set => connectibleFrom = value as ConnectibleRelativeBase; }
            public IConnectibleRelative ConnectibleTo { get => connectibleTo; set => connectibleTo = value as ConnectibleRelativeBase; }
        }
    }
}
