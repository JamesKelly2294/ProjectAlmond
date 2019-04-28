using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDishSlot : MonoBehaviour
{
    public bool spawnPetriDishOnStart;
    public GameObject petriDishPrefab;

    GameObject petriDish;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPetriDishOnStart)
        {
            petriDish = Instantiate(petriDishPrefab);
            petriDish.GetComponent<Draggable>().AttachToAnchor(GetComponentInChildren<AnchorBehavior>());
            petriDish.transform.parent = transform;
            petriDish.transform.localPosition = Vector3.zero;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
