using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PinParametricUniform
{
    public List<PinUniformConfigSide> sidesConfig;
    public int PinCount { get => sidesConfig.Select(sc => sc.pinCount).Sum(); }

    public IEnumerable<Vector3> PinPositions
    {
        get => sidesConfig.Select(sidesConfig => sidesConfig.PinPositions).SelectMany(p => p);
    }
}




