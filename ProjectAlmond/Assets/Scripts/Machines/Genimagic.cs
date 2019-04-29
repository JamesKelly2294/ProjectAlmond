using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genimagic : MonoBehaviour
{

    GameObject attachedDish;
    Culture culture;
    CultureAnchorPoint cultureAnchor;

    public GameObject screen;

    System.Random rand = new System.Random();

    public List<ReagentData> modifiers = new List<ReagentData>();

    // Start is called before the first frame update
    void Start()
    {
        evaluatePowerAndDanger();
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
                culture.Genome.apply(modifier.modifiers, rand);
                culture.SetGenome(culture.Genome);

                evaluatePowerAndDanger();
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

            evaluatePowerAndDanger();
        }
    }

    public void runButtonWasPressed() {
        Debug.Log("Run Button was Pressed...");
        if (!shouldEject && !running && modifiers.Count > 0) {
            Debug.Log("Setting should run...");
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

        evaluatePowerAndDanger();
    }

    public void diskWasDetached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        attachedDish = null;
        culture = null;
        cultureAnchor = null;

        evaluatePowerAndDanger();
    }

    public void reagentWasDropped(GameObject droppedObject, ReagentData data, PluckedReagentAnchorPoint point) {
        modifiers.Add(data);
        evaluatePowerAndDanger();
    }

    public void evaluatePowerAndDanger() {
        if (modifiers.Count == 0) {
            screen.GetComponent<TMPro.TextMeshPro>().text = "";
        } else {
            var str = "";

            foreach (var data in modifiers) {
                str += "" + data.name + "\n";
            }

            screen.GetComponent<TMPro.TextMeshPro>().text = str;
        }

        if (attachedDish == null) {
            this.GetComponentInChildren<Gauge>().SetNeedleProgress(0.0f, 0.1f);
        } else {
            this.GetComponentInChildren<Gauge>().SetNeedleProgress(Mathf.Min(0.1f * modifiers.Count, 1f), 0.1f);
        }
    }
}
