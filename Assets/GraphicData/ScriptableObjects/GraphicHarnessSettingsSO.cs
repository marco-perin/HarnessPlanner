using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GraphicHarnessSettingsSO", menuName = "SO/Settings")]
    public class GraphicHarnessSettingsSO : ScriptableObject
    {
        [SerializeField] private GameObject defaultConnectiblePrefab;

        public GameObject DefaultConnectiblePrefab { get; }
    }
}
