using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WoldGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        World worldGen = (World)target;

        if (DrawDefaultInspector())
        {
            if (worldGen.autoUpdate)
            {
                worldGen.GenerateWorld();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            worldGen.GenerateWorld();
        }
    }
}
