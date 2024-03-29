﻿using System;
using System.Collections.Generic;
using Assets.CoreData.Interfaces;
using UnityEngine;

public interface IPinConfiguration
{
    int PinCount { get; }
    IEnumerable<IPinData> PinDataArray { get; set; }
}

public interface IPinData : IWithId, IEquatable<IPinData>
{
    int PinNumber { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    PinTypeEnum PinType { get; set; }

}

[Serializable]
public enum PinTypeEnum
{
    Generic,
    Power,
    Ground,
    Signal
}




