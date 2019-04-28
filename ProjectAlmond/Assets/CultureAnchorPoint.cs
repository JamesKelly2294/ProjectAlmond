using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CultureAnchorPointEvent : UnityEvent<GameObject, Culture, CultureAnchorPoint> { }

public class CultureAnchorPoint : MonoBehaviour
{
    public CultureAnchorPointEvent onAttach;
    public Transform cameraAngle;

    public GameObject Culture { get; private set; }

    CameraController cameraController;

    public void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Attach(GameObject culture)
    {
        Culture = culture;

        onAttach.Invoke(culture, culture.GetComponent<Culture>(), this);

        if(cameraAngle)
        {
            cameraController.RequestPanToAngle(cameraAngle, 1.0f);
        }
    }
}
