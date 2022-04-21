using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SourceBaseSO", menuName = "CoreDataBaseSO/SourceBaseSO")]
    public class SourceBaseSO : BasePinnedObjectSO, INodeSO, INamed
    {
    }

}