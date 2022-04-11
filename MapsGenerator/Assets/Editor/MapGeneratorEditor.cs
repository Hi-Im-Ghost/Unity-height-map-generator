using System.Collections;
using UnityEngine;
using UnityEditor;

// Customowy skrypt UI edytora dla skryptu MapGenerator
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Kastuje odniesienie obecnie używanego skryptu do MapGenerator
        MapGenerator mapGenerator = (MapGenerator)target;

        // Jeżeli jakakolwiek wartość w inspektorze została zmieniona
        if (DrawDefaultInspector())
        {
            if (mapGenerator.autoUpdate)
            {
                // Rozpoczyna procedurę generowania mapy
                mapGenerator.GenerateMap();
            }
        }

        // Tworzy w UI skryptu przycisk, który po naciśnięciu wykona to co w if{}
        if (GUILayout.Button("Generate"))
        {
            // Rozpoczyna procedurę generowania mapy
            mapGenerator.GenerateMap();
        }
    }
}
