// TODO: Remove this and use only core data
using Assets.GraphicData.Interfaces;

namespace Assets.CoreData.Interfaces
{
    public interface INodeLinkBase : INode, IWithLenghtDouble
    {
        public ILinkInfo LinkInfo { get; set; }
        public IGraphicInstance ToNode { get; set; }
        public IGraphicInstance FromNode { get; set; }
        public INodeLinkBase SwappedEdges { get; }

    }
}