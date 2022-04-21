using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "SinkWithSignal", menuName = "CoreDataSO/SinkWithSignal")]
    public class SinkWithSignalSO : SinkBase, ISinkWithSignal
    {
        [SerializeField]
        private SignalConnectibleBase[] signalConnectibles;

        public SinkWithSignalSO(SinkBaseSO baseSO) : base(baseSO)
        {
        }

        public ISignalConnectible[] SignalConnectibles { get => signalConnectibles; set => signalConnectibles = value as SignalConnectibleBase[]; }
    }
}