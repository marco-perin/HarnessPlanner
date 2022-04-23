using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.ScriptableObjects;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class SourceGraphicBaseWrapper : SourceGraphic, ISourceBaseWrapper
    {
        public new ISource BaseWrapped { get => baseWrapped; set => baseWrapped = value as SourceBase; }

    }
}
