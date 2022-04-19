namespace Assets.CoreData.Interfaces
{
    public interface ISource : IBaseType, IBipole<IConnectibleRelative>
    {
        double MaxAvailability { get; set; }
    }

    public interface ISourceWithSignal : ISource, ITerminalWithSignal { };
}