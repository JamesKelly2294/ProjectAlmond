using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genimagic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void runButtonWasPressed() {
        Debug.Log("RUN GENIMAGIC");
        this.GetComponentInChildren<DishReceptical>().startAnimating();
        this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.7f, 0.1f);
    }

    public void ejectButtonWasPressed() {
        Debug.Log("EJECT GENIMAGIC");
        this.GetComponentInChildren<DishReceptical>().stopAnimating();
        this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.0f, 0.1f);
    }
}
