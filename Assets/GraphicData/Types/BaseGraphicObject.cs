using System;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public abstract class BaseGraphicObject : IGraphicInstance
    {

        [SerializeField] protected Vector3 position;
        [SerializeField] protected Vector2 size;

        public string Id
        {
            get => BaseWrapped.Id;
            set => BaseWrapped.Id = value;
        }
        public abstract IBaseType BaseWrapped { get; set; }
        public Vector3 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }

    }
}