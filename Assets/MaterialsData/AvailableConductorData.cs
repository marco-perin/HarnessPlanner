using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MaterialsData
{
    [CreateAssetMenu(menuName = "MaterialsSOs/AvailableConductorData", fileName = "AvailableConductorsData")]
    public class AvailableConductorData : ScriptableObject
    {
        public List<ConductorData> availableConductors;
    }
}