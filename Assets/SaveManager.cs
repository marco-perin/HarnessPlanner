using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public Transform nodesParent;
    public MainConnectionsManagerSingleton connectionsManager;

    public string saveFileName = "SaveFile";
    private string extension = ".json";

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

    public string Extension { get => extension; internal set => extension = value; }

    private void Start()
    {
        connectionsManager = MainConnectionsManagerSingleton.Instance;

    }


    public void SetFileName(string fileName)
    {
        saveFileName = fileName;
        if (!saveFileName.EndsWith(Extension))
            saveFileName += Extension;
    }

    public void Save()
    {

        Save(saveFileName);
    }

    public void Save(string fileName)
    {
        //Debug.Log($"Saving with name: {fileName}");

        var mbgis = nodesParent.GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>();
        var basetypes = mbgis.Select(mbgi => mbgi.GraphicInstance);

        SaveData sd = new(basetypes)
        {
            // TODO: make this more robust.
            canvasShift = nodesParent.transform.position,
        };

        string json = JsonUtility.ToJson(sd, true);

        string fullPath = Path.Combine(BaseSavePath, fileName);
        string dirPath = fullPath[..fullPath.LastIndexOfAny("/\\".ToCharArray())];
        var d = Directory.CreateDirectory(dirPath);

        File.WriteAllText(fullPath, json);
    }

    public void Load()
    {
        if (!saveFileName.EndsWith(Extension))
            saveFileName += Extension;

        Load(saveFileName);
    }

    public void Load(string fileName)
    {

        for (int i = nodesParent.childCount - 1; i >= 0; i--)
            Destroy(nodesParent.GetChild(i).gameObject);
        var fullPath = Path.Combine(BaseSavePath, fileName);
        if (!File.Exists(fullPath)) return;

        string json = File.ReadAllText(fullPath);
        SaveData sd = JsonUtility.FromJson<SaveData>(json);

        nodesParent.transform.position = sd.canvasShift;

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
        sinkPrefabGo.transform.localPosition = wrapper.Position;
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
    public SaveData() { }
    public SaveData(IEnumerable<IGraphicInstance> basetypes)
    {
        var baseObjects = basetypes.Where(bt => bt is BaseGraphicObject);
        //var sinks = baseObjects.Where(bt => bt is SinkGraphicBaseWrapper);
        //var sources = baseObjects.Where(x => x is SourceGraphicBaseWrapper);
        //var nodes = baseObjects.Where(x => x is ConnectionNodeBaseGraphicBaseWrapper);
        //var links = baseObjects.Where(x => x is NodeLinkBaseGraphicBaseWrapper);

        sinks = WhereSelect<SinkGraphicBaseWrapper>(baseObjects).ToList();
        sources = WhereSelect<SourceGraphicBaseWrapper>(baseObjects).ToList();
        nodes = WhereSelect<ConnectionNodeBaseGraphicBaseWrapper>(baseObjects).ToList();
        links = WhereSelect<NodeLinkBaseGraphicBaseWrapper>(baseObjects).ToList();
    }

    private IEnumerable<T> WhereSelect<T>(IEnumerable<IGraphicInstance> baseObjects) where T : BaseGraphicObject
    {
        return baseObjects.Where(bo => bo is T).Select(bo => bo as T);
    }

    public Vector2 canvasShift;
    [SerializeReference] public List<SinkGraphicBaseWrapper> sinks;
    [SerializeReference] public List<SourceGraphicBaseWrapper> sources;
    [SerializeReference] public List<ConnectionNodeBaseGraphicBaseWrapper> nodes;
    public List<NodeLinkBaseGraphicBaseWrapper> links;
}
