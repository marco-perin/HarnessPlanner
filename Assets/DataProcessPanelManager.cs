using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcessPanelManager : MonoBehaviour
{
    public TMPro.TMP_InputField inputField;

    private void Start()
    {
        inputField.text = MainCalculatorSingleton.Instance.BatteryNodeName;
    }

    public void SetBatteryName()
    {
        SetBatteryNameDirect(inputField.text);
    }

    public void SetBatteryNameDirect(string name)
    {
        MainCalculatorSingleton.Instance.BatteryNodeName = name;
    }

    public void ProcessData()
    {
        MainCalculatorSingleton.Instance.CalculateCurrents();
    }
}
