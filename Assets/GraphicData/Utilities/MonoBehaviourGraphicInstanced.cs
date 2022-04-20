using Assets.GraphicData.Interfaces;
using UnityEngine;

[RequireComponent(typeof(MonoBehaviourGraphicInstanceContainer))]
public class MonoBehaviourGraphicInstanced : MonoBehaviour
{
    private MonoBehaviourGraphicInstanceContainer _MBContainer;

    public IGraphicInstance GraphicInstance
    {
        get => MBContainer.GraphicInstance;
        set
        {
            MBContainer.GraphicInstance = value;
        }
    }

    public MonoBehaviourGraphicInstanceContainer MBContainer
    {
        get
        {
            if (_MBContainer == null)
                _MBContainer = GetComponent<MonoBehaviourGraphicInstanceContainer>();
            return _MBContainer;
        }
        set => _MBContainer = value;
    }

}