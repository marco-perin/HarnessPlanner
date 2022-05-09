using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class SinkGraphicBaseWrapper : SinkGraphic, ISinkBaseWrapper
    {
        //public SinkGraphicBaseWrapper()
        //{
        //    //Id = Guid.NewGuid().ToString();
        //}

        public new ISink BaseWrapped { get => baseWrapped; set => baseWrapped = value as SinkBase; }
    }

    [Serializable]
    public class ConnectorGraphicBaseWrapper : ConnectorGraphic, IConnectorNodeBaseWrapper
    {
        public new IConnectorNode BaseWrapped { get => baseWrapped; set => baseWrapped = value as ConnectorBase; }
    }
}
