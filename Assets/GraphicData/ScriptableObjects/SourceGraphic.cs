using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "Source", menuName = "GraphicDataSO/Source")]
    [Serializable]
    public class SourceGraphic : BaseGraphicObject, IGraphicSource
    {
        [SerializeField]
        [SerializeReference]
        protected SourceBase baseWrapped;

        public override IBaseType BaseWrapped { get => baseWrapped; set => baseWrapped = value as SourceBase; }

        //IBaseType IGraphicInstance.BaseWrapped { get => baseWrapped; set => BaseWrapped = value as INode; }
    }
}