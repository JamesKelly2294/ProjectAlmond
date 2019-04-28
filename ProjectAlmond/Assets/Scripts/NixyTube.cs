using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NixyTube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setText("ok");
    }

    // Update is called once per frame
    void Update()
    {
        System.Random rand = new System.Random();
        int i = rand.Next(0, 100);
        if ( i == 0) {
            setText("LOW");
        } else if (i == 1) {
            setText("HIGH");
        } else if (i % 2 == 1) {
            setText("GOOD");
        } else if (i % 2 == 0) {
            setText("");
        }
    }

    void setText(string newValue) {
        var light = this.GetComponentInChildren<Light>();
        if (newValue == "") {
            light.enabled = false;
        } else if(!light.enabled) {
            light.enabled = true;
        }

        this.GetComponentInChildren<TextMeshPro>().text = newValue ;
    }
}
