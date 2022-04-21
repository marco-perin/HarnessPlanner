using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.ScriptableObjects;
using Assets.GraphicData.Types;
using Assets.GraphicData.Interfaces;

public class SaveManager : MonoBehaviour
{
    public Transform nodesParent;
    public ConnectionsManagerSingleton connectionsManager;

    private void Start()
    {
        connectionsManager = ConnectionsManagerSingleton.Instance;
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

        SaveData sd = new SaveData()
        {
            sinks = sinks.Select(n => n as SinkGraphicBaseWrapper).ToList(),
            sources = sources.Select(n => n as SourceGraphicBaseWrapper).ToList(),
            nodes = nodes.Select(n => n as ConnectionNodeBaseGraphicBaseWrapper).ToList(),
            links = links.Select(n => n as NodeLinkBaseGraphicBaseWrapper).ToList()
        };

        string json = JsonUtility.ToJson(sd, true);
        Debug.Log(json);

        string fullPath = Path.Combine(Application.dataPath, "Saves", fileName);
        string dirPath = fullPath.Substring(0, fullPath.LastIndexOfAny("/\\".ToCharArray()));
        var d = Directory.CreateDirectory(dirPath);
        //d.Close();

        File.WriteAllText(fullPath, json);
        //json = JsonConvert.SerializeObject(sd, Formatting.Indented);
        //File.WriteAllText(Path.Combine(Application.dataPath, "Saves", "SaveFileNSJson.json"), json);


        //Debug.Log(JsonConvert.SerializeObject(c.GraphicInstance));
        //return true;
        //foreach(
        //JsonConvert.SerializeObject()
    }

    public void Load()
    {
        Load("SaveFile.json");
    }

    public void Load(string fileName)
    {

        string json = File.ReadAllText(Path.Combine(Application.dataPath, "Saves", fileName));
        SaveData sd = JsonUtility.FromJson<SaveData>(json);

        foreach (var sinkGraphic in sd.sinks)
        {
            CreateSinkGraphicWrapper(sinkGraphic);
        }

        foreach (var sinkGraphic in sd.sources)
        {
            CreateGraphicWrapper(sinkGraphic);
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

    private void CreateSinkGraphicWrapper(SinkGraphicBaseWrapper wrapper)
    {
        // Create The graphic instance wrapper
        //IGraphicInstance graphicInstanceWrapper = ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>();

        // Instantiate the scene GameObject prefab
        var sinkPrefabGo = Instantiate(wrapper.BaseWrapped.BaseSO.Prefab, nodesParent);
        sinkPrefabGo.transform.position = wrapper.Position;
        sinkPrefabGo.name = wrapper.BaseWrapped.Name;

        // Add the graphical Sync to the prefab object
        var graphSyncMB = sinkPrefabGo.AddComponent<GraphicalSOSync>();
        graphSyncMB.GraphicInstance = wrapper;

        graphSyncMB.GenerateConnectibles();
    }

}

[Serializable]
public class SaveData
{
    //public List<SinkGraphicBaseWrapper> sinks;
    public List<SinkGraphicBaseWrapper> sinks;
    public List<SourceGraphicBaseWrapper> sources;
    public List<ConnectionNodeBaseGraphicBaseWrapper> nodes;
    public List<NodeLinkBaseGraphicBaseWrapper> links;
}

//[Serializable]
//public class NodeSaveData
//{
//    public List<NodeSaveData> childs;

//    NodeSaveData()
//    {
//        childs = new List<NodeSaveData>();
//    }
//}
