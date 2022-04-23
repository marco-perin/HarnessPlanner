using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConnectionPanelManager : MonoBehaviour
{

    public UINodeConfigurationPanelManager parentPanelManager;

    public List<TMP_Dropdown.OptionData> nodes_options;
    public List<TMP_Dropdown.OptionData> harness_options;

    public TMP_Dropdown node_dd;
    public TMP_Text node_dd_text;
    public TMP_Dropdown harness_node_dd;
    public TMP_Dropdown harness_pin_dd;




    void Start()
    {
    }

    private IEnumerable<INode> selectableNodes;

    public void SetParent(UINodeConfigurationPanelManager parent)
    {
        parentPanelManager = parent;

        selectableNodes = parent.connectedNodesList;

        harness_node_dd.ClearOptions();
        harness_pin_dd.ClearOptions();

        //foreach (var node in selectableNodes)
        harness_options = selectableNodes.Select(node => new TMP_Dropdown.OptionData(node.Name)).ToList();
        harness_node_dd.options = harness_options;
        harness_node_dd.onValueChanged.RemoveAllListeners();
        harness_node_dd.onValueChanged.AddListener((selectionIndex) => SelectNode(parent, selectionIndex));
    }

    private void SelectNode(UINodeConfigurationPanelManager parent, int selectionIndex)
    {
        var node = selectableNodes.ElementAt(selectionIndex);
        var selectables = parent.SelectNode(node, null);

        Debug.Assert(node.BaseSO is IPinnedObjectSO);

        harness_pin_dd.ClearOptions();
        harness_pin_dd.options = (node.BaseSO as IPinnedObjectSO).PinConfiguration.PinDataArray.OrderBy(p => p.Id).Select(x => new TMP_Dropdown.OptionData($"{x.Id}-{x.Name}")).ToList();

    }

    public void SetPinData(IPinData pinData)
    {
        var pinDataString = $"{pinData.Id}-{pinData.Name}";
        nodes_options.Clear();
        nodes_options.Add(new(pinDataString));

        if (node_dd != null)
        {
            node_dd.options = nodes_options;
            node_dd.SetValueWithoutNotify(0);
        }
        else if (node_dd_text != null)
        {
            node_dd_text.text = pinDataString;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (graphicObject == null) return;

        if (node_dd != null)
        {
            //node_dd.options = nodes_options;
        }

        if (harness_node_dd != null)
        {
            //harness_node_dd.options = harness_options;
        }
    }
}
