using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PinConfigBase : IPinConfiguration
{
    public virtual int PinCount { get => PinNames.Count(); }

    [SerializeField]
    private List<string> pinNames = new List<string>();

    public virtual IEnumerable<string> PinNames { get => pinNames; set => pinNames = value.ToList(); }
}




