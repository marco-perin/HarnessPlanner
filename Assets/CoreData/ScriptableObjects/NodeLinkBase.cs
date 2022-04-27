using System;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class NodeLinkBase : BaseNode<NodeLinkBaseSO>, INodeLinkBase
    {
        [SerializeField] private double length;
        [SerializeField][SerializeReference] private BaseGraphicObject fromNode;
        [SerializeField][SerializeReference] private BaseGraphicObject toNode;

        public NodeLinkBase(NodeLinkBaseSO baseSO) : base(baseSO)
        {
        }

        public NodeLinkBase(NodeLinkBase nodeLinkBase) : base(nodeLinkBase)
        {
            Length = nodeLinkBase.Length;
            FromNode = nodeLinkBase.FromNode;
            ToNode = nodeLinkBase.ToNode;
        }

        public double Length { get => length; set => length = value; }
        public IGraphicInstance ToNode { get => toNode; set => toNode = value as BaseGraphicObject; }
        public IGraphicInstance FromNode { get => fromNode; set => fromNode = value as BaseGraphicObject; }

        public override TNode Clone<TNode>()
        {
            var newObj = new NodeLinkBase(this)
            {
                FromNode = FromNode,
                ToNode = ToNode
            };

            return newObj as TNode;
        }

        public INodeLinkBase SwappedEdges
        {
            get
            {
                var linkBase = Clone<NodeLinkBase>();
                linkBase.FromNode = ToNode;
                linkBase.ToNode = FromNode;
                return linkBase as INodeLinkBase;
            }
        }

    }
}