﻿using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SinkBaseSO", menuName = "CoreDataBaseSO/SinkBaseSO")]
    public class SinkBaseSO : ScriptableObject, INodeSO, INamed
    {
        [SerializeField] private GameObject prefab;

        [SerializeField] private ConnectibleRelativeBase positiveConnectible;
        [SerializeField] private ConnectibleRelativeBase negativeConnectible;
        [SerializeField] private string _name;

        public GameObject Prefab { get => prefab; set => prefab = value; }
        public virtual IConnectibleRelative PositiveConnectible { get => positiveConnectible; set => positiveConnectible = value as ConnectibleRelativeBase; }
        public virtual IConnectibleRelative NegativeConnectible { get => negativeConnectible; set => negativeConnectible = value as ConnectibleRelativeBase; }
        public string Name { get => _name; set => _name = value; }
    }
}