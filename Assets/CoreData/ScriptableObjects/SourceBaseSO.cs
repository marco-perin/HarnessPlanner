using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Source", menuName = "CoreDataSO/Source")]
    public class SourceBaseSO : BaseObjectSO, INamed, ISource, IWithPrefab
    {
        [SerializeField] private string _name;
        [SerializeField] private double maxAvailability;
        [SerializeField] private ConnectibleRelativeBase positiveConnectible;
        [SerializeField] private ConnectibleRelativeBase negativeConnectible;
        [SerializeField] private GameObject prefab;

        public string Name { get => _name; set => _name = value; }
        public double MaxAvailability { get => maxAvailability; set => maxAvailability = value; }

        public IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        public IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
        public GameObject Prefab { get => prefab; set => prefab = value; }
    }
}