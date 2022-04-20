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

public class SaveManager : MonoBehaviour
{
    public Transform nodesParent;
    public ConnectionsManagerSingleton connectionsManager;

    private void Start()
    {
        connectionsManager = ConnectionsManagerSingleton.Instance;
    }

    public void Save(string fileName = "SaveFile.json")
    {
        var mbgis = nodesParent.GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>();
        var basetypes = mbgis.Select(mbgi => mbgi.GraphicInstance);
        var nodes = basetypes.Where(bt => bt is SinkGraphic).Select(bt => bt as SinkGraphic).ToList();

        foreach (var g in nodes)
            Debug.Log(JsonUtility.ToJson(g));

        SaveData sd = new SaveData()
        {
            nodes = nodes
        };

        string json = JsonUtility.ToJson(sd, true);
        Debug.Log(json);
        File.WriteAllText(Path.Combine(Application.dataPath, fileName), json);
        json = JsonConvert.SerializeObject(sd, Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.dataPath, "SaveFileNSJson.json"), json);


        //Debug.Log(JsonConvert.SerializeObject(c.GraphicInstance));
        //return true;
        //foreach(
        //JsonConvert.SerializeObject()
    }

    public void Load(string fileName = "SaveFile.json")
    {
        string json = File.ReadAllText(Path.Combine(Application.dataPath, fileName));
        SaveData sd = JsonUtility.FromJson<SaveData>(json);

        foreach(var n in sd.nodes)
        {

        }

    }
}

[Serializable]
public class SaveData
{
    public List<SinkGraphic> nodes;
}

[Serializable]
public class NodeSaveData
{
    public List<NodeSaveData> childs;

    NodeSaveData()
    {
        childs = new List<NodeSaveData>();
    }
}
