using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CultureRenderer))]
public class CultureRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CultureRenderer cultureRenderer = (CultureRenderer)target;
        if (GUILayout.Button("Recalculate Culture"))
        {
            cultureRenderer.RecalculateCulture();
        }
    }
}
