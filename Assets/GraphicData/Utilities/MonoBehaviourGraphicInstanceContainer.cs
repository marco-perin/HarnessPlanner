using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using UnityEngine;

public class MonoBehaviourGraphicInstanceContainer : MonoBehaviour
{
    [SerializeField]
    private IGraphicInstance _graphicInstance;

    public IGraphicInstance GraphicInstance
    {
        get => _graphicInstance;
        set
        {
            _graphicInstance = value;
        }
    }
}
