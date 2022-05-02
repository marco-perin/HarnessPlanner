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
            LineData = new List<FullLineData>();
        }

        public IFullLineData PowerData { get => powerData; set => powerData = value as FullLineData; }
        //public IEnumerable<FullLineData> LineData { get => lineData; set => lineData = value.ToList(); }

        public IEnumerable<IFullLineData> LineData
        {
            get
            {
                //if (lineData != null)
                //    Debug.Log($"Got {string.Join("-", lineData.Select(l => "" + l.ConductorData.Awg))} from lineData");
                return lineData.Select(ld => ld as IFullLineData).AsEnumerable();

            }
            set
            {
                if (value != null)
                    Debug.Log($"Assigned {string.Join("-", (value.Select(ld => ld as FullLineData))?.ToList().Select(l => "" + l.ConductorData.Awg))} to lineData");
                lineData = (value?.Select(ld => ld as FullLineData))?.ToList();
            }
        }

        public void AddLineData(IFullLineData newData)
        {
            lineData.Add(newData as FullLineData);
        }

        public void ClearLineData()
        {
            powerData = new();
            Debug.Log("Clearing Line data");
            lineData.Clear();
        }

        public override string ToString()
        {
            var result = "";
            if (PowerData != null && PowerData.Current > 0)
                result += $"Power\n {PowerData.Current}A - {PowerData.ConductorData?.Awg ?? "NO"}awg\n";

            if (LineData.Count() > 0)
                result += $"Signals\n {LineData.Count()}";

            return result;

        }
    }
}

namespace Assets.CoreData.Interfaces
{
    public interface ILinkInfo
    {
        IFullLineData PowerData { get; set; }
        IEnumerable<IFullLineData> LineData { get; set; }
        void AddLineData(IFullLineData newData);
        void ClearLineData();
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