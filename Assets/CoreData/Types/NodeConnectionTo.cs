using System;
using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class NodeConnectionTo : INodeConnectionTo
    {
        [SerializeReference]
        [SerializeField] private BaseNode connectedNode;

        //[SerializeReference]
        [SerializeField] private PinData pinData;

        //[SerializeReference]
        [SerializeField] private PinData pinFromData;

        public INode ConnectedNode { get => connectedNode; set => connectedNode = value as BaseNode; }
        public IPinData PinToData { get => pinData; set => pinData = value as PinData; }
        public IPinData PinFromData { get => pinFromData; set => pinFromData = value as PinData; }
    }
}
