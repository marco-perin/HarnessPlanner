namespace Assets.CoreData.Interfaces
{
    public interface ITerminalWithSignal
    {
        public ISignalConnectible[] SignalConnectibles { get; set; }
    }
}