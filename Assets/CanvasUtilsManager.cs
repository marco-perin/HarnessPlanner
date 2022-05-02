using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using UnityEngine;

public class CanvasUtilsManager : MonoBehaviour
{

    public float fitPadding = 1;
    public void ResizeToFit()
    {
        var camera = Camera.main;

        var positions = MainConnectionsManagerSingleton.Instance.connectionsParent
          .GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>()
          .Where(mbContainer => mbContainer.GraphicInstance.BaseWrapped is INode)
          .Select(mbContainer => mbContainer.transform.position);

        if (!positions.Any()) return;

        var min = new Vector2(positions.Min(p => p.x), positions.Min(p => p.y));
        var max = new Vector2(positions.Max(p => p.x), positions.Max(p => p.y));

        var delta = max - min;
        var center = (max + min) / 2;

        camera.orthographicSize = Mathf.Max(delta.x, delta.y) / 2 + fitPadding;
        MainConnectionsManagerSingleton.Instance.connectionsParent.transform
        .position -= new Vector3(
            center.x,
            center.y,
        MainConnectionsManagerSingleton.Instance.connectionsParent.transform.position.z);



    }
}
