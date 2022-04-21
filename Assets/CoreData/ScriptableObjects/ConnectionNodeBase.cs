using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class ConnectionNodeBase : BaseNode<ConnectionNodeBaseSO>, IConnectionNode
    {
        public ConnectionNodeBase(ConnectionNodeBaseSO baseSO) : base(baseSO)
        {
        }
    }
}