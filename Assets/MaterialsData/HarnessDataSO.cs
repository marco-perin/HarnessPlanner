using System.Collections;
using UnityEngine;

namespace Assets.MaterialsData
{
    [CreateAssetMenu(menuName = "MaterialsSOs/HarnessDataSO", fileName = "HarnessDataSO")]
    public class HarnessDataSO : ScriptableObject
    {
        public AvailableConductorData availableConductorsData;
    }
}