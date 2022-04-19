using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    internal class HarnessTopologyGraphic : HarnessTopologyBase
    {
        [SerializeField] private SourceGraphicSO[] sources;
        [SerializeField] private IGraphicSink[] sinks;
        [SerializeField] private LinkGraphicSO[] links;
        [SerializeField] private LinkConnectionAbsoluteBase[] linksRaw;

        public new IGraphicSource[] Sources { get => sources; set => sources = value as SourceGraphicSO[]; }
        public new IGraphicSink[] Sinks { get => sinks; set => sinks = value; }
        public new IGraphicLink<IConnectibleRelative>[] LinksRelative { get => links; set => links = value as LinkGraphicSO[]; }
        public override ILinkConnection<IConnectibleAbsolute>[] LinksRaw { get => linksRaw; set => linksRaw = value as LinkConnectionAbsoluteBase[]; }
    }

}
