using System.Collections.Generic;
using UnityEngine;

public interface IPinConfiguration
{
    int PinCount { get; }
    IEnumerable<Vector3> PinPositions { get; }
}




