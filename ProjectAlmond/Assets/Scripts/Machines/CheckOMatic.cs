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
        this.clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void diskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        this.GetComponentInChildren<DishReceptical>().startAnimating();
        GameManager.Instance.RequestPlayCheckerSound();

        attachedDish = g;
        culture = c;
        cultureAnchor = a;

        Debug.Log("Received culture " + c + " with Genome " + c.Genome.String);
        string str = c.Genome.String;
        for (var i = 0; i < 8; i++) {
            tubes[i].setText("" + str[i]);
        }

        var winningGenome = GameObject.FindObjectOfType<GameManager>().winningGenome;
        var simularity = c.Genome.SimularityTo(winningGenome);

        var s = Mathf.RoundToInt(simularity * 100);
        c.dishLabel = s + "% Match";
        c.coinValue = Mathf.RoundToInt(250 * simularity * c.Growth);

        if (simularity >= 0.9f && (c.Genome != GameManager.Instance.diseaseGenome))
        {
            Debug.Log("Win Triggered");
            GameManager.Instance.WinGame();
        }
    }

    public void diskWasDetached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        if (attachedDish == null) {
            return;
        }

        this.GetComponentInChildren<DishReceptical>().stopAnimating();
        this.clear();

        attachedDish = null;
        culture = null;
        cultureAnchor = null;
    }

    public void clear() {
        for (var i = 0; i < 8; i++) {
            tubes[i].setText("");
        }
    }
}
