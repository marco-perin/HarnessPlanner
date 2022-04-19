namespace Assets.CoreData.Interfaces
{
    public interface IBipole<T> where T : IConnectible
    {
        T PositiveConnectible { get; set; }
        T NegativeConnectible { get; set; }
    }
}