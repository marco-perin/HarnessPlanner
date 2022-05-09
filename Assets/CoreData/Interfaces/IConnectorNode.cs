namespace Assets.CoreData.Interfaces
{
    public interface IConnectorNode : IBaseNodeWithPinnedSO
    {
        string Variant { get; set; }
        ConnectorTypeEnum ConnectorType { get; set; }
    }
    public enum ConnectorTypeEnum
    {
        M,
        F
    }
}