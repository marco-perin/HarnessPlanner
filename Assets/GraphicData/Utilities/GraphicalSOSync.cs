using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Button connectionsBtn;

    public Button ConnectionsBtn
    {
        get
        {
            if (connectionsBtn == null)
                connectionsBtn = transform.GetChild(0).Find("ConnectionsBtn").GetComponent<Button>();

            return connectionsBtn;
        }
        set => connectionsBtn = value;
    }

    ConnectionPrefabManager connectionPrefabManager;

    private void Start()
    {
        switch (GraphicInstance.BaseWrapped)
        {
            case SinkBase:
            case SourceBase:
                ConnectionsBtn.onClick.RemoveAllListeners();
                ConnectionsBtn.onClick.AddListener(() => UINodePanelSpawner.Instance.SpawnPanel(GraphicInstance));
                break;
            default:
                break;
        }
    }

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

                //ConnectionsBtn.onClick.RemoveAllListeners();

                break;
            case SourceBase source:

                transform.position = GraphicInstance.Position;

                NameText.text = source.Name;
                gameObject.name = source.Name;

                AttributeText.text = "Max: " + source.MaxAvailability + " A";

                //ConnectionsBtn.onClick.RemoveAllListeners();

                break;

            case ConnectionNodeBase node:
                transform.position = GraphicInstance.Position;
                break;

            case NodeLinkBase nodeLink:

                if (connectionPrefabManager == null)
                    connectionPrefabManager = GetComponent<ConnectionPrefabManager>();

                connectionPrefabManager.To.position = nodeLink.ToNode.Position + GraphicInstance.Position.z * Vector3.forward;
                connectionPrefabManager.From.position = nodeLink.FromNode.Position + GraphicInstance.Position.z * Vector3.forward;
                connectionPrefabManager.LengthText.text = nodeLink.Length + "m";

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
