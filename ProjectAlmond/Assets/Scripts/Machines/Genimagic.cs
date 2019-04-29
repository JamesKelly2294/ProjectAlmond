using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genimagic : MonoBehaviour
{

    GameObject attachedDish;
    Culture culture;
    CultureAnchorPoint cultureAnchor;

    System.Random rand = new System.Random();

    public List<List<AlleleModifier>> modifiers = new List<List<AlleleModifier>>();

    // Start is called before the first frame update
    void Start()
    {
        var a = new List<AlleleModifier>();
        a.Add(new AlleleModifier(AlleleType.HeatResistance, AlleleEffect.Increase, AlleleStrength.Strong));

        var b = new List<AlleleModifier>();
        a.Add(new AlleleModifier(AlleleType.RadiationResistance, AlleleEffect.Increase, AlleleStrength.Strong));

        var c = new List<AlleleModifier>();
        a.Add(new AlleleModifier(AlleleType.ColdResistance, AlleleEffect.Increase, AlleleStrength.Strong));


        modifiers.Add(a);
        modifiers.Add(b);
        modifiers.Add(c);

        modifiers.Add(a);
        modifiers.Add(b);
        modifiers.Add(c);

    }

    private bool shouldStartRunning = false;
    private bool shouldEject = false;
    private bool running = false;

    private float time = 0;

    // Update is called once per frame
    void Update()
    {
        if (shouldStartRunning) {
            Debug.Log("RUN GENIMAGIC");
            this.GetComponentInChildren<DishReceptical>().startAnimating();
            this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.95f, 0.1f);
            this.GetComponentInChildren<DishReceptical>().startAnimating();
            attachedDish.GetComponentInChildren<Draggable>().LockUserInteraction();
            attachedDish.GetComponentInChildren<Culture>().dishLabel = "";
            running = true;
            shouldStartRunning = false;
        }

        if(running) {
            time += Time.fixedDeltaTime;
            if (time > 2) {
                time = 0;

                var modifier = modifiers[0];
                modifiers.RemoveAt(0);

                Debug.Log("Working on: " + modifier);
                culture.Genome.apply(modifier, rand);
                culture.SetGenome(culture.Genome);
            }

            Debug.Log(attachedDish);
            Debug.Log(shouldEject);
            Debug.Log(modifiers);
            if (attachedDish == null || shouldEject || modifiers.Count <= 0) {
                Debug.Log("EJECTING... GENIMAGIC");
                shouldEject = true;
            }
        }

        if(shouldEject) {
            this.GetComponentInChildren<DishReceptical>().stopAnimating();
            this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.0f, 0.1f);
            attachedDish.GetComponentInChildren<Draggable>().UnlockUserInteraction();

            running = false;
            shouldEject = false;
        }
    }

    public void runButtonWasPressed() {
        if (!shouldEject && !running) {
            shouldStartRunning = true;
        }
    }

    public void ejectButtonWasPressed() {
        if (running) {
            Debug.Log("EJECT GENIMAGIC");
            shouldEject = true;
        }
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
