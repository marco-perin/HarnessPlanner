using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using Assets.CoreData.Interfaces;
using System.Linq;
using System;

public class DataExportManagerSingleton : Singleton<DataExportManagerSingleton>
{
    public string exportFolder = "HarnessPlanner";

    public string ExportBasePath
    {
        get
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }

    public string ExportFullPath { get => Path.Combine(ExportBasePath, exportFolder); }

    public void ExportAll()
    {
        var conductorsLenghtsTask = ExportAggregatedLengthsAsync();
        var linksLenghtsTask = ExportLinksLengthsAsync();

        var AllTask = Task.WhenAll(conductorsLenghtsTask, linksLenghtsTask);

        StartCoroutine(GeneralUtils.CoroutineTask(AllTask, (returns) =>
         {
             Debug.Log("Saved : " + string.Join(',', returns));
         }));
    }
    //public void ExportLenghts()
    //{
    //    var task = ExportAggregatedLengthsAsync();
    //    StartCoroutine(CoroutineTask(task, (result) =>
    //    {
    //        Debug.Log($"Saved: {result}");
    //    }));

    //}

    public async Task<bool> ExportLinksLengthsAsync()
    {
        var fileName = "Links Lenghts";

        List<string> lines = new();

        lines.Add(FullPathData.GetTableRowHeader(','));


        var links = MainCalculatorSingleton.Instance.fullPathsConnections.
            Select(fp => fp.GetTableRowString(','));

        lines.AddRange(links);


        await WriteExportFile(fileName, lines);

        return true;
    }

    public async Task<bool> ExportAggregatedLengthsAsync()
    {
        string fileName = "AggregatedLenghts";

        List<string> lines = new();

        lines.Add(IConductorData.GetTableRowHeader(',') + ",Total Lenght");

        var conductorsData = CanvasUtilsManager
            .GetAllGraphicInstanceWithBaseType<INodeLinkBase>()
            .Select(gi => (gi.BaseWrapped as INodeLinkBase))
            .Where(bt => bt.LinkInfo != null && bt.LinkInfo.IsValid)
            .SelectMany(bt =>
                bt.LinkInfo.LineData
                    .Append(bt.LinkInfo.PowerData)
                    .Where(ld => ld != null && ld.ConductorData != null)
                    .Select(ld => new { bt.Length, LineData = ld })
                )
            //.Select(cd => cd)
            //.GroupBy(
            //cds => cds.LineData.ConductorData,
            //cds => new { cds.Length, cds.LineData.ConductorData },
            //(key, value) => new { key, value }
            //)
            //.Select(group => new { awg = group.key, data=group })
            .Where(cds => cds.LineData.ConductorData != null);

        var aggData = conductorsData
            .GroupBy(
            cds => cds.LineData.ConductorData.Awg,
            cds => new { cds.Length, cds.LineData.ConductorData },
            (awg, lenghtData) => new
            {
                Lenght = lenghtData.Sum(ld => ld.Length),
                Data = lenghtData.First().ConductorData
            });

        var linksRows = aggData
        .Where(ld => ld.Lenght > 0)
        .OrderBy(ld => float.Parse(ld.Data.Awg))
        .Select(lengthData => lengthData.Data.GetTableRowString(',') + $",{lengthData.Lenght}");


        lines.AddRange(linksRows);

        await WriteExportFile(fileName, lines);

        return true;
    }

    private async Task WriteExportFile(string fileName, List<string> lines)
    {
#if UNITY_EDITOR
        var fullPath = Path.Combine(ExportFullPath,"Dev", fileName + ".csv");
#else
        var fullPath = Path.Combine(ExportFullPath, fileName + ".csv");
#endif
        string dirPath = fullPath[..fullPath.LastIndexOfAny("/\\".ToCharArray())];

        Directory.CreateDirectory(dirPath);

        await File.WriteAllLinesAsync(fullPath, lines);
    }



}
