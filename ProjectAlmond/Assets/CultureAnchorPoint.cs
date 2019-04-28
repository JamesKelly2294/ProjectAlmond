using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CultureAnchorPoint : MonoBehaviour
{
    public UnityEvent onAttach;
    public Transform cameraAngle;

    CameraController cameraController;

    public void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Attach()
    {
        onAttach.Invoke();

        if(cameraAngle)
        {
            cameraController.RequestPanToAngle(cameraAngle, 1.0f);
        }
    }
}
