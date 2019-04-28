using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Anchor
{
    void Attach(GameObject attachedObject);
    void Detach(GameObject detatchedObject);
}

public abstract class AnchorBehavior : MonoBehaviour, Anchor
{
    public List<DraggableType> draggableTypes;

    public abstract void Attach(GameObject attachedObject);
    public abstract void Detach(GameObject detatchedObject);
}

public enum DraggableType {
    Culture, EmptyDish
};

public class Draggable : MonoBehaviour
{
    public DraggableType draggableType;

    CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 mouseDownPosition;
    Quaternion mouseDownRotation;
    Transform anchorPoint;
    bool dragging;
    bool attached;
    bool userCanInteract = true;
    public void LockUserInteraction()
    {
        userCanInteract = false;
        Debug.Log("Lock user interaction for " + gameObject);
    }

    public void UnlockUserInteraction()
    {
        userCanInteract = true;
        Debug.Log("Unlock user interaction for " + gameObject);
    }

    void OnMouseDown()
    {
        if (!userCanInteract)
        {
            return;
        }

        Debug.Log("Picked up " + gameObject);

        if (attached)
        {
            attached = false;
            anchorPoint.GetComponent<Anchor>().Detach(transform.gameObject);
            anchorPoint = null;
        }

        dragging = true;
        mouseDownPosition = transform.position;
        mouseDownRotation = transform.rotation;

        cameraController.RequestPanToAngle(cameraController.baseview, 1.0f);
    }

    void OnMouseDrag()
    {
        if (!dragging)
        {
            return;
        }

        if (!userCanInteract)
        {
            return;
        }

        float distanceFromScreen = Camera.main.WorldToScreenPoint(transform.position).z;

        if (anchorPoint != null)
        {

            transform.position = anchorPoint.position;
            transform.rotation = anchorPoint.rotation;
        }
        else
        {
            transform.rotation = mouseDownRotation;
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromScreen));
        }

        RaycastHit hit;
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 25.0f, layerMask))
        {
            AnchorBehavior behavior = hit.transform.GetComponent<AnchorBehavior>();
            if (behavior && behavior.draggableTypes.Contains(draggableType))
            {
                anchorPoint = hit.transform;
            }
        }
        else
        {
            anchorPoint = null;
        }
    }

    void OnMouseUp()
    {
        if (!dragging)
        {
            return;
        }

        if (!userCanInteract)
        {
            return;
        }

        dragging = false;

        if (anchorPoint)
        {
            Debug.Log("Attaching " + gameObject + " to anchor point " + anchorPoint);
            transform.position = anchorPoint.position;
            transform.rotation = anchorPoint.rotation;
            attached = true;
            anchorPoint.GetComponent<Anchor>().Attach(transform.gameObject);
        }
        else
        {
            Debug.Log("Returning " + gameObject + " to its initial position");
            transform.position = mouseDownPosition;
            transform.rotation = mouseDownRotation;
        }
    }
}
