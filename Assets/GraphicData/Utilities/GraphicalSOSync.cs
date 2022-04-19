using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;

public class GraphicalSOSync : MonoBehaviourGraphicInstanced
{


    private void Start()
    {
    }


    void Update()
    {
        if (GraphicInstance == null) return;
        transform.position = GraphicInstance.Position;
    }

    internal void GenerateConnectibles()
    {
        //var sink = GraphicInstance as ISink;

        //var go = Instantiate(
        //    original: MainManagerSingleton.Instance.Settings.DefaultConnectiblePrefab,
        //    parent: transform
        //    );
        gameObject.AddComponent<ConnectibleManager>();
    }
}
