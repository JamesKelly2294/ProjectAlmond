using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    CameraController cameraController;

    public void Awake()
    {
        cameraController = (CameraController)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Overview"))
        {
            cameraController.PanToAngle(cameraController.overview, 1.0f);
        }

        if (GUILayout.Button("Baseview"))
        {
            cameraController.PanToAngle(cameraController.baseview, 1.0f);
        }

        if (GUILayout.Button("Petri Dishes"))
        {
            cameraController.PanToAngle(cameraController.petriDishes, 1.0f);
        }

    }
    
}
