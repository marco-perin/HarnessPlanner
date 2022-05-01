using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GraphicData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GraphicHarnessSettingsSO", menuName = "SO/Settings")]
    public class GraphicHarnessSettingsSO : ScriptableObject
    {

        [SerializeField] private float snapGridSize = 0.5f;

        [SerializeField] private float nodesPlaceHeight = 0.1f;
        [SerializeField] private float connectionsPlaceHeight = 0.5f;
        //[SerializeField] private SourceBaseSO defaultSourcePrefab;
        [SerializeField] private AssetReferenceT<SourceBaseSO> defaultSourcePrefab;
        [SerializeField] private AssetReferenceT<SinkBaseSO> defaultSinkPrefab;
        [SerializeField] private AssetReferenceT<ConnectionNodeBaseSO> defaultNodePrefab;
        [SerializeField] private AssetReferenceT<NodeLinkBaseSO> defaultLinkPrefab;


        public AssetReferenceT<SourceBaseSO> DefaultSourcePrefab => defaultSourcePrefab;
        public AssetReferenceT<SinkBaseSO> DefaultSinkPrefab => defaultSinkPrefab;
        public AssetReferenceT<ConnectionNodeBaseSO> DefaultNodePrefab => defaultNodePrefab;
        public AssetReferenceT<NodeLinkBaseSO> DefaultLinkPrefab => defaultLinkPrefab;

        public float NodesPlaceHeight => nodesPlaceHeight;
        public float SnapGridSize => snapGridSize;
        public float ConnectionsPlaceHeight => connectionsPlaceHeight;
    }

    public class HarnessSettingSOLoaded
    {
        public SourceBaseSO DefaultSourcePrefab { get; set; }
        public SinkBaseSO DefaultSinkPrefab { get; set; }
        public ConnectionNodeBaseSO DefaultNodePrefab { get; set; }
        public NodeLinkBaseSO DefaultLinkPrefab { get; set; }

        public float NodesPlaceHeight { get; set; }
        public float SnapGridSize { get; set; }
        public float ConnectionsPlaceHeight { get; set; }
    }

    //interface IHarnessSettingLoaded
    //{
    //    public AssetReferenceT<SourceBaseSO> DefaultSourcePrefab { get; }
    //    public AssetReferenceT<SinkBaseSO> DefaultSinkPrefab { get; }
    //    public AssetReferenceT<ConnectionNodeBaseSO> DefaultNodePrefab { get; }
    //    public AssetReferenceT<NodeLinkBaseSO> DefaultLinkPrefab { get; }

    //    public float NodesPlaceHeight { get; }
    //    public float SnapGridSize { get; }
    //    public float ConnectionsPlaceHeight { get; }
    //}
}
