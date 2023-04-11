using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class BaseNode<TSO> : BaseNode where TSO : BaseObjectSO
    {
        [SerializeField]
        //[SerializeReference]
        protected TSO baseSO;

        [SerializeField]
        private AssetReferenceT<TSO> baseSOAddressable;

        public BaseNode(TSO baseSO) : base(baseSO) { }

        public BaseNode(BaseNode<TSO> node) : this(node.baseSO)
        {
            Id = node.Id;
            Name = node.Name;
            // TODO: Understand if it is right to assign this here
            BaseSOAddressable = node.BaseSOAddressable;
        }

        public override INodeSO BaseSO { get => baseSO; set => baseSO = value as TSO; }
        public AssetReferenceT<TSO> BaseSOAddressableTyped { get => baseSOAddressable; set => baseSOAddressable = value; }
        public override AssetReference BaseSOAddressable { get => baseSOAddressable; set => baseSOAddressable = value as AssetReferenceT<TSO>; }

        public override TNode Clone<TNode>()
        {
            var newObj = new BaseNode<TSO>(this);

            return newObj as TNode;
        }
    }

    [Serializable]
    public abstract class BaseNode : INode, IEquatable<BaseNode>
    {
        [SerializeField] protected string _id;
        [SerializeField] protected string _name;

        public BaseNode(INodeSO baseSO)
        {
            BaseSO = baseSO;
            if (BaseSO != null)
            {
                Name = baseSO.Name;
            }

            Id = Guid.NewGuid().ToString();
        }

        public abstract INodeSO BaseSO { get; set; }
        public abstract AssetReference BaseSOAddressable { get; set; }
        public string Name { get => _name; set => _name = value; }
        public string Id { get => _id; set => _id = value; }
        public abstract TNode Clone<TNode>() where TNode : class, INode;

        public bool Equals(INode other)
        {
            if (other is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return this.Equals((BaseNode)obj);
            }
        }

        public bool Equals(BaseNode other)
        {
            return Equals(other as INode);
        }

        public override int GetHashCode()
        {
            var hash = HashCode.Combine(Id, BaseSOAddressable.AssetGUID);
            return hash;
        }

        public static bool operator ==(BaseNode lhs, BaseNode rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(BaseNode lhs, BaseNode rhs) => !(lhs == rhs);

    }
}