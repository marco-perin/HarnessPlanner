using System.Collections.Generic;
using UnityEngine;

public interface IPinConfiguration
{
    int PinCount { get; }
    IEnumerable<string> PinNames { get; }
}




