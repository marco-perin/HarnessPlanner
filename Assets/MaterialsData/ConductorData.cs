using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.MaterialsData
{
    [Serializable]
    public class ConductorData
    {
        [SerializeField] private string awg;
        [SerializeField] private int cma;
        [SerializeField] private double area;
        [SerializeField] private double maxCurrent;

        public string Awg { get => awg; set => awg = value; }
        public int CMA { get => cma; set => cma = value; }
        public double Area { get => area; set => area = value; }
        public double MaxCurrent { get => maxCurrent; set => maxCurrent = value; }
    }
}
