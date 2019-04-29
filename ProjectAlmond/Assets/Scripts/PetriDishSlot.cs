using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDishSlot : MonoBehaviour
{
    public bool spawnPetriDishOnStart;
    public GameObject petriDishPrefab;
    public float cultureInitialGrowth = 0.25f;

    public bool shouldSpawnEvilPetriDish;
    public Material diseaseOverride;

    GameObject petriDish;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPetriDishOnStart)
        {
            spawn(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
       if (shouldSpawnEvilPetriDish) {
            var winningGenome = FindObjectOfType<GameManager>().winningGenome;
            shouldSpawnEvilPetriDish = false;

            spawn(true);

            var cultureRenderer = GetComponentInChildren<CultureRenderer>();
            cultureRenderer.SetGenome(winningGenome);
       } 
    }

    void spawn(bool isDisease) {
        petriDish = Instantiate(petriDishPrefab);

        var culture = petriDish.GetComponent<Culture>();
        culture.Growth = cultureInitialGrowth;
        FindObjectOfType<GameManager>().GrowableCultures.Add(culture);

        Draggable draggable = petriDish.GetComponent<Draggable>();
        draggable.AttachToAnchor(GetComponentInChildren<AnchorBehavior>());

        petriDish.transform.rotation = transform.rotation;
        petriDish.transform.parent = transform;
        petriDish.transform.localPosition = Vector3.zero;

        if(isDisease)
        {
            draggable.draggableType = DraggableType.Disease;
            // hacky af lel
            petriDish.transform.Find("Dish").GetComponent<MeshRenderer>().material = diseaseOverride;
        }
    }
}
