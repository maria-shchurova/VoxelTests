using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimplexNoiseDisplay))]
public class NoiseTexDisplayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SimplexNoiseDisplay display = (SimplexNoiseDisplay)target;


        if (GUILayout.Button("display"))
        {
            display.Display();
        }
    }
}
