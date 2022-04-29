using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.MaterialsData.Editor
{

    public class AvailableConductorDataEditor : EditorWindow
    {


        private string awg_Field = "awg";
        private string cma_Field = "cma";
        private string area_Field = "Area";
        private string maxCurrent_Field = "maxcurrent";
        public bool isFirstLineHeader = true;

        [MenuItem("HarnessPlanner/LoadAvailableConductorsCSV")]
        static void InitWindow()
        {
            GetWindow(typeof(AvailableConductorDataEditor));

        }

        AvailableConductorData data = null;

        void OnGUI()
        {

            data = EditorGUILayout.ObjectField("", data, typeof(AvailableConductorData), true) as AvailableConductorData;

            if (data == null)
                return;


            EditorGUILayout.BeginHorizontal();
            awg_Field = EditorGUILayout.TextField("AWG Field", awg_Field);
            cma_Field = EditorGUILayout.TextField("CMA field", cma_Field);
            area_Field = EditorGUILayout.TextField("Area field", area_Field);
            maxCurrent_Field = EditorGUILayout.TextField("MaxCurrent Field", maxCurrent_Field);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            foreach (var c in data.availableConductors)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(c.Awg);
                EditorGUILayout.LabelField("" + c.CMA);
                EditorGUILayout.LabelField("" + c.Area);
                EditorGUILayout.LabelField("" + c.MaxCurrent);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();

            isFirstLineHeader = EditorGUILayout.Toggle("First Line is Header", isFirstLineHeader);

            if (GUILayout.Button("Load From CSV"))
            {
                Apply(data, isFirstLineHeader);
            }
        }

        void Apply(AvailableConductorData conductorData, bool headerFirstLine)
        {
            if (conductorData == null)
            {
                EditorUtility.DisplayDialog("Select Texture", "You must select an AvailableConductorData first!", "OK");
                return;
            }

            string path = EditorUtility.OpenFilePanel("Load CSV Data", "", "csv");
            if (path.Length == 0) return;


            conductorData.availableConductors.Clear();
            using var reader = new StreamReader(File.OpenRead(path));

            Dictionary<string, int> colsIndexes = new();
            int lineNr = 0;

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (lineNr == 0 && headerFirstLine)
                {

                    for (int i = 0; i < values.Length; i++)
                    {
                        colsIndexes[values[i].ToLower()] = i;
                    }

                    lineNr++;
                    continue;
                }

                conductorData.availableConductors.Add(new()
                {
                    Awg = values[colsIndexes[awg_Field.ToLower()]],
                    CMA = int.TryParse(values[colsIndexes[cma_Field.ToLower()]], out var res) ? res : 0,
                    Area = double.TryParse(values[colsIndexes[area_Field.ToLower()]], out var resArea) ? resArea : 0,
                    MaxCurrent = double.TryParse(values[colsIndexes[maxCurrent_Field.ToLower()]], out var resMC) ? resMC : 0,
                });

                lineNr++;
            }

        }
    }
}