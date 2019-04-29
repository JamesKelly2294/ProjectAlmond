using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenimagicInput : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Draggable>().DetachFromAnchor();

        ReagentData c = (ReagentData)other.GetComponent<Draggable>().Data;
        Debug.Log("Sending the following resource to Genimagic: " + c.modifiers);

        Destroy(other.gameObject);
    }
}
