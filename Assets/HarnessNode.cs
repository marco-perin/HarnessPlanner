using Assets.CoreData.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class HarnessNode<T> where T : IConnectible
{
    public string id;
    public Vector2 position;
    public List<T> connectibles;
}




