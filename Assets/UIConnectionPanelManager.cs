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
    public TMP_Dropdown harness_dd;


    void Start()
    {
        //node_dd.OnSelect()
    }

    // Update is called once per frame
    void Update()
    {
        //if (graphicObject == null) return;

        if (node_dd != null)
        {
            node_dd.options = nodes_options;
        }

        if (harness_dd != null)
        {
            harness_dd.options = harness_options;
        }
    }
}
