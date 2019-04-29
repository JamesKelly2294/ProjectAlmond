using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pluckable : MonoBehaviour
{
    // This should be far more generic... but it's not D:

    ReagentBehavior reagentBehavior;
    // Start is called before the first frame update
    void Start()
    {
        reagentBehavior = transform.parent.GetComponent<ReagentBehavior>();
    }

    bool dragging;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int mask = 1 << 13;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask)) 
            {
                if(hit.transform.gameObject != gameObject)
                {
                    return;
                }
                Debug.Log("FOUND");
                // Should check for other obstacles
                MouseDown();
            }
        }
    }

    GameObject plucked;

    // This is dirty and not generic but it'll have to do for now
    void MouseDown()
    {
        plucked = reagentBehavior.PluckedReagent();

        if(!plucked)
        {
            return;
        }

        plucked.GetComponent<Draggable>().RequestBeginDrag();

        //dragging = true;
    }
}
