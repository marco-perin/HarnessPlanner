using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class ConnectorGraphic : BaseGraphicObject, IGraphicConnector
    {
        [SerializeField]
        [SerializeReference]
        protected ConnectorBase baseWrapped;

        public override IBaseType BaseWrapped { get => baseWrapped; set => baseWrapped = value as ConnectorBase; }
    }
}