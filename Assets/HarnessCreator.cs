//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Assets.CoreData.ScriptableObjects;
//using Assets.GraphicData.ScriptableObjects;
//using Assets.GraphicData.Types;
//using UnityEngine;

//public enum PlacingType
//{
//    None,
//    Source,
//    Sink,
//    Link
//}

//public class HarnessCreator : MonoBehaviour
//{
//    public SinkBaseSO sinkPrefab;
//    public SourceBaseSO sourcePrefab;

//    public PlacingType placing;

//    public bool IsPlacing { get => placing == PlacingType.None; }

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.S))
//        {
//            placing = PlacingType.Sink;
//        }
//        else if (Input.GetKeyDown(KeyCode.N))
//        {
//            placing = PlacingType.None;
//        }


//        var ray = GetMouseRay();
//        var pos = GetMousePosOnWorld(ray);

//        bool hasCollided = Physics.Raycast(ray, out RaycastHit hitInfo);

//        if (Input.GetKeyDown(KeyCode.Mouse0))
//        {
//            if (IsPlacing)
//                switch (placing)
//                {
//                    case PlacingType.Sink:
//                        PlaceSink(pos);
//                        break;
//                    case PlacingType.Source:
//                        PlaceSource(pos);
//                        break;
//                }
//        }
//        // If pressing
//        if (Input.GetKey(KeyCode.Mouse0))
//        {

//        }
//    }

//    private void PlaceSource(Vector3 pos)
//    {
//        throw new NotImplementedException();
//        //var go = Instantiate(sourcePrefab.Prefab, pos, Quaternion.identity);
//        //go.name = sourcePrefab.name;
//        //var sourceMB = go.AddComponent<GraphicalSOSync>();

//        //var source = new SinkGraphicBaseWrapperSO
//        //{
//        //    BaseWrapped = sinkPrefab,
//        //    Position = pos
//        //};

//        //sourceMB.GraphicInstance = source;
//    }

//    private void PlaceSink(Vector3 pos)
//    {
//        var go = Instantiate(sinkPrefab.Prefab, pos, Quaternion.identity);
//        go.name = sinkPrefab.name;
//        var sourceMB = go.AddComponent<GraphicalSOSync>();
//        //var source = Instantiate(ScriptableObject.CreateInstance<SinkGraphicBaseWrapperSO>());
//        var source = new SinkGraphicBaseWrapperSO
//        {
//            BaseWrapped = sinkPrefab,
//            Position = pos
//        };

//        sourceMB.GraphicInstance = source;
//    }

//    Ray GetMouseRay()
//    {
//        return Camera.main.ScreenPointToRay(Input.mousePosition);
//    }

//    Vector3 GetMousePosOnWorld(Ray ray)
//    {
//        return ray.GetPoint(-Camera.main.transform.position.z);
//    }

//}
