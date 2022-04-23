using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using TMPro;
using UnityEngine;

public class UINodeConfigurationPanelManager : MonoBehaviour
{
    public TMP_InputField name_input;
    public TMP_InputField attribute_input;
    public TMP_Text attribute_text;

    public Transform pinRowsContainer;
    public GameObject pinLinePrefab;

    public IGraphicInstance graphicInstance;

    public List<INode> connectedNodesList;
    private List<INode> selectedNodesList;

    //public IEnumerable<INode> AvailableNodes;

    void Start()
    {
        selectedNodesList = new();
        connectedNodesList = new();
    }

    public void ClosePanel()
    {
        UINodePanelSpawner.Instance.ClosePanel();
    }

    public void SetGraphicInstance(IGraphicInstance gInstance)
    {

        if (gInstance == null)
            return;

        string text = gInstance.BaseWrapped switch
        {
            SinkBase sb => "Consumption: ",
            SourceBase sb => "Max Current: ",
            _ => null
        };

        var attributeInputType = gInstance.BaseWrapped switch
        {
            SinkBase => TMP_InputField.ContentType.DecimalNumber,
            SourceBase => TMP_InputField.ContentType.DecimalNumber,
            _ => TMP_InputField.ContentType.Standard,

        };

        if (text == null)
            attribute_text.text = "Err";
        else
            attribute_text.text = text;

        attribute_input.contentType = attributeInputType;

        attribute_input.text = gInstance.BaseWrapped switch
        {
            SinkBase sb => sb.Consumption.ToString(),
            SourceBase sb => sb.MaxAvailability.ToString(),
            _ => "",

        };

        attribute_input.onValueChanged.RemoveAllListeners();

        switch (gInstance.BaseWrapped)
        {
            case SinkBase sb:
                attribute_input.onValueChanged.AddListener((value) => sb.Consumption = double.Parse(value));
                break;
            case SourceBase sb:
                attribute_input.onValueChanged.AddListener((value) => sb.MaxAvailability = double.Parse(value));
                break;
        }


        name_input.text = (gInstance.BaseWrapped as INode).Name;

        name_input.onValueChanged.RemoveAllListeners();

        name_input.onValueChanged.AddListener((newName) => (gInstance.BaseWrapped as INode).Name = newName);


        var connectedNodes = MainConnectionsManagerSingleton.Instance.GetNodesConnectedToNode(gInstance);

        if (connectedNodes.Any())
        {
            connectedNodesList.Clear();
            foreach (var node in connectedNodes)
            {
                if (node.BaseWrapped is INode inode && inode.BaseSO is IPinnedObjectSO pinnedObjectSO)
                    connectedNodesList.Add(inode);
            }
        }

        if (gInstance.BaseWrapped is INode pinned_obj && pinned_obj.BaseSO is IPinned pinned_so)
        {
            var pinCount = pinned_so.PinConfiguration.PinDataArray.Count();

            // remove Additional childs, if any
            for (int i = pinRowsContainer.childCount - 1; i >= pinCount; i--)
            {
                Destroy(pinRowsContainer.GetChild(i).gameObject);
            }

            for (int i = 0; i < pinCount; i++)
            {
                IPinData pinData = pinned_so.PinConfiguration.PinDataArray.ElementAt(i);

                UIConnectionPanelManager panelMgr;
                if (i < pinRowsContainer.childCount)
                    panelMgr = pinRowsContainer.GetChild(i).GetComponent<UIConnectionPanelManager>();
                else
                    panelMgr = Instantiate(pinLinePrefab, pinRowsContainer).GetComponent<UIConnectionPanelManager>();

                panelMgr.SetPinData(pinData);

                panelMgr.SetParent(this);

            }
        }



    }

    public IEnumerable<INode> SelectNode(INode node, INode previousSelectedNode)
    {
        if (previousSelectedNode != null)
            selectedNodesList.Remove(previousSelectedNode);

        selectedNodesList.Add(node);

        // TODO: Check for faster implementations (caching)
        return connectedNodesList.Where(cn => !selectedNodesList.Contains(cn));
    }


    void Update()
    {
        if (graphicInstance == null) return;


        switch (graphicInstance.BaseWrapped)
        {
            case SinkBase sink:

                //name_input.text = sink.Name;
                //gameObject.name = sink.Name;

                //attribute_text.text = 

                //ConnectionsBtn.onClick.RemoveAllListeners();
                //ConnectionsBtn.onClick.AddListener(() =>);

                break;
            case SourceBase source:

                break;

            case ConnectionNodeBase node:

                break;

            case NodeLinkBase nodeLink:


                break;
            default:
                break;
        }
    }
}
