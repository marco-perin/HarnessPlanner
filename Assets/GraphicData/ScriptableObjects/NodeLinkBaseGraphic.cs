﻿using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    [Serializable]
    public class NodeLinkBaseGraphic : BaseGraphicObject, IGraphicNodeLinkBase
    {
        [SerializeField] protected NodeLinkBase baseWrapped;

        public override IBaseType BaseWrapped { get => baseWrapped; set => baseWrapped = value as NodeLinkBase; }

        //IBaseType IGraphicInstance.BaseWrapped { get => baseWrapped; set => BaseWrapped = value as INode; }
    }


}