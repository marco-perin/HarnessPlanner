using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PinConfigBase : IPinConfiguration
{
    public virtual int PinCount { get => PinDataArray.Count(); }

    [SerializeField]
    private List<PinData> pinNames = new();

    public virtual IEnumerable<IPinData> PinDataArray { get => pinNames; set => pinNames = value.Select(ipd => ipd as PinData).ToList(); }
}

[Serializable]
public class PinData : IPinData
{
    [SerializeField] private int id;
    [SerializeField] private string name;
    [TextArea]
    [SerializeField] private string description;

    public int Id { get => id; set => id = value; }
    public string Name => name; public string Description => description;
}




