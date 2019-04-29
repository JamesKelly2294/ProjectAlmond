using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluckedReagentAnchorPoint : AnchorBehavior
{
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
    }

    public override void Detach(GameObject attachedObject)
    {
        base.Detach(attachedObject);
        
    }

    public override void HoverDidChange(GameObject hoveredObject, bool isHovering)
    {

    }
}
