using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {

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

            name_input.text = (gInstance.BaseWrapped as INode).Name;

            name_input.onValueChanged.RemoveAllListeners();

            name_input.onValueChanged.AddListener((newName) => (gInstance.BaseWrapped as INode).Name = newName);
        


        if (gInstance.BaseWrapped is INode pinned_obj)
        {
            if (pinned_obj.BaseSO is IPinned pinned_so)
                foreach (var pinData in pinned_so.PinConfiguration.PinDataArray)
                {
                    var panelMgr = Instantiate(pinLinePrefab, pinRowsContainer).GetComponent<UIConnectionPanelManager>();
                    panelMgr.SetPinData(pinData);
                }
        }


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
