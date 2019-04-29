using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    GameManager gameManager;

    public void Awake()
    {
        gameManager = (GameManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Restart"))
        {
            gameManager.Restart();
        }

        if (GUILayout.Button("Win Game"))
        {
            gameManager.WinGame();
        }

        if (GUILayout.Button("Lose Game"))
        {
            gameManager.EndGame();
        }

        if (GUILayout.Button("Lose Game (Too Poor)"))
        {
            gameManager.EndGame("You can't afford any more petri dishes. Poor and destitute, the disease overtakes you without a fight.");
        }

    }
}
