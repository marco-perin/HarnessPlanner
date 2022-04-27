using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;

public class UINodePanelSpawner : Singleton<UINodePanelSpawner>
{
    public UINodeConfigurationPanelManager panelManager;
    public UINodeConfigurationPanelManager linkPanelManager;


    private void Start()
    {
        ClosePanels();
    }

    public void SpawnPanel(IGraphicInstance instanceWrapper)
    {

        panelManager.gameObject.SetActive(false);
        linkPanelManager.gameObject.SetActive(false);

        switch (instanceWrapper.BaseWrapped)
        {
            case SourceBase:
            case SinkBase:
                panelManager.gameObject.SetActive(true);
                panelManager.SetGraphicInstance(instanceWrapper);
                break;
            case NodeLinkBase:
                linkPanelManager.gameObject.SetActive(true);
                linkPanelManager.SetGraphicInstance(instanceWrapper);
                break;
        }
    }

    public void ClosePanels()
    {
        panelManager.gameObject.SetActive(false);
        linkPanelManager.gameObject.SetActive(false);
        panelManager.SetGraphicInstance(null);
        linkPanelManager.SetGraphicInstance(null);
    }
}
