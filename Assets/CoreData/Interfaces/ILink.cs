namespace Assets.CoreData.Interfaces
{
    public interface ILink<Tconnectible> : IBaseType where Tconnectible : IConnectible
    {
        ILinkConnection<Tconnectible>[] LinkConnections { get; set; }
    }
}