using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using UnityEngine;

namespace Assets.GraphicData.Interfaces
{
    public interface IGraphicInstance : IWithId
    {
        Vector3 Position { get; set; }
        Vector2 Size { get; set; }
        IBaseTypeSO BaseWrapped { get; set; }
    }
}