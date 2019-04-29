using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ReagentAnchorPointEvent : UnityEvent<GameObject, ReagentData, PluckedReagentAnchorPoint> { }

public class PluckedReagentAnchorPoint : AnchorBehavior
{

    public ReagentAnchorPointEvent onAttach;
    public ReagentAnchorPointEvent onDetach;

    public ReagentAnchorPointEvent onHover;
    public ReagentAnchorPointEvent onUnhover;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attach(GameObject attachedObject)
    {
        base.Attach(attachedObject);
        attachedObject.AddComponent<Rigidbody>();

        var data = (attachedObject.GetComponentInChildren<Draggable>().Data as ReagentData);
        GameObject.FindObjectOfType<GameManager>().coinDropper.take(data.price);

        onAttach.Invoke(attachedObject, data, this);
    }

    public override void Detach(GameObject attachedObject)
    {
        base.Detach(attachedObject);

        var data = (attachedObject.GetComponentInChildren<Draggable>().Data as ReagentData);
        onDetach.Invoke(attachedObject, data, this);
    }

    public override void HoverDidChange(GameObject hoveredObject, bool isHovering)
    {
        var data = (hoveredObject.GetComponentInChildren<Draggable>().Data as ReagentData);
        if (isHovering) {
            GameObject.FindObjectOfType<GameManager>().coinDropper.prepareToTake(data.price);
            onHover.Invoke(hoveredObject, data, this);
        } else {
            GameObject.FindObjectOfType<GameManager>().coinDropper.prepareToTake(0);
            onUnhover.Invoke(hoveredObject, data, this);
        }
    }
}
