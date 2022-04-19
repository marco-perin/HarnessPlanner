using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using System;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class SignalConnectibleBase : ConnectibleRelativeBase, ISignalConnectible, IConnectibleRelative
    {
        [SerializeField]
        private SignalSO signal;

        public ISignal Signal { get => signal; set => signal = value as SignalSO; }
    }

}
