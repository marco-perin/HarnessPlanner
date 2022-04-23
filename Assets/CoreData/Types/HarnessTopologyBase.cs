//using Assets.CoreData.Interfaces;
//using Assets.CoreData.ScriptableObjects;
//using System;
//using UnityEngine;

//namespace Assets.CoreData.Types
//{
//    [Serializable]
//    public class HarnessTopologyBase : IHarnessTopology
//    {
//        [SerializeField] private SourceBase[] sources;
//        [SerializeField] private SinkBase[] sinks;
//        [SerializeField] private LinkBase[] links;
//        [SerializeField] private LinkConnectionAbsoluteBase[] linksRaw;

//        public virtual ISource[] Sources { get => sources; set => sources = value as SourceBase[]; }
//        public virtual ISink[] Sinks { get => sinks; set => sinks = value as SinkBase[]; }
//        public virtual ILink<IConnectibleRelative>[] LinksRelative { get => links; set => links = value as LinkBase[]; }
//        public virtual ILinkConnection<IConnectibleAbsolute>[] LinksRaw { get => linksRaw; set => linksRaw = value as LinkConnectionAbsoluteBase[]; }
//    }
//}
