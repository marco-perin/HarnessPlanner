using System.Collections;
using System.Collections.Generic;
using Assets.GraphicData.Interfaces;
using Assets.GraphicData.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConnectionPanelManager : MonoBehaviour
{

    public List<TMP_Dropdown.OptionData> nodes_options;
    public List<TMP_Dropdown.OptionData> harness_options;

    public TMP_Dropdown node_dd;
    public TMP_Text node_dd_text;
    public TMP_Dropdown harness_dd;


    void Start()
    {
        //node_dd.OnSelect()
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

        if (harness_dd != null)
        {
            harness_dd.options = harness_options;
        }
    }
}
