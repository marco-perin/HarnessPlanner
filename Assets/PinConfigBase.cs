﻿using System;
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
public class PinData : IPinData, IEquatable<PinData>
{
    [SerializeField] private string name;
    [SerializeField] private int pinNumber;
    [SerializeField] private string id;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private PinTypeEnum pinType;

    public PinData()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id
    {
        get
        {
            if (id == "PIN")
                return "" + pinNumber;

            return id;
        }
        set => id = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public int PinNumber { get => pinNumber; set => pinNumber = value; }

    public PinTypeEnum PinType { get => pinType; set => pinType = value; }

    public bool Equals(IPinData other)
    {
        if (other is null)
        {
            return false;
        }

        // Optimization for a common success case.
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != other.GetType())
        {
            return false;
        }

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return this.Equals((PinData)obj);
        }
    }

    public bool Equals(PinData other)
    {
        return Equals(other as IPinData);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Description, PinNumber);
    }

    //public override int GetHashCode() => (X, Y).GetHashCode();

    public static bool operator ==(PinData lhs, PinData rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }

            // Only the left side is null.
            return false;
        }
        // Equals handles case of null on right side.
        return lhs.Equals(rhs);
    }

    public static bool operator !=(PinData lhs, PinData rhs) => !(lhs == rhs);
}




