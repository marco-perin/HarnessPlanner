using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "Source", menuName = "GraphicDataSO/Source")]
    public class SourceGraphicSO : SourceBaseSO, IGraphicSource
    {
        [SerializeField] private string id;
        [SerializeField] private Vector3 position;
        [SerializeField] private SourceBaseSO baseWrapped;
        [SerializeField] private Vector2 size;

        public string Id { get => id; set => id = value; }
        public Vector3 Position { get => position; set => position = value; }
        public IBaseTypeSO BaseWrapped { get => baseWrapped; set => baseWrapped = value as SourceBaseSO; }
        public Vector2 Size { get => size; set => size = value; }
    }
}