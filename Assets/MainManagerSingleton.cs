using System.Collections;
using System.Collections.Generic;
using Assets.GraphicData.ScriptableObjects;
using UnityEngine;

public class MainManagerSingleton : Singleton<MainManagerSingleton>
{
    [SerializeField] private GraphicHarnessSettingsSO settings;

    public GraphicHarnessSettingsSO Settings { get => settings; }
}
