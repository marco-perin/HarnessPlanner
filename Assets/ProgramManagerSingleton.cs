using System.Collections;
using System.Collections.Generic;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;

public class ProgramManagerSingleton : Singleton<ProgramManagerSingleton>
{
    [SerializeField]
    private GraphicHarnessSettingsSO harnessSettingsSO;

    public GraphicHarnessSettingsSO HarnessSettingsSO { get => harnessSettingsSO; }


    public void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
