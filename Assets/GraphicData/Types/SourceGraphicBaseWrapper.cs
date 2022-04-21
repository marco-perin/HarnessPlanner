using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.ScriptableObjects;

namespace Assets.GraphicData.Types
{
    [Serializable]
    public class SourceGraphicBaseWrapper : SourceGraphic, ISourceBaseWrapper
    {
        public SourceGraphicBaseWrapper()
        {
            Id = Guid.NewGuid().ToString();
        }

        public new ISource BaseWrapped { get => baseWrapped; set => baseWrapped = value as SourceBase; }
        //public IConnectibleRelative PositiveConnectible { get => BaseWrapped.PositiveConnectible; set => BaseWrapped.PositiveConnectible = value; }
        //public IConnectibleRelative NegativeConnectible { get => BaseWrapped.NegativeConnectible; set => BaseWrapped.NegativeConnectible = value; }

    }
}
