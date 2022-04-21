using Assets.CoreData.Interfaces;

namespace Assets.GraphicData.ScriptableObjects
{
    public interface IBaseWrapper<TBase> where TBase : IBaseType
    {
        TBase BaseWrapped { get; set; }
    }

    public interface ISinkBaseWrapper : IBaseWrapper<ISink> { }
    public interface ISourceBaseWrapper : IBaseWrapper<ISource> { }
    public interface IConnectionNodeBaseWrapper : IBaseWrapper<IConnectionNode> { }
    public interface INodeLinkBaseBaseWrapper : IBaseWrapper<INodeLinkBase> { }
    public interface ILinkBaseWrapper<Tconnectible> : IBaseWrapper<ILink<Tconnectible>> where Tconnectible : IConnectible { }
}
