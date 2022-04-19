using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using System;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Harness", menuName = "CoreDataSO/Harness")]
    public class HarnessSO : ScriptableObject, IHarnessData
    {
        [SerializeField] protected string _name;
        [SerializeField] protected string description;
        [SerializeField] protected string version;
        [SerializeField] protected DateTime date;
        [SerializeField] private HarnessTopologyBase topology;

        public string Name { get => _name; set => _name = value; }
        public string Description { get => description; set => description = value; }
        public string Version { get => version; set => version = value; }
        public DateTime Date { get => date; set => date = value; }
        public virtual IHarnessTopology Topology { get => topology; set => topology = value as HarnessTopologyBase; }
    }
}