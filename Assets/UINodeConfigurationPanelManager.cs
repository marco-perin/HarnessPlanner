using System.Collections;
using System.Collections.Generic;
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

    public IGraphicInstance graphicInstance;


    void Start()
    {

    }


    void SetGraphicInstance(IGraphicInstance gInstance)
    {
        //attribute_text.text = gInstance switch
        //{
        //    SinkBase => 
        //}
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
