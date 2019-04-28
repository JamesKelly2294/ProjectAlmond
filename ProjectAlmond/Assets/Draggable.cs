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

    public bool Occupied { get; set; }
    public List<DraggableType> draggableTypes;

    public virtual void Attach(GameObject attachedObject)
    {
        if (Occupied)
        {
            return;
        }

        Occupied = true;
    }

    public virtual void Detach(GameObject detatchedObject)
    {
        if (!Occupied)
        {
            return;
        }

        Occupied = false;
    }
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
    AnchorBehavior anchorPoint;
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
        if(!enabled)
        {
            return;
        }

        if (!userCanInteract)
        {
            return;
        }

        Debug.Log("Picked up " + gameObject);

        DetachFromAnchor();

        dragging = true;
        mouseDownPosition = transform.position;
        mouseDownRotation = transform.rotation;

        cameraController.RequestPanToAngle(cameraController.baseview, 1.0f);
    }

    void OnMouseDrag()
    {
        if (!enabled)
        {
            return;
        }

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

            transform.position = anchorPoint.transform.position;
            transform.rotation = anchorPoint.transform.rotation;
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
                anchorPoint = behavior;
            }
        }
        else
        {
            anchorPoint = null;
        }
    }

    void OnMouseUp()
    {
        if (!enabled)
        {
            return;
        }

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
            AttachToAnchor(anchorPoint.GetComponent<AnchorBehavior>());
        }
        else
        {
            Debug.Log("Returning " + gameObject + " to its initial position");
            transform.position = mouseDownPosition;
            transform.rotation = mouseDownRotation;
        }
    }

    public void DetachFromAnchor()
    {
        if (attached)
        {
            attached = false;
            anchorPoint.GetComponent<AnchorBehavior>().Detach(transform.gameObject);
            anchorPoint = null;
        }
    }

    public void AttachToAnchor(AnchorBehavior anchor)
    {
        Debug.Log("Attaching " + gameObject + " to anchor point " + anchor.gameObject);
        transform.position = anchor.transform.position;
        transform.rotation = anchor.transform.rotation;
        attached = true;
        anchor.Attach(transform.gameObject);
    }
}
