using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class GraphicInstanceDeletable : MonoBehaviourGraphicInstanced, IPointerDownHandler
{

    public bool isDeleting = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isDeleting) return;
        DeleteInstance(GraphicInstance);
    }

    private void DeleteInstance(IGraphicInstance thisGraphicInstance)
    {
        Debug.Assert(thisGraphicInstance != null);

        Debug.Log($"Deleting {gameObject.name} Node: {(GraphicInstance.BaseWrapped is INode inode ? inode.Name : "Node")}");

        var connMgr = MainConnectionsManagerSingleton.Instance;

        var otherNodes = connMgr.GetNodesConnectedToNode(thisGraphicInstance);

        foreach (var node in otherNodes)
        {
            if (node.BaseWrapped is not IBaseNodeWithPinnedSO basePinned) continue;

            // Remove Connections To this node
            basePinned.Connections = basePinned.Connections.Where(c => c.ConnectedNode != thisGraphicInstance);
        }

        foreach (var connGO in connMgr.ActiveConnectionsGO)
        {
            var conn = connGO.GraphicInstance.BaseWrapped as INodeLinkBase;
            if (conn.ToNode == thisGraphicInstance || conn.FromNode == thisGraphicInstance)
                connMgr.DeleteConnection(connGO);
        }

        Destroy(gameObject);
    }

    void Start()
    {
        InputManager.Instance.AddKeyDownAction(KeyCode.X, () => isDeleting = true);
        InputManager.Instance.AddKeyDownAction(KeyCode.None, () => isDeleting = false);

    }

}
