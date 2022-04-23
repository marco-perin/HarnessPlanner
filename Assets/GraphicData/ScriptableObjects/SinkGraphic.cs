using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    [Serializable]
    public class SinkGraphic : BaseGraphicObject, IGraphicSink
    {
        [SerializeField] protected SinkBase baseWrapped;

        public override IBaseType BaseWrapped { get => baseWrapped; set => baseWrapped = value as SinkBase; }
    }

}