using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOMatic : MonoBehaviour
{

    public NixyTube[] tubes;

    GameObject attachedDish;
    Culture culture;
    CultureAnchorPoint cultureAnchor;

    // Start is called before the first frame update
    void Start()
    {
        tubes[0].setText("H");
        tubes[1].setText("E");
        tubes[2].setText("L");
        tubes[3].setText("L");
        tubes[4].setText("O");
        tubes[5].setText("");
        tubes[6].setText("WOR");
        tubes[7].setText("LD!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ejectButtonWasPressed() {
        StartCoroutine(showMessage("Ejected..."));
        attachedDish.GetComponentInChildren<Draggable>().UnlockUserInteraction();
        this.GetComponentInChildren<DishReceptical>().stopAnimating();

        attachedDish = null;
        culture = null;
        cultureAnchor = null;
    }

    public IEnumerator showMessage(string msg)
    {

        string str = "        " + msg + "        ";

        for (var i = 0; i < ((str.Length) - 8); i ++) {
            for (var j = 0; j < 8; j++) {
                tubes[j].setText(""+str[i + j]);
            }

            yield return new WaitForSeconds(0.25f);
        }

        tubes[0].setText("");
        tubes[1].setText("");
        tubes[2].setText("");
        tubes[3].setText("");
        tubes[4].setText("");
        tubes[5].setText("");
        tubes[6].setText("");
        tubes[7].setText("");
    }

    public void diskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        this.GetComponentInChildren<DishReceptical>().startAnimating();

        attachedDish = g;
        culture = c;
        cultureAnchor = a;

        g.GetComponentInChildren<Draggable>().LockUserInteraction();


        string str = c.Genome.String;
        for (var i = 0; i < 8; i++) {
            tubes[i].setText("" + str[i]);
        }

        g.GetComponentInChildren<Culture>().dishLabel = "96% Match";
    }
}
