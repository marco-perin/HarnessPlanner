using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SourceBaseSO", menuName = "CoreDataSO/SourceBaseSO")]
    public class SourceBaseSO : BaseObjectSO, INodeSO
    {
    }
}