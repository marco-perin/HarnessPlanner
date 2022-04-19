using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ConnectorSO", menuName = "CoreDataSO/ConnectorSO")]
    public class ConnectorSO : LinkBaseSO, ILink<IConnectibleRelative>, INamed
    {
        [SerializeField] private string _name;

        public string Name { get => _name; set => _name = value; }

    }
}