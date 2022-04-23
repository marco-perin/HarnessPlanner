using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using System;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Harness", menuName = "CoreDataSO/Harness")]
    public class HarnessGraphicSO : HarnessSO, IHarnessData
    {
        //[SerializeField] private HarnessTopologyGraphic topology;

        //public override IHarnessTopology Topology { get => topology; set => topology = value as HarnessTopologyGraphic; }
    }
}