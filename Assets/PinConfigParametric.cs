using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PinConfigParametric : PinConfigBase
{
    [SerializeField]
    private PinParametricUniform pinParametricUniform;

    //public override int PinCount { get => pinParametricUniform.PinCount; }
    //public override IEnumerable<string> PinPositions { get => pinParametricUniform.PinPositions; }
}




