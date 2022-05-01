using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
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

    public async void Load()
    {
        if (!saveFileName.EndsWith(Extension))
            saveFileName += Extension;

        if (!ProgramManagerSingleton.Instance.HasLoaded)
        {
            Debug.Log("Not yet loaded all assets");
            return;
        }

        await Load(saveFileName);
    }

    public async Task Load(string fileName)
    {

        for (int i = nodesParent.childCount - 1; i >= 0; i--)
            Destroy(nodesParent.GetChild(i).gameObject);

        var fullPath = Path.Combine(BaseSavePath, fileName);
        if (!File.Exists(fullPath)) return;

        string json = File.ReadAllText(fullPath);
        SaveData sd = JsonUtility.FromJson<SaveData>(json);

        nodesParent.transform.position = sd.canvasShift;

        List<Task<bool>> tasks = new();

        foreach (var sinkGraphic in sd.sinks)
        {
            tasks.Add(CreateGraphicWrapper(sinkGraphic));
        }

        foreach (var sourceGraphic in sd.sources)
        {
            tasks.Add(CreateGraphicWrapper(sourceGraphic));
        }

        foreach (var nodeGraphic in sd.nodes)
        {
            tasks.Add(CreateGraphicWrapper(nodeGraphic));
        }

        foreach (var link in sd.links)
        {
            tasks.Add(CreateGraphicWrapper(link));
        }

        var result = await Task.WhenAll(tasks.ToArray());
        if (result.Any(r => r == false))
            Debug.LogWarning($"Failed Loading {result.Where(r => !r).Count()} assets");
    }

    private async Task<bool> CreateGraphicWrapper(BaseGraphicObject wrapper)
    {
        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();

        // NOTE: this is used temporarily to make this an awaitable task.
        //      It will be instead used in the next line to wait for the object SO to load.
        //await Task.Yield();

        // Instantiate the scene GameObject prefab
        //var sinkPrefabGo = Instantiate((wrapper.BaseWrapped as INode).BaseSO.Prefab, nodesParent);
        var sinkPrefabGoHandle = (wrapper.BaseWrapped as BaseNode).BaseSOAddressable.LoadAssetAsync<BaseObjectSO>();
        var sinkPrefabGoTask = await sinkPrefabGoHandle.Task;

        if (sinkPrefabGoHandle.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            return false;

        var sinkPrefabGo = Instantiate(sinkPrefabGoTask.Prefab, nodesParent);

        sinkPrefabGo.transform.localPosition = wrapper.Position;
        sinkPrefabGo.name = (wrapper.BaseWrapped as INode).BaseSO.Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = wrapper;

        graphSyncMB.GenerateConnectibles();

        return true;
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
