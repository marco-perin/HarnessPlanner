namespace Assets.CoreData.Interfaces
{
    public interface ISink : IBaseType, IBipole<IConnectibleRelative>
    {
        double Consumption { get; set; }
    }

    public interface ISinkWithSignal : ISink, ITerminalWithSignal { };
}