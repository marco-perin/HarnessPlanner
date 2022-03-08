using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Connector", menuName = "SO/Connector")]
public class ConnectorSO : ScriptableObject
{
    public string modelName;

    public IPinConfiguration PinConfig => useParametricConfig ? pinConfigParametric : pinConfigList;

    public bool useParametricConfig;

    [SerializeField]
    private PinConfigBase pinConfigList;

    [SerializeField]
    private PinConfigParametric pinConfigParametric;

}

[Serializable]
public class PinConfigParametric : PinConfigBase
{
    [SerializeField]
    private PinParametricUniform pinParametricUniform;
    public override int PinCount { get => pinParametricUniform.PinCount; }
    public override IEnumerable<Vector3> PinPositions { get => pinParametricUniform.PinPositions; }
}

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


[Serializable]
public class PinConfigBase : IPinConfiguration
{
    public virtual int PinCount { get => PinPositions.Count(); }

    [SerializeField]
    private List<Vector3> pinPositions;

    public virtual IEnumerable<Vector3> PinPositions { get => pinPositions; set => pinPositions = value.ToList(); }
}

public interface IPinConfiguration
{
    int PinCount { get; }
    IEnumerable<Vector3> PinPositions { get; }
}




