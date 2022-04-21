using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    [Serializable]
    public class SinkGraphic : BaseGraphicObject, IGraphicSink//, ICanGenerateConnectibles
    {
        [SerializeField] protected SinkBase baseWrapped;

        public override IBaseType BaseWrapped { get => baseWrapped; set => baseWrapped = value as SinkBase; }



        //public void GenerateConnectibles(Transform parent)
        //{
        //    throw new NotImplementedException();
        //}

        //public Vector3 GetPositionForConnectorIndex(int i)
        //{
        //    var v = i switch
        //    {
        //        0 => new Vector3(position.x - size.x / 2, position.y, 0),
        //        1 => new Vector3(position.x + size.x / 2, position.y, 0),
        //        _ => Vector3.zero,
        //    };
        //    return v;
        //}

    }

}