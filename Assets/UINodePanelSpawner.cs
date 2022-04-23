using Assets.GraphicData.Interfaces;

public class UINodePanelSpawner : Singleton<UINodePanelSpawner>
{
    public UINodeConfigurationPanelManager panelManager;


    private void Start()
    {
        ClosePanel();
    }

    public void SpawnPanel(IGraphicInstance instanceWrapper)
    {
        panelManager.gameObject.SetActive(true);
        panelManager.SetGraphicInstance(instanceWrapper);
    }

    public void ClosePanel()
    {
        panelManager.gameObject.SetActive(false);
        panelManager.SetGraphicInstance(null);
    }
}
