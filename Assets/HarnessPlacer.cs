using System;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.Types;
using Assets.UXData.Interfaces;
using UnityEngine;

#if  ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public enum PlacingType
{
    None,
    Source,
    Sink,
    Link
}
public class HarnessPlacer : MonoBehaviour, IInteractionStartableV3
{
    [Header("Scene Config")]
    public Transform newObjectsParentTransform;

    [Header("Scriptable Objects")]
    public SinkBaseSO sinkBaseSo;
    public SourceBaseSO sourceBaseSo;


    [Header("Runtime Variables")]
    public PlacingType placing;

    public bool IsPlacing { get => placing != PlacingType.None; }


    private void Start()
    {
        InputManager.Instance.AddAction(KeyCode.S,
            () => placing = PlacingType.Sink,
            true
            );

        InputManager.Instance.AddAction(KeyCode.P,
            () => placing = PlacingType.Source,
            true
            );

        InputManager.Instance.AddAction(KeyCode.None,
            () => placing = PlacingType.None
            );
    }


    public void StartInteraction(Vector3 param)
    {
        if (IsPlacing)
        {

            switch (placing)
            {
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
        var baseObj = new SourceBase(sourceBaseSo);
        //var prefabGo = Instantiate(sourceBaseSo.Prefab, newObjectsParentTransform);
        IGraphicInstance graphicInstanceWrapper = new SourceGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, 0.1f)
        };

        // Instantiate the scene GameObject prefab
        var prefabGo = Instantiate(baseObj.BaseSO.Prefab, newObjectsParentTransform);
        prefabGo.transform.position = pos;
        prefabGo.name = baseObj.Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = prefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = graphicInstanceWrapper;

        graphSyncMB.GenerateConnectibles();
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
        var baseObj = new SinkBase(sinkBaseSo);
        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();
        IGraphicInstance graphicInstanceWrapper = new SinkGraphicBaseWrapper
        {
            BaseWrapped = baseObj,
            Position = new Vector3(pos.x, pos.y, 0.1f)
        };

        // Instantiate the scene GameObject prefab
        var sinkPrefabGo = Instantiate(baseObj.BaseSO.Prefab, newObjectsParentTransform);
        sinkPrefabGo.transform.position = pos;
        sinkPrefabGo.name = baseObj.Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = graphicInstanceWrapper;

        graphSyncMB.GenerateConnectibles();
    }
}
