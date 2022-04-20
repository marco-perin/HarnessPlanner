using System;
using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [Serializable]
    public class BaseObjectSO : ScriptableObject, INodeSO
    {
        [SerializeField] private GameObject prefab;
        private string _name;

        public GameObject Prefab { get => prefab; set => prefab = value; }
        public string Name { get => _name; set => _name = value; }
    }
}