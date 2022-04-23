using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public Transform nodesParent;
    public MainConnectionsManagerSingleton connectionsManager;

    private string BaseSavePath
    {
        get
        {
            return
#if UNITY_EDITOR
                Path.Combine(Application.dataPath, "Saves");
#else
            Path.Combine(Application.persistentDataPath, "Saves");
#endif
        }
    }

    private void Start()
    {
        connectionsManager = MainConnectionsManagerSingleton.Instance;
    }

    public void Save()
    {
        Save("SaveFile.json");
    }

    public void Save(string fileName)
    {
        Debug.Log($"Saving with name: {fileName}");

        var mbgis = nodesParent.GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>();
        var basetypes = mbgis.Select(mbgi => mbgi.GraphicInstance);

        var baseObjects = basetypes.Where(bt => bt is BaseGraphicObject);
        var sinks = baseObjects.Where(bt => bt is SinkGraphicBaseWrapper);
        var sources = baseObjects.Where(x => x is SourceGraphicBaseWrapper);
        var nodes = baseObjects.Where(x => x is ConnectionNodeBaseGraphicBaseWrapper);
        var links = baseObjects.Where(x => x is NodeLinkBaseGraphicBaseWrapper);

        SaveData sd = new()
        {
            sinks = sinks.Select(n => n as SinkGraphicBaseWrapper).ToList(),
            sources = sources.Select(n => n as SourceGraphicBaseWrapper).ToList(),
            nodes = nodes.Select(n => n as ConnectionNodeBaseGraphicBaseWrapper).ToList(),
            links = links.Select(n => n as NodeLinkBaseGraphicBaseWrapper).ToList()
        };

        string json = JsonUtility.ToJson(sd, true);

        string fullPath = Path.Combine(BaseSavePath, fileName);
        string dirPath = fullPath[..fullPath.LastIndexOfAny("/\\".ToCharArray())];
        var d = Directory.CreateDirectory(dirPath);

        File.WriteAllText(fullPath, json);
    }

    public void Load()
    {
        Load("SaveFile.json");
    }

    public void Load(string fileName)
    {

        string json = File.ReadAllText(Path.Combine(BaseSavePath, fileName));
        SaveData sd = JsonUtility.FromJson<SaveData>(json);

        foreach (var sinkGraphic in sd.sinks)
        {
            CreateGraphicWrapper(sinkGraphic);
        }

        foreach (var sourceGraphic in sd.sources)
        {
            CreateGraphicWrapper(sourceGraphic);
        }

        foreach (var nodeGraphic in sd.nodes)
        {
            CreateGraphicWrapper(nodeGraphic);
        }

        foreach (var link in sd.links)
        {
            CreateGraphicWrapper(link);
        }
    }

    private void CreateGraphicWrapper(BaseGraphicObject wrapper)
    {
        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();

        // Instantiate the scene GameObject prefab
        var sinkPrefabGo = Instantiate((wrapper.BaseWrapped as INode).BaseSO.Prefab, nodesParent);
        sinkPrefabGo.transform.position = wrapper.Position;
        sinkPrefabGo.name = (wrapper.BaseWrapped as INode).BaseSO.Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = wrapper;

        graphSyncMB.GenerateConnectibles();
    }

}

[Serializable]
public class SaveData
{
    [SerializeReference] public List<SinkGraphicBaseWrapper> sinks;
    [SerializeReference] public List<SourceGraphicBaseWrapper> sources;
    [SerializeReference] public List<ConnectionNodeBaseGraphicBaseWrapper> nodes;
    public List<NodeLinkBaseGraphicBaseWrapper> links;
}
