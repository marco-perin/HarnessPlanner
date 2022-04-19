using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;

namespace Assets.GraphicData.Types
{
    public class LinkGraphicBaseWrapperSO : LinkGraphicSO, ILinkBaseWrapper<IConnectibleRelative>
    {
        [SerializeField] private LinkBaseSO linkBase;

        public new ILinkConnection<IConnectibleRelative>[] LinkConnections { get => linkBase.LinkConnections; set => linkBase.LinkConnections = value as LinkConnectionRelativeBase[]; }

        public virtual new ILink<IConnectibleRelative> BaseWrapped { get => linkBase; set => linkBase = value as LinkBaseSO; }
    }
}
