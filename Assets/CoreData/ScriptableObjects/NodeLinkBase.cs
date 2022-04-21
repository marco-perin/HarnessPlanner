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
        [SerializeField] private float length;
        [SerializeField][SerializeReference] private BaseGraphicObject fromNode;
        [SerializeField][SerializeReference] private BaseGraphicObject toNode;

        public NodeLinkBase(NodeLinkBaseSO baseSO) : base(baseSO)
        {
        }

        public float Length { get => length; set => length = value; }
        public IGraphicInstance ToNode { get => toNode; set => toNode = value as BaseGraphicObject; }
        public IGraphicInstance FromNode { get => fromNode; set => fromNode = value as BaseGraphicObject; }
    }
}