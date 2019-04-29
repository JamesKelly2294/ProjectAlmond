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
