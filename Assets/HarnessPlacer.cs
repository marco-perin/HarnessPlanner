using System;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.Types;
using Assets.UXData.Interfaces;
using UnityEngine;

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

    void Update()
    {
        if (!Input.anyKeyDown) return;

        if (Input.GetKeyDown(KeyCode.S))
        {
            placing = PlacingType.Sink;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            placing = PlacingType.None;
        }

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
        var prefabGo = Instantiate(sourceBaseSo.Prefab, newObjectsParentTransform);
        prefabGo.transform.position = pos;
        prefabGo.name = sinkBaseSo.name;
        var graphSyncMB = prefabGo.AddComponent<GraphicalSOSync>();

        throw new NotImplementedException();

        //IGraphicInstance source = ScriptableObject.CreateInstance<SourceGraphicBaseWrapperSO>();
        //source.BaseWrapped = sinkBaseSo;
        //source.Position = new Vector3(pos.x, pos.y, 0.1f);

        ////source.Size = sinkBaseSO.
        //graphSyncMB.GraphicInstance = source;
    }

    private void PlaceSink(Vector3 pos)
    {
        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();
        IGraphicInstance graphicInstanceWrapper = new SinkGraphicBaseWrapperSO
        {
            BaseWrapped = sinkBaseSo,
            Position = new Vector3(pos.x, pos.y, 0.1f)
        };

        // Instantiate the scene GameObject prefab
        var sinkPrefabGo = Instantiate(sinkBaseSo.Prefab, newObjectsParentTransform);
        sinkPrefabGo.transform.position = pos;
        sinkPrefabGo.name = sinkBaseSo.name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = graphicInstanceWrapper;

        graphSyncMB.GenerateConnectibles();
    }
}
