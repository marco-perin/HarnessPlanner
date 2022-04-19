//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class PinGenerator : MonoBehaviour
//{
//    public GameObject pinPrefab;
//    public ConnectorData connector;

//    void Start()
//    {

//    }

//    void Update()
//    {

//    }

//    private void OnDrawGizmos()
//    {
//        if (connector == null) return;
//        if (connector.connector == null) return;

//        foreach (var pos in Getpositions())
//        {
//            Gizmos.DrawCube(transform.TransformPoint(pos), Vector3.one * 0.2f);
//        }
//    }

//    internal void GenerateData()
//    {
//        var positions = Getpositions().ToArray();
//        for (int i = 0; i < Mathf.Min(positions.Count(), connector.Pins.Count()); i++)
//        {
//            connector.Pins[i].position = positions[i];
//        }
//    }

//    private IEnumerable<Vector3> Getpositions() => connector.connector.PinConfig?.PinPositions;
//}
