namespace Assets.CoreData.Interfaces
{
    public interface ILinkConnection<Tconnectible> where Tconnectible : IConnectible
    {
        Tconnectible ConnectibleFrom { get; set; }
        Tconnectible ConnectibleTo { get; set; }
    }

    public interface ISignalConnectible : IConnectible
    {
        ISignal Signal { get; set; }
    }

}
