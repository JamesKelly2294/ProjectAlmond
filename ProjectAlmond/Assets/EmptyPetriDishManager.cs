using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPetriDishManager : MonoBehaviour
{
    public GameObject emptyPetriDishPrefab;

    List<GameObject> petriDishes;

    public bool RequestPetriDish()
    {
        if (petriDishes.Count > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
