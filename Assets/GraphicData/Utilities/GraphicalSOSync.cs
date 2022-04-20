using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
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

    private void Start()
    {
    }


    void Update()
    {
        if (GraphicInstance == null) return;

        transform.position = GraphicInstance.Position;

        switch (GraphicInstance.BaseWrapped)
        {
            case SinkBase sink:


                NameText.text = sink.Name;
                gameObject.name = sink.Name;

                AttributeText.text = "" + sink.Consumption + " A";

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
