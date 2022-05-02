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
    public TMP_Text name_text;
    public TMP_InputField attribute_input;
    public TMP_Text attribute_text;

    public Transform pinRowsContainer;
    public GameObject pinLinePrefab;

    public IGraphicInstance graphicInstance;

    public List<IBaseNodeWithPinnedSO> connectedNodesList;
    //private List<INode> selectedNodesList;

    //public IEnumerable<INode> AvailableNodes;

    void Awake()
    {
        //selectedNodesList ??= new();
        connectedNodesList ??= new();
    }

    public void ClosePanel()
    {
        if (name_input != null)
            name_input.onValueChanged.RemoveAllListeners();

        if (attribute_input != null)
            attribute_input.onValueChanged.RemoveAllListeners();

        UINodePanelSpawner.Instance.ClosePanels();
    }

    public void SetGraphicInstance(IGraphicInstance gInstance)
    {
        graphicInstance = gInstance;
        if (gInstance == null)
            return;

        string text = gInstance.BaseWrapped switch
        {
            SinkBase sb => "Consumption: ",
            SourceBase sb => "Max Current: ",
            NodeLinkBase lb => "Length: ",
            _ => null
        };

        var attributeInputType = gInstance.BaseWrapped switch
        {
            SinkBase => TMP_InputField.ContentType.DecimalNumber,
            SourceBase => TMP_InputField.ContentType.DecimalNumber,
            NodeLinkBase => TMP_InputField.ContentType.DecimalNumber,
            _ => TMP_InputField.ContentType.Standard,

        };

        if (text == null)
            attribute_text.text = "Err";
        else
            attribute_text.text = text;


        attribute_input.onValueChanged.RemoveAllListeners();

        switch (gInstance.BaseWrapped)
        {
            case SinkBase sb:
                attribute_input.onValueChanged.AddListener((value) => sb.Consumption = ParseFieldIfValid(sb.Consumption, value));
                break;
            case SourceBase sb:
                attribute_input.onValueChanged.AddListener((value) => sb.MaxAvailability = ParseFieldIfValid(sb.MaxAvailability, value));
                break;
            case NodeLinkBase lb:
                attribute_input.onValueChanged.AddListener((value) => lb.Length = ParseFieldIfValid(lb.Length, value));
                break;
        }

        attribute_input.contentType = attributeInputType;

        attribute_input.text = gInstance.BaseWrapped switch
        {
            SinkBase sb => sb.Consumption.ToString(),
            SourceBase sb => sb.MaxAvailability.ToString(),
            NodeLinkBase lb => lb.Length.ToString(),
            _ => ""
        };

        switch (gInstance.BaseWrapped)
        {
            case SinkBase:
            case SourceBase:
                name_input.onValueChanged.RemoveAllListeners();

                name_input.text = (gInstance.BaseWrapped as INode).Name;
                name_input.onValueChanged.AddListener((newName) => (gInstance.BaseWrapped as INode).Name = newName);

                break;
            case NodeLinkBase lb:
                attribute_input.Select();
                attribute_input.ActivateInputField();
                break;

        }

        IEnumerable<IGraphicInstance> connectedNodes = MainConnectionsManagerSingleton.Instance.GetNodesConnectedToNode(gInstance);

        if (connectedNodes.Any())
        {
            connectedNodesList.Clear();
            foreach (var node in connectedNodes)
            {
                if (node.BaseWrapped is IBaseNodeWithPinnedSO inode)
                    connectedNodesList.Add(inode);
            }
        }

        if (gInstance.BaseWrapped is INode pinned_obj && pinned_obj.BaseSO is IPinned pinned_so)
        {
            CreatePinDataWithPanelManager(gInstance, pinned_so);
        }
    }


    private void CreatePinDataWithPanelManager(IGraphicInstance gInstance, IPinned pinned_so)
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

            panelMgr.InitOptions(gInstance.BaseWrapped as IBaseNodeWithPinnedSO);

        }
    }

    internal double ParseFieldIfValid(double field, string inValue)
    {
        return double.TryParse(inValue, out var res) ? res : field;
    }

    internal void SelectPinForNode(IPinData fromPinData, IBaseNodeWithPinnedSO node, IPinData pinToData)
    {

        Debug.Assert(node != null);
        Debug.Assert(pinToData != null);

        if (graphicInstance.BaseWrapped is not IBaseNodeWithPinnedSO pinnedSo)
            return;

        var connections = pinnedSo.Connections as IEnumerable<NodeConnectionTo>;

        //Debug.Log($"Trying to connect pin {fromPinData.Name} to pin {pinToData.Name} of node {node.Name}");
        if (connections != null && connections.Any(c => c.PinFromData.Equals(fromPinData)))
        {
            connections = connections.Select(c =>
            {
                //Debug.Log($"Connecting pin {c.PinFromData.Name} to pin {c.PinToData.Name} of node {node.Name}");
                if (c.PinFromData.Equals(fromPinData))
                {
                    c.ConnectedNode = node;
                    c.PinToData = pinToData;
                }

                return c;
            });
        }
        else
        {
            var newConnection = new NodeConnectionTo()
            {
                PinFromData = fromPinData,
                ConnectedNode = node,
                PinToData = pinToData
            };

            if (connections != null)
                connections = connections.Append(newConnection);
            else
                connections = new List<NodeConnectionTo>() { newConnection };
            //connections.

        }
        pinnedSo.Connections = connections;
    }

    public void SelectNode(IPinData thisPinData, IBaseNodeWithPinnedSO node, IBaseNodeWithPinnedSO previousSelectedNode)
    {
        //Debug.Assert(node != null);

        if (node == null)
        {
            if (graphicInstance.BaseWrapped is not IBaseNodeWithPinnedSO pinnedSo)
                return;

            pinnedSo.Connections = pinnedSo.Connections.Where(c => !c.PinFromData.Equals(thisPinData));
        }

        //    if (previousSelectedNode != null)
        //        selectedNodesList.Remove(previousSelectedNode);

        //    selectedNodesList.Add(node);

        // TODO: Check for faster implementations (caching)
        //return connectedNodesList.Where(cn => !selectedNodesList.Contains(cn));
    }

    internal void SetPinType(IPinData thisPinData, PinTypeEnum type)
    {
        var pinSO = graphicInstance.BaseWrapped as IBaseNodeWithPinnedSO;

        // TODO: this should create copy of arrays, can be optimized.
        pinSO.Connections = pinSO.Connections.Select(c =>
        {
            if (c.PinFromData == thisPinData)
                c.PinFromData.PinType = type;
            return c;
        });
    }



    void Update()
    {
        if (graphicInstance == null) return;


        //switch (graphicInstance.BaseWrapped)
        //{
        //    case SinkBase sink:

        //        //name_input.text = sink.Name;
        //        //gameObject.name = sink.Name;

        //        //attribute_text.text = 

        //        //ConnectionsBtn.onClick.RemoveAllListeners();
        //        //ConnectionsBtn.onClick.AddListener(() =>);

        //        break;
        //    case SourceBase source:

        //        break;

        //    case ConnectionNodeBase node:

        //        break;

        //    case NodeLinkBase nodeLink:


        //        break;
        //    default:
        //        break;
        //}
    }
}
