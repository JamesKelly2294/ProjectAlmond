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
        if(dragging)
        {
            if(!Input.GetMouseButton(0))
            {
                Destroy(plucked);
                dragging = false;
            }

            float distanceFromScreen = Camera.main.WorldToScreenPoint(transform.position).z;
            plucked.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromScreen));
        }
    }

    GameObject plucked;

    // This is dirty and not generic but it'll have to do for now
    void OnMouseDown()
    {
        plucked = reagentBehavior.PluckedReagent();

        dragging = true;
    }
}
