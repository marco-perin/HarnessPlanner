namespace Assets.CoreData.Interfaces
{
    public interface ISource : INode, IBipole<IConnectibleRelative>
    {
        double MaxAvailability { get; set; }
    }

    public interface ISourceWithSignal : ISource, ITerminalWithSignal { };
}