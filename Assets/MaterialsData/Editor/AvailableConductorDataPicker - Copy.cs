using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.CoreData.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Assets.MaterialsData.Editor
{

    public class ConnectorDataEditor : EditorWindow
    {


        private string pinNr_Field = "PinNr";
        private string id_Field = "Id";
        private string name_Field = "Name";
        private string description_Field = "Description";
        public bool isFirstLineHeader = true;

        [MenuItem("HarnessPlanner/EditConnectorDataCSV")]
        static void InitWindow()
        {
            GetWindow(typeof(ConnectorDataEditor));

        }

        ConnectorNodeBaseSO data = null;
        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {

            data = EditorGUILayout.ObjectField("", data, typeof(ConnectorNodeBaseSO), true) as ConnectorNodeBaseSO;

            if (data == null)
                return;

            var array = data.PinConfiguration.PinDataArray;

            EditorGUILayout.BeginHorizontal();
            pinNr_Field = EditorGUILayout.TextField("Pin Nr Field", pinNr_Field);
            id_Field = EditorGUILayout.TextField("Id field", id_Field);
            name_Field = EditorGUILayout.TextField("Name field", name_Field);
            description_Field = EditorGUILayout.TextField("Descr. Field", description_Field);
            EditorGUILayout.EndHorizontal();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginVertical();
            foreach (var c in array)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("" + c.PinNumber);
                EditorGUILayout.LabelField("" + c.Id);
                EditorGUILayout.LabelField("" + c.Name);
                EditorGUILayout.LabelField("" + c.Description);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Separator();

            isFirstLineHeader = EditorGUILayout.Toggle("First Line is Header", isFirstLineHeader);

            if (GUILayout.Button("Load From CSV"))
            {
                Apply(data, isFirstLineHeader);
            }
        }

        void Apply(ConnectorNodeBaseSO conductorData, bool headerFirstLine)
        {
            if (conductorData == null)
            {
                EditorUtility.DisplayDialog("Select Texture", "You must select an AvailableConductorData first!", "OK");
                return;
            }

            string path = EditorUtility.OpenFilePanel("Load CSV Data", "", "csv");
            if (path.Length == 0) return;

            List<PinData> pins = new();

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

                PinData newLine = new()
                {
                    PinNumber = int.TryParse(values[colsIndexes[pinNr_Field.ToLower()]], out var pinNr) ? pinNr : -1,
                    Id = values[colsIndexes[id_Field.ToLower()]],
                    Name = values[colsIndexes[name_Field.ToLower()]],
                    Description = values[colsIndexes[description_Field.ToLower()]],
                };

                if (!LineValid(newLine)) continue;

                pins.Add(newLine);

                lineNr++;
            }

            conductorData.PinConfiguration.PinDataArray = pins;
        }

        private bool LineValid(PinData data)
        {
            bool ret = true;

            ret &= data.Id != "-";
            ret &= data.Name != "";

            return ret;
        }
    }
}