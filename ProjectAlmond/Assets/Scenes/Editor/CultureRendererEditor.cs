using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CultureRenderer))]
public class CultureRendererEditor : Editor
{
    public float growthValue = 0.5f;
    CultureRenderer cultureRenderer;

    public void Awake()
    {
        cultureRenderer = (CultureRenderer)target;
        growthValue = cultureRenderer.Growth;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Recalculate Culture"))
        {
            cultureRenderer.RecalculateCulture();
        }

        GUILayout.Label("Growth");
        float newGrowthValue = GUILayout.HorizontalSlider(growthValue, 0.0f, 1.0f, null);
        if(newGrowthValue != growthValue)
        {
            growthValue = newGrowthValue;
            cultureRenderer.Growth = growthValue;
        }
    }
    
}
