using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "Sink", menuName = "GraphicDataSO/Sink")]
    [Serializable]
    public class SinkGraphic : IGraphicSink, ICanGenerateConnectibles
    {
        [SerializeField] private string id;
        [SerializeField] protected SinkBase baseWrapped;
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector2 size;

        public string Id { get => id; set => id = value; }
        public INode BaseWrapped { get => baseWrapped; set => baseWrapped = value as SinkBase; }
        public Vector3 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        IBaseType IGraphicInstance.BaseWrapped { get => BaseWrapped; set => BaseWrapped = value as INode; }


        public void GenerateConnectibles(Transform parent)
        {
            throw new NotImplementedException();
            //if (BaseWrapped is IBipole<IConnectible>)
            //{
            //    var bipole = (BaseWrapped as IBipole<IConnectible>);
            //    Instantiate(bipole.NegativeConnectible.Prefab, parent);
            //}
        }

        public Vector3 GetPositionForConnectorIndex(int i)
        {
            var v = i switch
            {
                0 => new Vector3(position.x - size.x / 2, position.y, 0),
                1 => new Vector3(position.x + size.x / 2, position.y, 0),
                _ => Vector3.zero,
            };
            return v;
        }

    }

}