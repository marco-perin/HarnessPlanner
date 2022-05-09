using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ConnectorNodeBaseSO", menuName = "CoreDataBaseSO/ConnectorNodeBaseSO")]
    public class ConnectorNodeBaseSO : BasePinnedObjectSO
    {

        [SerializeField] private Texture2D imgTexture;
        [SerializeField] private string[] variants;

        public string[] Variants => variants;
        public Texture2D ImgTexture => imgTexture;
    }

}