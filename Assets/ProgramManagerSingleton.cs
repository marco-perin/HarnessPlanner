using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ProgramManagerSingleton : Singleton<ProgramManagerSingleton>
{
    [SerializeField]
    private GraphicHarnessSettingsSO harnessSettingsSO;
    private HarnessSettingSOLoaded harnessSettingsSOLoaded;

    public int loadedRefCount = 0;
    public HarnessSettingSOLoaded HarnessSettingsSO { get => harnessSettingsSOLoaded; }
    public GraphicHarnessSettingsSO HarnessSettingsSOAddressables { get => harnessSettingsSO; }

    public bool HasLoaded => hasLoaded;
    [SerializeField]
    private bool hasLoaded = false;

    private void Start()
    {
        harnessSettingsSOLoaded = new HarnessSettingSOLoaded
        {
            NodesPlaceHeight = harnessSettingsSO.NodesPlaceHeight,
            ConnectionsPlaceHeight = harnessSettingsSO.ConnectionsPlaceHeight,
            SnapGridSize = harnessSettingsSO.SnapGridSize
        };


        StartCoroutine(LoadAssets());
    }

    private IEnumerator LoadAssets()
    {

        var sourceload = harnessSettingsSO.DefaultSourcePrefab.LoadAssetAsync();
        var sinkload = harnessSettingsSO.DefaultSinkPrefab.LoadAssetAsync();
        var nodeload = harnessSettingsSO.DefaultNodePrefab.LoadAssetAsync();
        var linkload = harnessSettingsSO.DefaultLinkPrefab.LoadAssetAsync();

        var operations = new List<AsyncOperationHandle>
        {
            sourceload,
            sinkload,
            nodeload,
            linkload
        };

        while (loadedRefCount < 4)
        {
            loadedRefCount = operations.Where(o => o.IsDone).Count();
            if (loadedRefCount < 4)
                yield return operations.First(o => !o.IsDone);
            else
                break;
        }

        harnessSettingsSOLoaded.DefaultSourcePrefab = sourceload.Result;
        harnessSettingsSOLoaded.DefaultSinkPrefab = sinkload.Result;
        harnessSettingsSOLoaded.DefaultNodePrefab = nodeload.Result;
        harnessSettingsSOLoaded.DefaultLinkPrefab = linkload.Result;

        hasLoaded = true;

        Debug.Log("Loaded all assets");
    }

    //public async Task LoadAssetsAsync()
    //{
    //    var sourceload = harnessSettingsSO.DefaultSourcePrefab.LoadAssetAsync();
    //    var sinkload = harnessSettingsSO.DefaultSinkPrefab.LoadAssetAsync();
    //    var nodeload = harnessSettingsSO.DefaultNodePrefab.LoadAssetAsync();
    //    var linkload = harnessSettingsSO.DefaultLinkPrefab.LoadAssetAsync();

    //    var operations = new List<AsyncOperationHandle>
    //    {
    //        sourceload,
    //        sinkload,
    //        nodeload,
    //        linkload
    //    };

    //    while (!HasLoaded)
    //    {
    //        loadedRefCount = operations.Where(o => o.IsDone).Count();
    //    }
    //}

    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
