namespace Assets.CoreData.Interfaces
{

    public interface IConnectionNode : INode
    {
        IConnectionNodeInfo NodeInfo { get; set; }
    }

    public interface IConnectionNodeInfo
    {
        double CMA { get; set; }
    }
}