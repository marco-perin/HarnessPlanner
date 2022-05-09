using Assets.CoreData.Interfaces;
using Assets.CoreData.ScriptableObjects;
using Assets.CoreData.Types;
using Assets.GraphicData.Interfaces;
using UnityEngine;

public class UINodePanelSpawner : Singleton<UINodePanelSpawner>
{
    public UINodeConfigurationPanelManager panelManager;
    public UINodeConfigurationPanelManager linkPanelManager;
    public UINodeConfigurationPanelManager connectorPanelManager;


    private void Start()
    {
        ClosePanels();
    }

    public void SpawnPanel(IGraphicInstance instanceWrapper)
    {

        panelManager.gameObject.SetActive(false);
        linkPanelManager.gameObject.SetActive(false);
        connectorPanelManager.gameObject.SetActive(false);

        switch (instanceWrapper.BaseWrapped)
        {
            case NodeLinkBase:
                linkPanelManager.gameObject.SetActive(true);
                linkPanelManager.SetGraphicInstance(instanceWrapper);
                break;
            case IBaseNodeWithPinnedSO:
                panelManager.gameObject.SetActive(true);
                panelManager.SetGraphicInstance(instanceWrapper);
                break;
        }
    }

    public void SpawnConnectorPanel(IGraphicInstance instanceWrapper)
    {
        connectorPanelManager.gameObject.SetActive(true);
        connectorPanelManager.SetConnectorData(instanceWrapper);
    }

    public void ClosePanels()
    {
        panelManager.gameObject.SetActive(false);
        linkPanelManager.gameObject.SetActive(false);
        connectorPanelManager.gameObject.SetActive(false);
        panelManager.SetGraphicInstance(null);
        linkPanelManager.SetGraphicInstance(null);
        connectorPanelManager.SetGraphicInstance(null);
    }

    public void ClosePanel(UINodeConfigurationPanelManager panelObject)
    {

        panelObject.gameObject.SetActive(false);
        panelObject.SetGraphicInstance(null);
    }
}
