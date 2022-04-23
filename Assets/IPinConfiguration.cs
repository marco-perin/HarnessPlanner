using System.Collections.Generic;
using UnityEngine;

public interface IPinConfiguration
{
    int PinCount { get; }
    IEnumerable<IPinData> PinDataArray { get; }
}

public interface IPinData
{
    int Id { get; set; }
    string Name { get; }
    string Description { get; }
}




