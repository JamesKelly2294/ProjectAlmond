using System.Collections;
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

    public GameObject petriDishPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void run() {
        if ((tlDish != null || trDish != null) && bDish != null) {
            StartCoroutine(amalgomate());
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

        yield return new WaitForSeconds(2);

        ////
        // Create new SPAWN here
        //  - Perform amalgomation
        //  - Create Petri Dish
        ///
        
        if (bDish)
        {
            if (tlDish && trDish) {
                Culture tlCulture = tlDish.GetComponent<Culture>();
                Culture trCulture = trDish.GetComponent<Culture>();
                CultureGenome mutated = trCulture.Genome.combine(trCulture.Genome, new System.Random());

                Debug.Log("Parent A: " + tlCulture.Genome.String);
                Debug.Log("Parent B: " + trCulture.Genome.String);
                Debug.Log("Child: " + mutated.String);

                GameObject petriDish = Instantiate(petriDishPrefab);

                var culture = petriDish.GetComponent<Culture>();
                culture.Growth = (tlCulture.Growth + trCulture.Growth) / 2.0f;
                FindObjectOfType<GameManager>().GrowableCultures.Add(culture);

                Draggable draggable = petriDish.GetComponent<Draggable>();

                CultureAnchorPoint anchor = bAnchor;
                GameObject dish = bDish;
                bDish.GetComponent<Draggable>().DetachFromAnchor();
                Destroy(dish);
                draggable.AttachToAnchor(anchor);

                petriDish.transform.rotation = bAnchor.transform.rotation;
                petriDish.transform.parent = bAnchor.transform;
                petriDish.transform.localPosition = Vector3.zero;


                var cultureRenderer = GetComponentInChildren<CultureRenderer>();
                cultureRenderer.SetGenome(mutated);

            } else if (tlDish)
            {

            } else if (trDish)
            {

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
