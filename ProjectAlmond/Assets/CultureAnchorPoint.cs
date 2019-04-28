using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CultureAnchorPointEvent : UnityEvent<GameObject, Culture, CultureAnchorPoint> { }

public class CultureAnchorPoint : AnchorBehavior
{
    public CultureAnchorPointEvent onAttach;
    public CultureAnchorPointEvent onDetach;
    public Transform cameraAngle;

    public GameObject Culture { get; private set; }

    CameraController cameraController;

    public void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public override void Attach(GameObject attachedObject)
    {
        Culture c = attachedObject.GetComponent<Culture>();

        if(c == null)
        {
            return;
        }

        Culture = attachedObject;

        onAttach.Invoke(attachedObject, c, this);

        if(cameraAngle)
        {
            cameraController.RequestPanToAngle(cameraAngle, 1.0f);
        }
    }

    public override void Detach(GameObject attachedObject)
    {
        Culture c = attachedObject.GetComponent<Culture>();

        if (c == null)
        {
            return;
        }

        Culture = null;

        onDetach.Invoke(attachedObject, c, this);
    }
}
