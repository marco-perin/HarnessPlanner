namespace Assets.CoreData.Interfaces
{
    public interface IBaseType { }
    public interface INode : IBaseType
    {
        INodeSO BaseSO { get; set; }
    }

    public interface INodeSO : IWithPrefab,INamed { }
    //public interface INodeSOWithPrefab : INodeSO, IWithPrefab { }
}