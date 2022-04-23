// TODO: Remove this and use only core data
using Assets.GraphicData.Interfaces;

namespace Assets.CoreData.Interfaces
{
    public interface INodeLinkBase : INode, IWithLenghtFloat
    {
        public IGraphicInstance ToNode { get; set; }
        public IGraphicInstance FromNode { get; set; }
    }
}