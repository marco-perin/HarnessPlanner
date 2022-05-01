using System;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using UnityEngine;

public interface IPinConfiguration
{
    int PinCount { get; }
    IEnumerable<IPinData> PinDataArray { get; }
}

public interface IPinData : IWithId, IEquatable<IPinData>
{
    int PinNumber { get; set; }
    string Name { get; }
    string Description { get; }
    PinType PinType { get; set; }

}
public enum PinType
{
    Generic,
    Power,
    Ground,
    Signal
}




