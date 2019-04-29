using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDishSlot : MonoBehaviour
{
    public bool spawnPetriDishOnStart;
    public GameObject petriDishPrefab;

    public bool shouldSpawnEvilPetriDish;

    GameObject petriDish;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPetriDishOnStart)
        {
            spawn();
        }

    }

    // Update is called once per frame
    void Update()
    {
       if (shouldSpawnEvilPetriDish) {
            var winningGenome = GameObject.FindObjectOfType<GameManager>().winningGenome;
            Debug.Log("HERE AGAIN: "+ winningGenome.String);
            shouldSpawnEvilPetriDish = false;

            spawn();

            var cultureRenderer = this.GetComponentInChildren<CultureRenderer>();
            cultureRenderer.SetGenome(winningGenome);
            GetComponentInChildren<Draggable>().LockUserInteraction();
       } 
    }

    void spawn() {
        petriDish = Instantiate(petriDishPrefab);
        petriDish.GetComponent<Draggable>().AttachToAnchor(GetComponentInChildren<AnchorBehavior>());
        petriDish.transform.parent = transform;
        petriDish.transform.localPosition = Vector3.zero;
    }
}
