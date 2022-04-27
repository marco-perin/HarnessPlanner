namespace Assets.CoreData.Interfaces
{
    public interface ISource : IBaseNodeWithPinnedSO//, IBipole<IConnectibleRelative>
    {
        double MaxAvailability { get; set; }
    }

    public interface ISourceWithSignal : ISource, ITerminalWithSignal { };
}