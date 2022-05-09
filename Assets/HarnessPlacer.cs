using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

#if  ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public enum PlacingType
{
    None,
    Source,
    Sink,
    Node,
    Link,
    Connector,
    Deleting
}

public class HarnessPlacer : MonoBehaviour, IPointerDownHandler
{
    [Header("Scene Config")]
    public Transform newObjectsParentTransform;

    //[Header("Scriptable Objects")]
    public HarnessSettingSOLoaded HarnessSettings { get => ProgramManagerSingleton.Instance.HarnessSettingsSO; }
    public GraphicHarnessSettingsSO HarnessSettingsAddressable { get => ProgramManagerSingleton.Instance.HarnessSettingsSOAddressables; }

    [Header("Runtime Variables")]
    public PlacingType placing;

    public bool IsPlacing { get => placing != PlacingType.None; }

    public PointerEventData.InputButton inputButton = PointerEventData.InputButton.Left;

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

        InputManager.Instance.AddKeyDownAction(KeyCode.D,
            () => placing = PlacingType.Connector,
            true
            );

        InputManager.Instance.AddKeyDownAction(KeyCode.None,
            () => placing = PlacingType.None
            );
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != inputButton) return;

        StartInteraction(newObjectsParentTransform.InverseTransformPoint(eventData.pointerPressRaycast.worldPosition));
    }

    public void StartInteraction(Vector3 param)
    {
        if (IsPlacing)
        {
            var snapGridSize = HarnessSettings.SnapGridSize;
            param.x = Mathf.Round(param.x / snapGridSize) * snapGridSize;
            param.y = Mathf.Round(param.y / snapGridSize) * snapGridSize;


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

                case PlacingType.Connector:
                    PlaceConnector(param);
                    break;
            }
        }
    }

    private void PlaceSource(Vector3 pos)
    {

        //throw new NotImplementedException();
        var baseObj = new SourceBase(HarnessSettings.DefaultSourcePrefab)
        {
            BaseSOAddressableTyped = HarnessSettingsAddressable.DefaultSourcePrefab
        };

        IGraphicInstance wrapper = new SourceGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, HarnessSettings.NodesPlaceHeight)
        };


        CreateGraphicWrapper(wrapper, newObjectsParentTransform);
    }

    private void PlaceSink(Vector3 pos)
    {
        var baseObj = new SinkBase(HarnessSettings.DefaultSinkPrefab)
        {
            BaseSOAddressableTyped = HarnessSettingsAddressable.DefaultSinkPrefab
        };

        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();
        IGraphicInstance wrapper = new SinkGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, HarnessSettings.NodesPlaceHeight)
        };


        CreateGraphicWrapper(wrapper, newObjectsParentTransform);
    }

    private void PlaceNode(Vector3 pos)
    {
        var baseObj = new ConnectionNodeBase(HarnessSettings.DefaultNodePrefab)
        {
            BaseSOAddressableTyped = HarnessSettingsAddressable.DefaultNodePrefab
        };

        IGraphicInstance wrapper = new ConnectionNodeBaseGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, HarnessSettings.NodesPlaceHeight)
        };

        // Instantiate the scene GameObject prefab
        CreateGraphicWrapper(wrapper, newObjectsParentTransform);
    }
    private void PlaceConnector(Vector3 pos)
    {
        
        var baseObj = new ConnectorBase(HarnessSettings.DefaultConnectorPrefab)
        {
            BaseSOAddressableTyped = HarnessSettingsAddressable.DefaultConnectorPrefab
        };

        IGraphicInstance wrapper = new ConnectorGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, HarnessSettings.NodesPlaceHeight)
        };

        // Instantiate the scene GameObject prefab
        CreateGraphicWrapper(wrapper, newObjectsParentTransform);
    }

    public static void CreateGraphicWrapper(IGraphicInstance wrapper, Transform parent)
    {
        Object prefab = (wrapper.BaseWrapped as INode).BaseSO.Prefab;
        GameObject sinkPrefabGo;

        sinkPrefabGo = Instantiate(prefab as GameObject, parent);

        sinkPrefabGo.transform.localPosition = wrapper.Position;
        sinkPrefabGo.name = (wrapper.BaseWrapped as INode).Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = wrapper;


        graphSyncMB.GenerateConnectibles();
    }

}
