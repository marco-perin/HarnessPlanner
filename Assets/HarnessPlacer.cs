using System;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
//using Assets.UXData.Interfaces;
using UnityEngine.EventSystems;
using UnityEngine;

#if  ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public enum PlacingType
{
    None,
    Source,
    Sink,
    Node,
    Link
}

public class HarnessPlacer : MonoBehaviour, IPointerDownHandler//, IInteractionStartableV3
{
    [Header("Scene Config")]
    public Transform newObjectsParentTransform;

    [Header("Scriptable Objects")]
    public GraphicHarnessSettingsSO harnessSettings;
    //public SinkBaseSO sinkBaseSo;
    //public SourceBaseSO sourceBaseSo;
    //public ConnectionNodeBaseSO nodeBaseSo;


    [Header("Runtime Variables")]
    public PlacingType placing;

    public bool IsPlacing { get => placing != PlacingType.None; }


    private void Start()
    {
        InputManager.Instance.AddKeyDownAction(KeyCode.S,
            () => placing = PlacingType.Sink,
            true
            );

        InputManager.Instance.AddKeyDownAction(KeyCode.P,
            () => placing = PlacingType.Source,
            true
            );

        InputManager.Instance.AddKeyDownAction(KeyCode.N,
            () => placing = PlacingType.Node,
            true
            );

        InputManager.Instance.AddKeyDownAction(KeyCode.None,
            () => placing = PlacingType.None
            );
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown Of Harness placer");
        StartInteraction(eventData.pointerPressRaycast.worldPosition);
    }

    public void StartInteraction(Vector3 param)
    {
        if (IsPlacing)
        {

            switch (placing)
            {
                case PlacingType.Node:
                    PlaceNode(param);
                    break;

                case PlacingType.Sink:
                    PlaceSink(param);
                    break;

                case PlacingType.Source:
                    PlaceSource(param);
                    break;
            }
        }
    }

    private void PlaceSource(Vector3 pos)
    {
        //throw new NotImplementedException();
        var baseObj = new SourceBase(harnessSettings.DefaultSourcePrefab);
        //var prefabGo = Instantiate(sourceBaseSo.Prefab, newObjectsParentTransform);
        IGraphicInstance wrapper = new SourceGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, harnessSettings.NodesPlaceHeight)
        };


        CreateGraphicWrapper(wrapper, newObjectsParentTransform);

        //// Instantiate the scene GameObject prefab
        //var prefabGo = Instantiate(baseObj.BaseSO.Prefab, newObjectsParentTransform);
        //prefabGo.transform.position = pos;
        //prefabGo.name = baseObj.Name;

        //// Add the graphical Sync to the prefab object
        //var graphSyncMB = prefabGo.AddComponent<GraphicalSOSync>();
        //graphSyncMB.GraphicInstance = graphicInstanceWrapper;

        //graphSyncMB.GenerateConnectibles();
        //prefabGo.transform.position = pos;
        //prefabGo.name = sinkBaseSo.name;
        //var graphSyncMB = prefabGo.AddComponent<GraphicalSOSync>();


        //IGraphicInstance source = ScriptableObject.CreateInstance<SourceGraphicBaseWrapperSO>();
        //source.BaseWrapped = sinkBaseSo;
        //source.Position = new Vector3(pos.x, pos.y, 0.1f);

        ////source.Size = sinkBaseSO.
        //graphSyncMB.GraphicInstance = source;
    }

    private void PlaceSink(Vector3 pos)
    {
        var baseObj = new SinkBase(harnessSettings.DefaultSinkPrefab);
        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();
        IGraphicInstance wrapper = new SinkGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, harnessSettings.NodesPlaceHeight)
        };


        CreateGraphicWrapper(wrapper, newObjectsParentTransform);
        //// Instantiate the scene GameObject prefab
        //var sinkPrefabGo = Instantiate(baseObj.BaseSO.Prefab, newObjectsParentTransform);
        //sinkPrefabGo.transform.position = pos;
        //sinkPrefabGo.name = baseObj.Name;

        //// Add the graphical Sync to the prefab object
        //var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        //graphSyncMB.GraphicInstance = graphicInstanceWrapper;

        //graphSyncMB.GenerateConnectibles();
    }

    private void PlaceNode(Vector3 pos)
    {
        var baseObj = new ConnectionNodeBase(harnessSettings.DefaultNodePrefab);

        IGraphicInstance wrapper = new ConnectionNodeBaseGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, harnessSettings.NodesPlaceHeight)
        };

        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();

        // Instantiate the scene GameObject prefab
        CreateGraphicWrapper(wrapper, newObjectsParentTransform);
    }

    public static void CreateGraphicWrapper(IGraphicInstance wrapper, Transform parent)
    {
        var sinkPrefabGo = Instantiate((wrapper.BaseWrapped as INode).BaseSO.Prefab, parent);
        sinkPrefabGo.transform.position = wrapper.Position;
        sinkPrefabGo.name = (wrapper.BaseWrapped as INode).Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = wrapper;


        graphSyncMB.GenerateConnectibles();
    }

}
