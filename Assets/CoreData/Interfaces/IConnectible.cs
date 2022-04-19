using UnityEngine;

namespace Assets.CoreData.Interfaces
{

    public interface IConnectible : IWithGettableId { GameObject Prefab { get; set; } }

    public interface IConnectibleRelative : IWithCalculatedId, IConnectible
    {
        string RelativeId { get; set; }
    }

    public interface IConnectibleAbsolute : IConnectible, IWithId
    {
        string AbsoluteId { get; set; }
    }
}