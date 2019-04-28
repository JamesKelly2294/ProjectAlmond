using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DishReceptical))]
public class DishRecepticalEditor : Editor
{
    DishReceptical dishReceptical;

    public void Awake()
    {
        dishReceptical = (DishReceptical)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Start"))
        {
            dishReceptical.startAnimating();
        }

        if (GUILayout.Button("End"))
        {
            dishReceptical.stopAnimating();
        }
    }
}
