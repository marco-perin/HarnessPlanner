
using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    public class BasePinnedObjectSO : BaseObjectSO, IPinnedObjectSO
    {
        [SerializeField]
        private PinConfigBase pinConfiguration;
        public IPinConfiguration PinConfiguration => pinConfiguration;

    }
}
