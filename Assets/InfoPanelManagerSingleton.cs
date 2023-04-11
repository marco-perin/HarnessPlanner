using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanelManagerSingleton : Singleton<InfoPanelManagerSingleton> 
{
    [SerializeField]
    private TMP_Text StatusText;

    public GameObject Panel;

    public string defaultText;

    private void Start()
    {
        if(string.IsNullOrEmpty(defaultText))
            defaultText = StatusText.text;
    }

    public void SetPanelVisibility(bool visible)
    {
        Panel.SetActive(visible);
    }

    public void SetStatus(string status)
    {
        StatusText.text = status;
    }

    public void ResetStatus()
    {
        SetStatus(defaultText);
    }
}
