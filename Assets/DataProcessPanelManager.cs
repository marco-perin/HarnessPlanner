using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Types;
using UnityEngine;

public class DataProcessPanelManager : MonoBehaviour
{
   //public TMPro.TMP_InputField inputField;
   public TMPro.TMP_Dropdown inputDropDownField;

   private void Start()
   {
      //inputField.text = MainCalculatorSingleton.Instance.BatteryNodeName;
      inputDropDownField.onValueChanged.RemoveAllListeners();
      inputDropDownField.onValueChanged.AddListener((value) => SetBatteryNameDirect(inputDropDownField.options[value].text));
   }

   private void Update()
   {
      var sources = MainConnectionsManagerSingleton.Instance.connectionsParent
                      .GetComponentsInChildren<GraphicalSOSync>()
                      .Where(mbgi => mbgi.GraphicInstance is SourceGraphicBaseWrapper);

      inputDropDownField.options = sources.Select(mbgi => new TMPro.TMP_Dropdown.OptionData((mbgi.GraphicInstance as SourceGraphicBaseWrapper).BaseWrapped.Name)).ToList();

   }


   public void SetBatteryName()
   {
      //SetBatteryNameDirect(inputField.text);
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
