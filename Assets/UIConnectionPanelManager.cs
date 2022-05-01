using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using TMPro;
using UnityEngine;

public class UIConnectionPanelManager : MonoBehaviour
{

    public UINodeConfigurationPanelManager parentPanelManager;

    public List<TMP_Dropdown.OptionData> nodes_options;
    public List<TMP_Dropdown.OptionData> harness_options;

    public TMP_Dropdown node_dd;
    public TMP_Text node_dd_text;
    public TMP_Dropdown harness_node_dd;
    public TMP_Dropdown harness_pin_dd;


    private IPinData thisPinData;


    void Start()
    {
        selectablePins = new Dictionary<IBaseNodeWithPinnedSO, IEnumerable<IPinData>>();
    }

    private List<IBaseNodeWithPinnedSO> selectableNodes;
    private Dictionary<IBaseNodeWithPinnedSO, IEnumerable<IPinData>> selectablePins;

    public void SetParent(UINodeConfigurationPanelManager parent)
    {
        harness_node_dd.onValueChanged.RemoveAllListeners();
        harness_pin_dd.onValueChanged.RemoveAllListeners();

        parentPanelManager = parent;

        selectableNodes = parentPanelManager.connectedNodesList;

        harness_node_dd.ClearOptions();
        harness_pin_dd.ClearOptions();

        harness_options = selectableNodes.Select(node => new TMP_Dropdown.OptionData(node.Name)).Prepend(new("none")).ToList();

        harness_node_dd.options = harness_options;
        harness_node_dd.onValueChanged.AddListener((selectionIndex) => SelectNode(selectionIndex - 1));
    }


    private void SelectNode(int selectionIndex)
    {
        if (selectionIndex < 0) return;
        var node = selectableNodes[selectionIndex];

        parentPanelManager.SelectNode(node, null);

        Debug.Assert(node.BaseSO is IPinnedObjectSO);
        if (selectablePins == null)
            selectablePins = new Dictionary<IBaseNodeWithPinnedSO, IEnumerable<IPinData>>();

        selectablePins[node] = (node.BaseSO as IPinnedObjectSO).PinConfiguration.PinDataArray.OrderBy(p => p.Id);

        harness_pin_dd.onValueChanged.RemoveAllListeners();
        harness_pin_dd.ClearOptions();
        harness_pin_dd.options = selectablePins[node].Select(x => new TMP_Dropdown.OptionData($"{x.PinNumber}-{x.Name}")).Prepend(new("none")).ToList();

        harness_pin_dd.onValueChanged.AddListener((value) => SelectPinForNode(thisPinData, node, value - 1));
    }

    private void SelectPinForNode(IPinData thisPinData, IBaseNodeWithPinnedSO node, int value)
    {
        var pinData = selectablePins[node].ElementAt(value);

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
            return;
        }

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
        //harness_pin_dd.onValueChanged.Invoke(pin_value);
    }
}
