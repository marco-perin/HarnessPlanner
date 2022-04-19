using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.GraphicData.ScriptableObjects
{
    public interface ICanGenerateConnectibles
    {
        void GenerateConnectibles(Transform parent);
    }
}