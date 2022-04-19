namespace Assets.CoreData.Interfaces
{
    public interface IHarnessData : INamed, IDescribed, IVersioned, IDateTimed
    {
        IHarnessTopology Topology { get; set; }
    }

    public interface IHarnessTopology
    {
        ISource[] Sources { get; set; }
        ISink[] Sinks { get; set; }
        ILink<IConnectibleRelative>[] LinksRelative { get; set; }
        ILinkConnection<IConnectibleAbsolute>[] LinksRaw { get; set; }
    }

}