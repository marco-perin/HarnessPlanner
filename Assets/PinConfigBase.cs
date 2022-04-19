using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PinConfigBase : IPinConfiguration
{
    public virtual int PinCount { get => PinPositions.Count(); }

    [SerializeField]
    private List<Vector3> pinPositions;

    public virtual IEnumerable<Vector3> PinPositions { get => pinPositions; set => pinPositions = value.ToList(); }
}




