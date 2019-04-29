using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Anchor
{
    void Attach(GameObject attachedObject);
    void Detach(GameObject detatchedObject);
    void HoverDidChange(GameObject hoveredObject, bool isHovering);
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

    public virtual void HoverDidChange(GameObject hoveredObject, bool isHovering) {
        Debug.Log(hoveredObject + " hovering? " + isHovering + " over " + this);
    }
}

public enum DraggableType {
    Culture, EmptyDish, PluckedReagent, Disease
};

public class Draggable : MonoBehaviour
{
    public DraggableType draggableType;
    public AnchorBehavior Anchor { get; private set; }

    public Object Data { get; set; } // arbitrary data a draggable can have

    CameraController cameraController;
    AnchorBehavior candidateAnchor;
    // Start is called before the first frame update
    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            if(Input.GetMouseButton(0))
            {
                Drag();
            } else
            {
                EndDrag();
            }
        }
    }

    Vector3 mouseDownPosition;
    Quaternion mouseDownRotation;
    bool dragging;
    bool attached;
    bool userCanInteract = true;
    AnchorBehavior abandondedAnchor;
    public bool DiesOnRelease { get; set; }
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
        BeginDrag();
    }

    public void RequestBeginDrag()
    {
       BeginDrag();
    }

    void BeginDrag()
    {
        if (!enabled)
        {
            return;
        }

        if (!userCanInteract)
        {
            return;
        }

        GameManager.Instance.RequestPlayDishPickUpSound();

        Debug.Log("Picked up " + gameObject);

        DetachFromAnchor();

        dragging = true;
        mouseDownPosition = transform.position;
        mouseDownRotation = transform.rotation;

        cameraController.RequestPanToAngle(cameraController.baseview, 1.0f);
    }

    private AnchorBehavior hoverTarget; 

    void Drag()
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
            if (candidateAnchor != hoverTarget) {
                if (hoverTarget != null) {
                    hoverTarget.HoverDidChange(transform.gameObject, false);
                }

                hoverTarget = candidateAnchor;
                hoverTarget.HoverDidChange(transform.gameObject, true);
            }

            transform.position = candidateAnchor.transform.position;
            transform.rotation = candidateAnchor.transform.rotation;
            
        }
        else
        {
            if (hoverTarget != null) {
                hoverTarget.HoverDidChange(transform.gameObject, false);
                hoverTarget = null;
            }

            transform.rotation = mouseDownRotation;
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromScreen));
        }

        RaycastHit hit;
        int layerMask = 1 << 8;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            AnchorBehavior behavior = hit.transform.GetComponent<AnchorBehavior>();
            if (behavior && behavior.draggableTypes.Contains(draggableType) && !behavior.Occupied && behavior != candidateAnchor)
            {
                candidateAnchor = behavior;

                GameManager.Instance.RequestPlayDishPickUpSound();
            }
        }
        else
        {
            candidateAnchor = null;
        }
    }

    void EndDrag()
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
        else if (DiesOnRelease)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Returning " + gameObject + " to its initial position");
            GameManager.Instance.RequestPlayDishPickUpSound();
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
            Anchor.Detach(transform.gameObject);
            abandondedAnchor = Anchor;
            Anchor = null;
        }
    }

    public void AttachToAnchor(AnchorBehavior anchor)
    {
        abandondedAnchor = null;
        Debug.Log("Attaching " + gameObject + " to anchor point " + anchor.gameObject);
        this.Anchor = anchor;
        transform.position = anchor.transform.position;
        transform.rotation = anchor.transform.rotation;
        attached = true;
        anchor.Attach(transform.gameObject);
    }
}
