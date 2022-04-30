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
        public override TNode Clone<TNode>()
        {
            var a = base.Clone<TNode>();

            a.BaseSO = (ConnectionNodeBaseSO)this.BaseSO;

            return a;
        }
    }
}