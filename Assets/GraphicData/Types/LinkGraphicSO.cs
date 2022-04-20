using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using System;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    public class LinkGraphicSO : LinkBaseSO, IGraphicLink<IConnectibleRelative>
    {
        [SerializeField] private string id;
        [SerializeField] private Vector3 position;
        [SerializeField] protected LinkBaseSO baseWrapped;
        [SerializeField] private Vector2 size;

        public virtual string Id { get => id; set => id = value; }
        public virtual Vector3 Position { get => position; set => position = value; }
        public virtual Vector2 Size { get => size; set => size = value; }
        public IBaseTypeSO BaseWrapped { get => baseWrapped; set => baseWrapped = value as LinkBaseSO; }
    }


}
