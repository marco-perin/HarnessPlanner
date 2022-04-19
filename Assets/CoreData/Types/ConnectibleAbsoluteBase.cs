using Assets.CoreData.Interfaces;
using System;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class ConnectibleAbsoluteBase : IConnectibleAbsolute
    {
        [SerializeField] private string _absoluteId;
        [SerializeField] private GameObject prefab;

        public string Id { get => AbsoluteId; set => AbsoluteId = value; }
        public string AbsoluteId { get => _absoluteId; set => _absoluteId = value; }
        public GameObject Prefab { get => prefab; set => prefab = value; }
    }

}
