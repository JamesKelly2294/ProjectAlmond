using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genimagic : MonoBehaviour
{

    GameObject attachedDish;
    Culture culture;
    CultureAnchorPoint cultureAnchor;

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
        this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.95f, 0.1f);
        
        this.GetComponentInChildren<DishReceptical>().startAnimating();

        

        attachedDish.GetComponentInChildren<Draggable>().LockUserInteraction();
        attachedDish.GetComponentInChildren<Culture>().dishLabel = "";
    }

    public void ejectButtonWasPressed() {
        Debug.Log("EJECT GENIMAGIC");
        this.GetComponentInChildren<DishReceptical>().stopAnimating();
        this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.0f, 0.1f);

        attachedDish.GetComponentInChildren<Draggable>().UnlockUserInteraction();
    }

    public void diskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        attachedDish = g;
        culture = c;
        cultureAnchor = a;

        this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.7f, 0.1f);
    }

    public void diskWasDetached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.0f, 0.1f);

        attachedDish = null;
        culture = null;
        cultureAnchor = null;
    }
}
