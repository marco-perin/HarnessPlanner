using Assets.CoreData.Interfaces;

namespace Assets.GraphicData.Interfaces
{
    public interface IGraphicLink<T> : ILink<T>, IGraphicInstance where T : IConnectible { }
}