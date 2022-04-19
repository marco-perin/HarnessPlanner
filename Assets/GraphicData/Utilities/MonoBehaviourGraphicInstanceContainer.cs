using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.GraphicData.Interfaces;
using UnityEngine;

public class MonoBehaviourGraphicInstanceContainer : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject _so_clone;

    [SerializeField]
    private BaseObjectSO _so_base_type;

    private IGraphicInstance _graphicInstance;

    public IGraphicInstance GraphicInstance
    {
        get => _graphicInstance;
        set
        {
            _so_clone = value as ScriptableObject;
            _so_base_type = value.BaseWrapped as BaseObjectSO;
            _graphicInstance = value;
        }
    }
}
