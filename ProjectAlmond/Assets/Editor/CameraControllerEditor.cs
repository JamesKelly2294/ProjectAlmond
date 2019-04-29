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
        
        if (GUILayout.Button("Title"))
        {
            cameraController.RequestPanToAngle(cameraController.title, 1.0f);
        }

        if (GUILayout.Button("Overview"))
        {
            cameraController.RequestPanToAngle(cameraController.overview, 1.0f);
        }

        if (GUILayout.Button("Baseview"))
        {
            cameraController.RequestPanToAngle(cameraController.baseview, 1.0f);
        }

        if (GUILayout.Button("Petri Dishes"))
        {
            cameraController.RequestPanToAngle(cameraController.petriDishes, 1.0f);
        }

        if (GUILayout.Button("Genimagic"))
        {
            cameraController.RequestPanToAngle(cameraController.genimagic, 1.0f);
        }

        if (GUILayout.Button("Check-o-matic"))
        {
            cameraController.RequestPanToAngle(cameraController.checkomatic, 1.0f);
        }

        if (GUILayout.Button("Amalgamizer"))
        {
            cameraController.RequestPanToAngle(cameraController.amalgamizer, 1.0f);
        }

        if (GUILayout.Button("Resources"))
        {
            cameraController.RequestPanToAngle(cameraController.resources, 1.0f);
        }
    }
}
