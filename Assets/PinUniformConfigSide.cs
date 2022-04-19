using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PinUniformConfigSide
{
    public int pinCount;
    public float pinSpacing;
    public Vector3 pinAxis;
    public Vector3 axisOffset;

    public IEnumerable<Vector3> PinPositions
    {
        get => Enumerable.Range(0, pinCount).Select(p => (p - (float)(pinCount -1 ) / 2f) * pinSpacing * pinAxis + axisOffset);
    }
}




