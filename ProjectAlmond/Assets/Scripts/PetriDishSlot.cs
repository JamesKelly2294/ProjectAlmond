using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDishSlot : MonoBehaviour
{
    public bool spawnPetriDishOnStart;
    public GameObject petriDishPrefab;
    public float cultureInitialGrowth = 0.25f;
    
    public Material diseaseOverride;

    GameObject petriDish;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPetriDishOnStart)
        {
            Spawn();
        }

    }

    public void SpawnDisease(CultureGenome genome)
    {
        var diseaseGenome = genome;

        if (!Spawn())
        {
            return;
        }

        var c = petriDish.GetComponent<Culture>();

        Debug.Log("Setting disease genome");
        c.SetGenome(diseaseGenome);

        Draggable draggable = petriDish.GetComponent<Draggable>();
        draggable.draggableType = DraggableType.Disease;
        petriDish.transform.Find("Dish").GetComponent<MeshRenderer>().material = diseaseOverride;
    }

    bool Spawn() {
        if(petriDish)
        {
            return false;
        }

        petriDish = Instantiate(petriDishPrefab);

        var culture = petriDish.GetComponent<Culture>();
        culture.Growth = cultureInitialGrowth;
        FindObjectOfType<GameManager>().GrowableCultures.Add(culture);

        Draggable draggable = petriDish.GetComponent<Draggable>();
        draggable.AttachToAnchor(GetComponentInChildren<AnchorBehavior>());

        petriDish.transform.rotation = transform.rotation;
        petriDish.transform.parent = transform;
        petriDish.transform.localPosition = Vector3.zero;

        return true;
    }
}
