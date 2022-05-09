using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using UnityEngine;

public class CanvasUtilsManager : MonoBehaviour
{
    Transform nodesParent;

    public TransformDraggable canvasTransformDraggable;
    public CameraScrollHandler cameraScrollHandler;

    public float fitBasePadding = 0.6f;
    public float fitPaddingPercentage = 1;

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

        var aspectRatio = (float)camera.pixelWidth / (float)camera.pixelHeight;

        cameraScrollHandler.IsScolling = false;

        camera.orthographicSize = Mathf.Max(delta.x / aspectRatio, delta.y) / 2 * (1 + fitPaddingPercentage) + fitBasePadding;


        canvasTransformDraggable.StopTracking();

        MainConnectionsManagerSingleton.Instance.connectionsParent.transform
        .position -= new Vector3(
            center.x,
            center.y,
        MainConnectionsManagerSingleton.Instance.connectionsParent.transform.position.z);

    }

    public static List<IGraphicInstance> GetAllGraphicInstanceWithBaseType<T>() where T : IBaseType
    {
        return MainCalculatorSingleton.Instance.nodesParent
          .GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>()
          .Select(mb => mb.GraphicInstance)
          .Where(gi => gi.BaseWrapped is T)
          .ToList();
    }

    public static List<IGraphicInstance> GetAllGraphicInstanceWithType<T>() where T : IGraphicInstance
    {
        return MainCalculatorSingleton.Instance.nodesParent
          .GetComponentsInChildren<MonoBehaviourGraphicInstanceContainer>()
          .Select(mb => mb.GraphicInstance)
          .Where(gi => gi is T)
          .ToList();
    }
}
