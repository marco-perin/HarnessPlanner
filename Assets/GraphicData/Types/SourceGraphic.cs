using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class SourceGraphic : BaseGraphicObject, IGraphicSource
    {
        [SerializeField]
        [SerializeReference]
        protected SourceBase baseWrapped;

        public override IBaseType BaseWrapped { get => baseWrapped; set => baseWrapped = value as SourceBase; }

    }
}