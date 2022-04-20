using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SinkBaseSO", menuName = "CoreDataBaseSO/SinkBaseSO")]
    public class SinkBaseSO : BaseObjectSO, INodeSO, INamed
    {

        [SerializeField] private ConnectibleRelativeBase positiveConnectible;
        [SerializeField] private ConnectibleRelativeBase negativeConnectible;

        public virtual IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        public virtual IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
    }
}