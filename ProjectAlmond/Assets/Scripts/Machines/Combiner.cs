﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combiner : MonoBehaviour
{

    public GameObject tlHolder;
    GameObject tlDish;
    Culture tlCulture;
    CultureAnchorPoint tlAnchor;

    public GameObject trHolder;
    GameObject trDish;
    Culture trCulture;
    CultureAnchorPoint trAnchor;

    public GameObject bHolder;
    GameObject bDish;
    Culture bCulture;
    CultureAnchorPoint bAnchor;

    public Light combinerLightGood;
    public Light combinerLightBad;

    public GameObject petriDishPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tlDish && trDish)
        {
            InputGrowth = tlCulture.Growth / 2.0f + trCulture.Growth / 2.0f;
        }
        else if (tlDish)
        {
            InputGrowth = tlCulture.Growth / 2.0f;
        }
        else if (trDish)
        {
            InputGrowth = trCulture.Growth / 2.0f;
        }

        ValidateInput(InputGrowth);
    }

    float InputGrowth = 0.0f;

    public void run() {
        if ((tlDish != null || trDish != null) && bDish != null) {
            if(ValidateInput(InputGrowth))
            {
                StartCoroutine(amalgomate());
            }
        }
    }

    public IEnumerator amalgomate() {

        if (tlDish != null) { 
            tlHolder.GetComponent<DishReceptical>().startAnimating(); 
            tlDish.GetComponentInChildren<Draggable>().LockUserInteraction();
        }

        if (trDish != null) { 
            trHolder.GetComponent<DishReceptical>().startAnimating(); 
            trDish.GetComponentInChildren<Draggable>().LockUserInteraction();
        }

        if (bDish != null)  {  
            bHolder.GetComponent<DishReceptical>().startAnimating(); 
            bDish.GetComponentInChildren<Draggable>().LockUserInteraction();
        }

        GameManager.Instance.RequestPlayCombinerSound();

        yield return new WaitForSeconds(2);

        ////
        // Create new SPAWN here
        //  - Perform amalgomation
        //  - Create Petri Dish
        ///
        
        if (bDish)
        {
            if (tlDish && trDish) {
                CultureGenome mutated = tlCulture.Genome.combine(trCulture.Genome, new System.Random());
                
                Debug.Log("Parent A: " + tlCulture.Genome.String);
                Debug.Log("Parent B: " + trCulture.Genome.String);
                Debug.Log("Child: " + mutated.String);

                GameObject petriDish = Instantiate(petriDishPrefab);
                
                var culture = petriDish.GetComponent<Culture>();
                culture.Growth = InputGrowth;
                culture.SetGenome(mutated);
                FindObjectOfType<GameManager>().GrowableCultures.Add(culture);


                tlCulture.Growth = tlCulture.Growth / 2.0f;
                tlCulture.GetComponent<CultureRenderer>().Growth = tlCulture.Growth;
                trCulture.Growth = trCulture.Growth / 2.0f;
                trCulture.GetComponent<CultureRenderer>().Growth = trCulture.Growth;
                
                Draggable draggable = petriDish.GetComponent<Draggable>();

                CultureAnchorPoint anchor = bAnchor;
                GameObject dish = bDish;
                bDish.GetComponent<Draggable>().DetachFromAnchor();
                Destroy(dish);
                draggable.AttachToAnchor(anchor);

                petriDish.transform.rotation = bAnchor.transform.rotation;
                petriDish.transform.parent = bAnchor.transform;
                petriDish.transform.localPosition = Vector3.zero;
            } else if (tlDish)
            {
                CultureGenome mutated = tlCulture.Genome.combine(tlCulture.Genome, new System.Random());

                Debug.Log("Split Parent A: " + tlCulture.Genome.String);
                Debug.Log("Child: " + mutated.String);

                GameObject petriDish = Instantiate(petriDishPrefab);

                var culture = petriDish.GetComponent<Culture>();
                culture.Growth = InputGrowth;
                culture.SetGenome(mutated);
                FindObjectOfType<GameManager>().GrowableCultures.Add(culture);

                tlCulture.Growth = tlCulture.Growth / 2.0f;
                tlCulture.GetComponent<CultureRenderer>().Growth = tlCulture.Growth;

                Draggable draggable = petriDish.GetComponent<Draggable>();

                CultureAnchorPoint anchor = bAnchor;
                GameObject dish = bDish;
                bDish.GetComponent<Draggable>().DetachFromAnchor();
                Destroy(dish);
                draggable.AttachToAnchor(anchor);

                petriDish.transform.rotation = bAnchor.transform.rotation;
                petriDish.transform.parent = bAnchor.transform;
                petriDish.transform.localPosition = Vector3.zero;
            } else if (trDish)
            {
                CultureGenome mutated = trCulture.Genome.combine(trCulture.Genome, new System.Random());

                Debug.Log("Split Parent B: " + trCulture.Genome.String);
                Debug.Log("Child: " + mutated.String);

                GameObject petriDish = Instantiate(petriDishPrefab);

                var culture = petriDish.GetComponent<Culture>();
                culture.Growth = InputGrowth;
                culture.SetGenome(mutated);
                FindObjectOfType<GameManager>().GrowableCultures.Add(culture);

                trCulture.Growth = trCulture.Growth / 2.0f;
                trCulture.GetComponent<CultureRenderer>().Growth = trCulture.Growth;

                Draggable draggable = petriDish.GetComponent<Draggable>();

                CultureAnchorPoint anchor = bAnchor;
                GameObject dish = bDish;
                bDish.GetComponent<Draggable>().DetachFromAnchor();
                Destroy(dish);
                draggable.AttachToAnchor(anchor);

                petriDish.transform.rotation = bAnchor.transform.rotation;
                petriDish.transform.parent = bAnchor.transform;
                petriDish.transform.localPosition = Vector3.zero;
            }
        } 
        
        if (tlDish != null) { 
            tlHolder.GetComponent<DishReceptical>().stopAnimating(); 
            tlDish.GetComponentInChildren<Draggable>().UnlockUserInteraction();
        }

        if (trDish != null) { 
            trHolder.GetComponent<DishReceptical>().stopAnimating(); 
            trDish.GetComponentInChildren<Draggable>().UnlockUserInteraction();
        }

        if (bDish != null)  {  
            bHolder.GetComponent<DishReceptical>().stopAnimating(); 
            bDish.GetComponentInChildren<Draggable>().UnlockUserInteraction();
        }

    }

    public IEnumerator FlashLight(Light light)
    {
        int numFlashes = 5;

        for(int i = 0; i <= numFlashes; i++)
        {
            light.enabled = true;
            yield return new WaitForSeconds(0.5f);
            light.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public bool ValidateInput(float totalInputGrowth)
    {
        bool validated = false;

        validated = totalInputGrowth >= 0.5f && bCulture == null && bDish != null;

        if (validated)
        {
            combinerLightBad.enabled = false;
            combinerLightGood.enabled = true;
        } else
        {
            combinerLightBad.enabled = true;
            combinerLightGood.enabled = false;
        }

        return validated;
    }

    public void topLeftDiskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        tlDish = g;
        tlCulture = c;
        tlAnchor = a;
    }

    public void topLeftDiskWasDetached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        tlDish = null;
        tlCulture = null;
        tlAnchor = null;
    }

    public void topRightDiskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        trDish = g;
        trCulture = c;
        trAnchor = a;
    }

    public void topRightDiskWasDetached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        trDish = null;
        trCulture = null;
        trAnchor = null;
    }

    public void bottomDiskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        bDish = g;
        bCulture = c;
        bAnchor = a;
    }

    public void bottomDiskWasDetached(GameObject g, Culture c, CultureAnchorPoint a)
    {
        bDish = null;
        bCulture = null;
        bAnchor = null;
    }

}
