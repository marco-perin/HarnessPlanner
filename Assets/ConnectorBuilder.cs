using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectorBuilder : MonoBehaviour
{
    public TMP_Text text;
    public PinGenerator pinGenerator;

    // Start is called before the first frame update
    void Start()
    {
        text ??= GetComponentInChildren<TMP_Text>();
        pinGenerator ??= GetComponent<PinGenerator>();

    }

    public void SetData(ConnectorData cData)
    {
        Debug.Assert(cData != null);

        pinGenerator ??= GetComponent<PinGenerator>();
        text.text = cData.name;
        pinGenerator.connector = cData;
        pinGenerator.GenerateData();
    }
}
