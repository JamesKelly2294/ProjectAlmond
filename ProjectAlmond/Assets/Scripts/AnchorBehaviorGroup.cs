using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorBehaviorGroup : MonoBehaviour
{
    public Transform cameraAngle;

    CameraController cameraController;
    List<AnchorBehavior> anchors;

    public void Awake()
    {
        anchors = new List<AnchorBehavior>();
    }

    public void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void RegisterAnchor(AnchorBehavior anchor)
    {
        anchors.Add(anchor);
    }

    public void UnregisterAnchor(AnchorBehavior anchor)
    {
        anchors.Remove(anchor);
    }

    public void AnchorAttached(AnchorBehavior attachedObject)
    {
        if (!anchors.Contains(attachedObject))
        {
            return;
        }

        // slow af i dont care leeeel
        bool allAnchorsAttached = true;
        foreach (AnchorBehavior anchor in anchors)
        {
            allAnchorsAttached &= anchor.Occupied;
        }

        if (allAnchorsAttached && cameraAngle)
        {
            cameraController.RequestPanToAngle(cameraAngle, 1.0f);
        }
    }

    public void AnchorDetached(AnchorBehavior detatchedObject)
    {

    }
}
