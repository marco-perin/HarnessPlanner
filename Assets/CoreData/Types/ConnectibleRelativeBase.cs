using Assets.CoreData.Interfaces;
using System;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class ConnectibleRelativeBase : IConnectibleRelative
    {
        [SerializeField] private string _relativeId;

        private string _parentId;
        [SerializeField] private GameObject prefab;

        public string RelativeId { get => _relativeId; set => _relativeId = value; }
        public string Id { get => GetId(_parentId); }
        public GameObject Prefab { get => prefab; set => prefab = value; }

        public string GetId(string ParentId)
        {
            _parentId = ParentId;
            return $"{ParentId}.{RelativeId}";
        }
    }

}
