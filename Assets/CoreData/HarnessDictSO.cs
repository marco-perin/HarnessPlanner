using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "SO/Base/HarnessDict")]
public class HarnessDictSO //: ScriptableObject
{
    //public List<HarnessDictEntry> entries;
    public Dictionary<string, HarnessDictEntry> harnessIdDictEntryDict;
    public Dictionary<string, HarnessDictEntry> parentsIdDictEntryDict;
    public Dictionary<string, HarnessDictEntry> childsIdDictEntryDict;
}

[Serializable]
public class HarnessDictEntry
{
    public ConnectorData connectorData;
}
