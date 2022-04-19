using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    internal class HarnessTopologyBase : IHarnessTopology
    {
        [SerializeField] private SourceBaseSO[] sources;
        [SerializeField] private ISink[] sinks;
        [SerializeField] private LinkBaseSO[] links;
        [SerializeField] private LinkConnectionAbsoluteBase[] linksRaw;

        public virtual ISource[] Sources { get => sources; set => sources = value as SourceBaseSO[]; }
        public virtual ISink[] Sinks { get => sinks; set => sinks = value; }
        public virtual ILink<IConnectibleRelative>[] LinksRelative { get => links; set => links = value as LinkBaseSO[]; }
        public virtual ILinkConnection<IConnectibleAbsolute>[] LinksRaw { get => linksRaw; set => linksRaw = value as LinkConnectionAbsoluteBase[]; }
    }
}
