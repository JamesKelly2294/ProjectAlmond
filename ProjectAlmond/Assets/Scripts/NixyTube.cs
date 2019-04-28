using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NixyTube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setText(string newValue) {
        var light = this.GetComponentInChildren<Light>();
        if (newValue.Trim() == "") {
            light.enabled = false;
        } else if(!light.enabled) {
            light.enabled = true;
        }

        this.GetComponentInChildren<TextMeshPro>().text = newValue ;
    }
}
