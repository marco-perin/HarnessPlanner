using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.ScriptableObjects;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class NodeLinkBaseGraphicBaseWrapper : NodeLinkBaseGraphic, INodeLinkBaseBaseWrapper
    {
        public new INodeLinkBase BaseWrapped { get => baseWrapped; set => baseWrapped = value as NodeLinkBase; }
    }
}
