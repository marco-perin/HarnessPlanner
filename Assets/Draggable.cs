using System.Collections;
using System.Collections.Generic;
using Assets.GraphicData.Interfaces;
using Assets.UXData.Interfaces;
using UnityEngine;

public class Draggable : MonoBehaviourGraphicInstanced, IDraggable
{
    public float dragSpeed = 1;
    Vector3 targetPosition;

    public void StartInteraction(Vector3 param)
    {

    }

    public void Drag(Vector3 dx)
    {
        //Debug.Log("/*Start Drag*/");
        targetPosition += dx;
    }

    // Start is called before the first frame update
    void Start()
    {

        targetPosition = GraphicInstance.Position;
    }

    // Update is called once per frame
    void Update()
    {
        //if ((targetPosition - transform.position).sqrMagnitude > 1e-3)
        //transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime);
        //errq = (targetPosition - GraphicInstance.Position).sqrMagnitude;
        //GraphicInstance.Position = targetPosition;
        GraphicInstance.Position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dragSpeed * 10);
    }

    //private void OnDrawGizmos()
    //{

    //    Gizmos.DrawWireSphere(targetPosition, 5f * transform.localScale.x);
    //}


}
