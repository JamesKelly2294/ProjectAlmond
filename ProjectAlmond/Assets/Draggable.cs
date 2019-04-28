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

        Debug.Log(attachedObject + " attached to " + this);
        Occupied = true;
    }

    public virtual void Detach(GameObject detachedObject)
    {
        if (!Occupied)
        {
            return;
        }

        Debug.Log(detachedObject + " detached from " + this);
        Occupied = false;
    }
}

public enum DraggableType {
    Culture, EmptyDish
};

public class Draggable : MonoBehaviour
{
    public DraggableType draggableType;
    public AnchorBehavior currentAnchor;

    CameraController cameraController;
    AnchorBehavior candidateAnchor;
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
    bool dragging;
    bool attached;
    bool userCanInteract = true;
    AnchorBehavior abandondedAnchor;
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

        if (candidateAnchor != null)
        {

            transform.position = candidateAnchor.transform.position;
            transform.rotation = candidateAnchor.transform.rotation;
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
            if (behavior && behavior.draggableTypes.Contains(draggableType) && !behavior.Occupied)
            {
                candidateAnchor = behavior;
            }
        }
        else
        {
            candidateAnchor = null;
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

        if (candidateAnchor)
        {
            AttachToAnchor(candidateAnchor);
        }
        else
        {
            Debug.Log("Returning " + gameObject + " to its initial position");
            AttachToAnchor(abandondedAnchor);
            transform.position = mouseDownPosition;
            transform.rotation = mouseDownRotation;
        }
    }

    public void DetachFromAnchor()
    {
        if (attached)
        {
            attached = false;
            currentAnchor.Detach(transform.gameObject);
            abandondedAnchor = currentAnchor;
            currentAnchor = null;
        }
    }

    public void AttachToAnchor(AnchorBehavior anchor)
    {
        abandondedAnchor = null;
        Debug.Log("Attaching " + gameObject + " to anchor point " + anchor.gameObject);
        currentAnchor = anchor;
        transform.position = anchor.transform.position;
        transform.rotation = anchor.transform.rotation;
        attached = true;
        anchor.Attach(transform.gameObject);
    }
}
