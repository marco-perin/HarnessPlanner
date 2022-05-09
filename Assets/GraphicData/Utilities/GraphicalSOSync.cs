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
                nameText = transform.GetChild(0).Find("NameText")?.GetComponent<TMP_Text>();

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
                connectionsBtn = transform.GetChild(0).Find("ConnectionsBtn")?.GetComponent<Button>();

            return connectionsBtn;
        }
        set => connectionsBtn = value;
    }

    [SerializeField]
    private RawImage imgTexture;

    public RawImage ImgTexture
    {
        get
        {
            if (imgTexture == null)
                imgTexture = transform.GetChild(0).Find("Icon")?.GetComponent<RawImage>();

            return imgTexture;
        }
        set => imgTexture = value;
    }

    ConnectionPrefabManager connectionPrefabManager;

    private void Start()
    {
        switch (GraphicInstance.BaseWrapped)
        {
            //case SinkBase:
            //case SourceBase:
            case IBaseNodeWithPinnedSO:
                ConnectionsBtn.onClick.RemoveAllListeners();
                ConnectionsBtn.onClick.AddListener(() => UINodePanelSpawner.Instance.SpawnPanel(GraphicInstance));
                break;
            case INodeLinkBase:
                break;
            default:
                if (ConnectionsBtn != null)
                    ConnectionsBtn.gameObject.SetActive(false);
                break;
        }

        if (GraphicInstance.BaseWrapped is not INodeLinkBase)
            if (GraphicInstance.BaseWrapped is IConnectionNode)
                NameText.gameObject.SetActive(false);
            else
                NameText.gameObject.SetActive(GraphicInstance.BaseWrapped is INamed);

    }

    private Vector3 Position
    {
        set
        {
            transform.localPosition = value;
        }
    }

    void Update()
    {
        if (GraphicInstance == null) return;

        if (GraphicInstance.BaseWrapped is not INodeLinkBase)
            if (GraphicInstance.BaseWrapped is INamed wrappedNamed)
                NameText.text = wrappedNamed.Name;


        switch (GraphicInstance.BaseWrapped)
        {
            case SinkBase sink:

                Position = GraphicInstance.Position;

                NameText.text = sink.Name;
                gameObject.name = sink.Name;

                AttributeText.text = "" + sink.Consumption + " A";

                break;
            case SourceBase source:

                Position = GraphicInstance.Position;

                NameText.text = source.Name;
                gameObject.name = source.Name;

                AttributeText.text = "Max: " + source.MaxAvailability + " A";

                break;

            case ConnectorBase connectorNode:
                Position = GraphicInstance.Position;
                gameObject.name = connectorNode.Name;
                var img = (connectorNode.BaseSO as ConnectorNodeBaseSO).ImgTexture;
                if (img != null)
                {
                    ImgTexture.gameObject.SetActive(true);
                    ImgTexture.texture = img;
                }
                break;

            case ConnectionNodeBase connectionNode:
                Position = GraphicInstance.Position;
                if (connectionNode.NodeInfo != null && connectionNode.NodeInfo.CMA > 0)
                    AttributeText.text = "" + connectionNode.NodeInfo.CMA + "\nCMA";
                break;


            case NodeLinkBase nodeLink:

                if (connectionPrefabManager == null)
                    connectionPrefabManager = GetComponent<ConnectionPrefabManager>();

                connectionPrefabManager.To.localPosition = nodeLink.ToNode.Position + GraphicInstance.Position.z * Vector3.forward;
                connectionPrefabManager.From.localPosition = nodeLink.FromNode.Position + GraphicInstance.Position.z * Vector3.forward;
                connectionPrefabManager.LengthText.text = nodeLink.Length + "m";

                if (nodeLink.LinkInfo != null)
                {
                    connectionPrefabManager.DataText.gameObject.SetActive(true);
                    connectionPrefabManager.DataText.text = (nodeLink.LinkInfo as LinkInfo).ToString();
                }
                else
                {
                    connectionPrefabManager.DataText.gameObject.SetActive(false);
                }

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
        if (GraphicInstance.BaseWrapped is not NodeLinkBase)
            gameObject.AddComponent<ConnectibleManager>();
    }
}
