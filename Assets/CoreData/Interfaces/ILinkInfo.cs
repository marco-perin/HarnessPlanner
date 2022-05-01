using Assets.CoreData.Interfaces;
using System;
// TODO: Remove this and use only core data
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class LinkInfo : ILinkInfo
    {

        [SerializeField] private FullLineData powerData;
        [SerializeField] private List<FullLineData> lineData;

        public LinkInfo()
        {
            lineData = new List<FullLineData>();
        }

        public IFullLineData PowerData { get => powerData; set => powerData = value as FullLineData; }
        public IEnumerable<IFullLineData> LineData { get => lineData; set => lineData = (value as IEnumerable<FullLineData>).ToList(); }

        public override string ToString()
        {
            return $"Power: {PowerData.Current}A - {PowerData.ConductorData.Awg}awg";
        }
    }
}

namespace Assets.CoreData.Interfaces
{
    public interface ILinkInfo
    {
        IFullLineData PowerData { get; set; }
        IEnumerable<IFullLineData> LineData { get; set; }
    }

    public interface IFullLineData
    {
        IConductorData ConductorData { get; set; }
        double Current { get; set; }
    }

    public interface IConductorData
    {
        public string Awg { get; set; }
        public int CMA { get; set; }
        public double Area { get; set; }
        public double MaxCurrent { get; set; }
    }
}