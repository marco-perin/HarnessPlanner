using System;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIConnectionPanelManager : MonoBehaviour
{

    public UINodeConfigurationPanelManager parentPanelManager;

    public List<TMP_Dropdown.OptionData> nodes_options;
    public List<TMP_Dropdown.OptionData> harness_options;

    public TMP_Dropdown node_dd;
    public TMP_Text node_dd_text;
    public TMP_Text pin_type_text;
    public TMP_Dropdown pin_type_dd;
    public TMP_Dropdown harness_node_dd;
    public TMP_Dropdown harness_pin_dd;

    public Button thisButton;


    private IPinData thisPinData;
    private ConnectorNodeBaseSO thisConnData;
    private AssetReference thisAddrAsset;


    void Start()
    {
        selectablePins = new Dictionary<IBaseNodeWithPinnedSO, IEnumerable<IPinData>>();
    }

    private List<IBaseNodeWithPinnedSO> selectableNodes;
    private Dictionary<IBaseNodeWithPinnedSO, IEnumerable<IPinData>> selectablePins;


    public void SetParent(UINodeConfigurationPanelManager parent)
    {
        parentPanelManager = parent;

        pin_type_dd.onValueChanged.RemoveAllListeners();

        pin_type_dd.ClearOptions();

        if (thisPinData != null)
        {
            harness_pin_dd.onValueChanged.RemoveAllListeners();
            harness_pin_dd.ClearOptions();
            harness_node_dd.onValueChanged.RemoveAllListeners();
            harness_node_dd.ClearOptions();


            pin_type_dd.options = Enum.GetNames(typeof(PinTypeEnum)).Select(s => new TMP_Dropdown.OptionData(s)).ToList();
            pin_type_dd.onValueChanged.AddListener((value) => SelectPinType(value));


            selectableNodes = parentPanelManager.connectedNodesList;

            if (thisPinData.PinType == PinTypeEnum.Power)
            {
                selectableNodes = selectableNodes.Where(n => n is ISource).ToList();
            }

            harness_options = selectableNodes.Select(node => new TMP_Dropdown.OptionData(node.Name)).Prepend(new("none")).ToList();

            harness_node_dd.options = harness_options;
            harness_node_dd.onValueChanged.AddListener((selectionIndex) => SelectNode(selectionIndex - 1));

        }
        else if (thisConnData != null)
        {

            //harness_node_dd.options = thisConnData.Variants.Select(s => new TMP_Dropdown.OptionData(s)).ToList();
            //harness_node_dd.onValueChanged.AddListener((value) => SelectConnectorVariant(value));

            //harness_options = selectableNodes.Select(node => new TMP_Dropdown.OptionData(node.Name)).Prepend(new("none")).ToList();
            if (thisButton == null)
                thisButton = GetComponent<Button>();

            node_dd.options = new string[] { thisConnData.Name }.Select(s => new TMP_Dropdown.OptionData(s)).ToList();

            //pin_type_dd.options = Enum.GetNames(typeof(ConnectorTypeEnum)).Select(str => new TMP_Dropdown.OptionData(str)).ToList();
            //pin_type_dd.onValueChanged.AddListener((selectionIndex) => SelectConnectorType(selectionIndex));

            thisButton.onClick.RemoveAllListeners();
            thisButton.onClick.AddListener(() => parent.SelectConnectorSO(thisConnData, thisAddrAsset));


            pin_type_dd.options = new string[] { "" + thisConnData.PinConfiguration.PinCount }.Select(s => new TMP_Dropdown.OptionData(s)).ToList();
        }
    }


    private void SelectNode(int selectionIndex)
    {
        if (selectionIndex < 0)
        {
            harness_pin_dd.value = 0;

            parentPanelManager.SelectNode(thisPinData, null, null);
            harness_pin_dd.onValueChanged.RemoveAllListeners();
            harness_pin_dd.ClearOptions();

            return;

        }

        var node = selectableNodes[selectionIndex];

        parentPanelManager.SelectNode(thisPinData, node, null);

        Debug.Assert(node.BaseSO is IPinnedObjectSO);

        if (selectablePins == null)
            selectablePins = new Dictionary<IBaseNodeWithPinnedSO, IEnumerable<IPinData>>();

        selectablePins[node] = (node.BaseSO as IPinnedObjectSO).PinConfiguration.PinDataArray.OrderBy(p => p.Id);

        harness_pin_dd.onValueChanged.RemoveAllListeners();
        harness_pin_dd.ClearOptions();
        harness_pin_dd.options = selectablePins[node].Select(x => new TMP_Dropdown.OptionData($"{x.PinNumber}-{x.Name}")).Prepend(new("none")).ToList();

        harness_pin_dd.onValueChanged.AddListener((value) => SelectPinForNode(thisPinData, node, value - 1));
    }

    private void SelectPinType(int selectionIndex)
    {
        if (selectionIndex < 0) return;
        var type = (PinTypeEnum)selectionIndex;

        parentPanelManager.SetPinType(thisPinData, type);
    }

    //private void SelectConnectorVariant(int selectionIndex)
    //{
    //    if (selectionIndex < 0) return;
    //    var variant = thisConnData.Variants[selectionIndex];

    //    //parentPanelManager.SetConnectorVariant(thisConnData, variant);

    //}

    //private void SelectConnectorType(int selectionIndex)
    //{
    //    if (selectionIndex < 0) return;
    //    var connType = (ConnectorTypeEnum)selectionIndex;

    //    parentPanelManager.SetConnectorType(thisConnData, connType);
    //}

    private void SelectPinForNode(IPinData thisPinData, IBaseNodeWithPinnedSO node, int selectionIndex)
    {
        if (selectionIndex < 0) return;
        var pinData = selectablePins[node].ElementAt(selectionIndex);

        parentPanelManager.SelectPinForNode(thisPinData, node, pinData);
    }

    public void SetPinData(IPinData pinData)
    {
        thisPinData = pinData;
        var pinDataString = $"{thisPinData.PinNumber}-{thisPinData.Name}";
        nodes_options.Clear();
        nodes_options.Add(new(pinDataString));

        if (node_dd != null)
        {
            node_dd.options = nodes_options;
            //node_dd.SetValueWithoutNotify(0);
            node_dd.value = 0;
        }

        else if (node_dd_text != null)
        {
            node_dd_text.text = pinDataString;
        }

    }

    internal void InitOptions(IBaseNodeWithPinnedSO base_node)
    {
        if (base_node == null)
        {
            harness_node_dd.value = 0;
            harness_pin_dd.value = 0;
            pin_type_dd.value = 0;
            return;
        }

        if (thisPinData != null)
        {

            pin_type_dd.value = (int)thisPinData.PinType;

            var c = base_node.Connections.SingleOrDefault(c => c.PinFromData.Equals(thisPinData));

            var harness_value = 0;
            if (c != null)
                harness_value = selectableNodes.FindIndex(n => n == c.ConnectedNode) + 1;

            harness_node_dd.value = harness_value;
            //harness_node_dd.onValueChanged.Invoke(harness_value);

            var pin_value = 0;
            if (harness_value > 0)
                if (c.ConnectedNode is IBaseNodeWithPinnedSO connected_as_with_pinned_so)
                    pin_value = selectablePins[connected_as_with_pinned_so].ToList().FindIndex(p => p.Equals(c.PinToData)) + 1;

            harness_pin_dd.value = pin_value;

        }
        //harness_pin_dd.onValueChanged.Invoke(pin_value);
    }

    internal void SetConnectorData(ConnectorNodeBaseSO connectorData, AssetReference assetReference)
    {
        thisConnData = connectorData;
        thisAddrAsset = assetReference;

        var pinDataString = connectorData.Name;

        nodes_options.Clear();
        nodes_options.Add(new(pinDataString));

        if (node_dd != null)
        {
            node_dd.options = nodes_options;
            //node_dd.SetValueWithoutNotify(0);
            node_dd.value = 0;
        }

        else if (node_dd_text != null)
        {
            node_dd_text.text = pinDataString;
        }

    }
}
