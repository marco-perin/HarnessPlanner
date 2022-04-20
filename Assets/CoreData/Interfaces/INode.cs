namespace Assets.CoreData.Interfaces
{
    public interface IBaseType { }
    public interface INode : IBaseType, INamed
    {
        INodeSO BaseSO { get; set; }
    }

    public abstract class BaseNode : INode
    {
        //private BaseTypeSO baseSO;
        //private string _name;

        public BaseNode(INodeSO baseSO)
        {
            BaseSO = baseSO;
            if (BaseSO != null)
            {
                Name = baseSO.Name;
            }
        }

        public abstract INodeSO BaseSO { get; set; }
        public abstract string Name { get; set; }
    }

    public interface INodeSO : IWithPrefab, INamed { }
    //public interface INodeSOWithPrefab : INodeSO, IWithPrefab { }
}