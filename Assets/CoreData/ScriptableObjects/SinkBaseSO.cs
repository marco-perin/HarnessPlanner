using Assets.CoreData.Interfaces;
using Assets.CoreData.Types;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SinkBaseSO", menuName = "CoreDataBaseSO/SinkBaseSO")]
    public class SinkBaseSO : BasePinnedObjectSO, INodeSO, INamed, IPinned
    {
    }
}