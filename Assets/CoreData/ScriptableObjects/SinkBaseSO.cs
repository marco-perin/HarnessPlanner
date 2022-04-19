using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Sink", menuName = "CoreDataSO/Sink")]
    public class SinkBaseSO : BaseObjectSO, INamed, ISink, IWithPrefab
    {
        [SerializeField] private string _name;
        [SerializeField] private double consumption;
        [SerializeField] private ConnectibleRelativeBase positiveConnectible;
        [SerializeField] private ConnectibleRelativeBase negativeConnectible;
        [SerializeField] private GameObject prefab;

        public string Name { get => _name; set => _name = value; }
        public double Consumption { get => consumption; set => consumption = value; }

        public virtual IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        public virtual IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
        public GameObject Prefab { get => prefab; set => prefab = value; }
    }
}