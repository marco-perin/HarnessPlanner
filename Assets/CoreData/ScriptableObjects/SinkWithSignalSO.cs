﻿using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SinkWithSignal", menuName = "CoreDataSO/SinkWithSignal")]
    public class SinkWithSignalSO : SinkBaseSO, ISinkWithSignal
    {
        [SerializeField]
        private SignalConnectibleBase[] signalConnectibles;

        public ISignalConnectible[] SignalConnectibles { get => signalConnectibles; set => signalConnectibles = value as SignalConnectibleBase[]; }
    }
}