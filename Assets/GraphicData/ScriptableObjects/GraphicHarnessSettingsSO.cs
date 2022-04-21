using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GraphicHarnessSettingsSO", menuName = "SO/Settings")]
    public class GraphicHarnessSettingsSO : ScriptableObject
    {
        [SerializeField] private float nodesPlaceHeight = 0.1f;
        [SerializeField] private float connectionsPlaceHeight = 0.5f;
        [SerializeField] private SourceBaseSO defaultSourcePrefab;
        [SerializeField] private SinkBaseSO defaultSinkPrefab;
        [SerializeField] private ConnectionNodeBaseSO defaultNodePrefab;
        [SerializeField] private NodeLinkBaseSO defaultLinkPrefab;

        public SourceBaseSO DefaultSourcePrefab { get => defaultSourcePrefab; }
        public SinkBaseSO DefaultSinkPrefab { get => defaultSinkPrefab; }
        public ConnectionNodeBaseSO DefaultNodePrefab { get => defaultNodePrefab; }
        public NodeLinkBaseSO DefaultLinkPrefab { get => defaultLinkPrefab; }

        public float NodesPlaceHeight => nodesPlaceHeight;
        public float ConnectionsPlaceHeight => connectionsPlaceHeight;
    }
}
