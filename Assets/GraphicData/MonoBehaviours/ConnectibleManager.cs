using System;
using System.Collections;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using Assets.GraphicData.Types;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConnectibleManager : MonoBehaviourGraphicInstanced, IPointerClickHandler//, IClickable
{
   [Header("Runtime Variables")]
   public bool connecting;

   public bool IsConnecting { get => connecting; }

   private void Start()
   {
      InputManager.Instance.AddKeyDownAction(KeyCode.C, () => StartConnecting());
      InputManager.Instance.AddKeyDownAction(KeyCode.D, () => StopConnecting());

   }

   private bool isHoldingConnection = false;
   private void HoldConnection(bool value = true)
   {
      isHoldingConnection = value;
   }

   private void StartConnecting()
   {
      MainConnectionsManagerSingleton.Instance.ResetConnectionState();
      connecting = true;
   }

   private void StopConnecting()
   {
      MainConnectionsManagerSingleton.Instance.ResetConnectionState();
      connecting = false;
   }

   //public void Click()
   //{
   //    if (IsConnecting)
   //        MainConnectionsManagerSingleton.Instance.Connect(this);

   //}

   public void OnPointerClick(PointerEventData eventData)
   {
      //throw new NotImplementedException();
      if (IsConnecting)
         MainConnectionsManagerSingleton.Instance.Connect(this);
   }
}
