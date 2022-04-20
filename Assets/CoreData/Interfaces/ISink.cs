namespace Assets.CoreData.Interfaces
{
    public interface ISink : INode, IBipole<IConnectibleRelative>
    {
        double Consumption { get; set; }
    }

    public interface ISinkWithSignal : ISink, ITerminalWithSignal { };
}