using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using TMPro;
using UnityEngine;

public class GraphicalSOSync : MonoBehaviourGraphicInstanced
{
    [SerializeField]
    private TMP_Text nameText;

    public TMP_Text NameText
    {
        get
        {
            if (nameText == null)
                nameText = transform.GetChild(0).Find("NameText").GetComponent<TMP_Text>();

            return nameText;
        }
        set => nameText = value;
    }

    [SerializeField]
    private TMP_Text attributeText;

    public TMP_Text AttributeText
    {
        get
        {
            if (attributeText == null)
                attributeText = transform.GetChild(0).Find("AttributeText").GetComponent<TMP_Text>();

            return attributeText;
        }
        set => attributeText = value;
    }

    ConnectionPrefabManager connectionPrefabManager;

    void Update()
    {
        if (GraphicInstance == null) return;


        switch (GraphicInstance.BaseWrapped)
        {
            case SinkBase sink:

                transform.position = GraphicInstance.Position;

                NameText.text = sink.Name;
                gameObject.name = sink.Name;

                AttributeText.text = "" + sink.Consumption + " A";

                break;
            case SourceBase source:

                transform.position = GraphicInstance.Position;

                NameText.text = source.Name;
                gameObject.name = source.Name;

                AttributeText.text = "Max: " + source.MaxAvailability + " A";

                break;

            case ConnectionNodeBase node:
                transform.position = GraphicInstance.Position;
                break;

            case NodeLinkBase nodeLink:

                connectionPrefabManager = connectionPrefabManager != null ? connectionPrefabManager : GetComponent<ConnectionPrefabManager>();

                connectionPrefabManager.To.position = nodeLink.ToNode.Position + GraphicInstance.Position.z * Vector3.forward;
                connectionPrefabManager.From.position = nodeLink.FromNode.Position + GraphicInstance.Position.z * Vector3.forward;

                break;
            default:
                break;
        }
        //NameText.text = GraphicInstance.BaseWrapped
    }

    internal void GenerateConnectibles()
    {
        //var sink = GraphicInstance as ISink;

        //var go = Instantiate(
        //    original: MainManagerSingleton.Instance.Settings.DefaultConnectiblePrefab,
        //    parent: transform
        //    );
        gameObject.AddComponent<ConnectibleManager>();
    }
}
