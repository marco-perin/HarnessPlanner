using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.ScriptableObjects;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class ConnectionNodeBaseGraphicBaseWrapper : ConnectionNodeBaseGraphic, IConnectionNodeBaseWrapper
    {
        //public ConnectionNodeBaseGraphicBaseWrapper()
        //{
        //    Id = Guid.NewGuid().ToString();
        //}

        public new IConnectionNode BaseWrapped { get => baseWrapped; set => baseWrapped = value as ConnectionNodeBase; }
    }
}
