using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.Types
{
    [Serializable]
    public class ConductorData : IConductorData
    {
        [SerializeField] private string awg;
        [SerializeField] private int cma;
        [SerializeField] private double area;
        [SerializeField] private double maxCurrent;

        public string Awg { get => awg; set => awg = value; }
        public int CMA { get => cma; set => cma = value; }
        public double Area { get => area; set => area = value; }
        public double MaxCurrent { get => maxCurrent; set => maxCurrent = value; }

        public string GetTableRowString(char separator)
        {
            return string.Join(separator, new string[] { Awg, CMA.ToString(), Area.ToString() });
        }
    }
}
