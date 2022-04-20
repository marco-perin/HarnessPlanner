using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Assets.CoreData.Interfaces;

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
        var mbgis = nodesParent.GetComponentsInChildren<MonoBehaviourGraphicInstanced>();
        var gis = mbgis.Select(mbgi => mbgi.GraphicInstance.BaseWrapped).ToList();

        foreach (var g in gis)
            Debug.Log(JsonUtility.ToJson(g));

        SaveData sd = new SaveData()
        {
            nodes = gis
        };

        string json = JsonUtility.ToJson(gis, true);
        Debug.Log(JsonUtility.ToJson(sd));
        File.WriteAllText(Path.Combine(Application.dataPath, "SaveFile.json"), json);


        //Debug.Log(JsonConvert.SerializeObject(c.GraphicInstance));
        //return true;
        //foreach(
        //JsonConvert.SerializeObject()
    }
}

[Serializable]
public class SaveData
{
    public List<IBaseTypeSO> nodes;
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
