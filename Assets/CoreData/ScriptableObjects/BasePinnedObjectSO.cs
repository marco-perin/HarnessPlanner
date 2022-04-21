using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    public class BasePinnedObjectSO : BaseObjectSO
    {
        [SerializeField]
        private PinConfigBase pinConfiguration;
        public IPinConfiguration PinConfiguration => pinConfiguration;

    }

}