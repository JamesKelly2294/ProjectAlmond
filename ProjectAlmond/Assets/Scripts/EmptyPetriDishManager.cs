using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPetriDishManager : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float petriDishStackHeightBuffer;

    [Range(0.1f, 5.0f)]
    public float petriDishFallTime = 2.0f;

    [Range(0, 10)]
    public int petriDishStackMaxCount = 5;
    public GameObject emptyPetriDishPrefab;
    public AnchorBehavior anchorPoint;

    Vector3 petriDishSpawnOffset = new Vector3(0.0f, 15.0f, 0.0f);
    Stack<GameObject> petriDishes;
    bool currentlyOrderingDish;

    Vector3 CurrentTopOffset {
        get
        {
            return petriDishes.Count * new Vector3(0.0f, petriDishStackHeightBuffer, 0.0f);
        }
    }

    IEnumerator MovePetriDishToStack(GameObject petriDish, Vector3 target, float transitionDuration)
    {
        Vector3 startingPos = petriDish.transform.localPosition;
        Vector3 currentVelocity = Vector3.zero;
        while (true)
        {
            Vector3 newPosition =
                Vector3.SmoothDamp(petriDish.transform.localPosition, target, ref currentVelocity, transitionDuration);
            
            if (Vector3.Distance(newPosition, target) <= 0.01f)
            {
                petriDish.transform.localPosition = target;
                break;
            }

            petriDish.transform.localPosition = newPosition;
            transitionDuration -= Time.deltaTime;

            yield return 0;
        }

        currentlyOrderingDish = false;
    }

    public void PurchasePetriDish()
    {
        if (!ValidatePurchase())
        {
            return;
        }

        if(petriDishes.Count > 0)
        {
            petriDishes.Peek().GetComponent<Draggable>().enabled = false;
        }

        currentlyOrderingDish = true;

        GameObject petriDish = Instantiate(emptyPetriDishPrefab);
        petriDish.transform.parent = transform;
        petriDish.transform.localPosition = Vector3.zero + petriDishSpawnOffset;
        anchorPoint.transform.localPosition = CurrentTopOffset;
        StartCoroutine(MovePetriDishToStack(petriDish, CurrentTopOffset, petriDishFallTime));
        
        petriDishes.Push(petriDish);

        return;
    }

    public bool RequestPetriDish()
    {
        if(currentlyOrderingDish)
        {
            return false;
        }

        if (petriDishes.Count > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    bool ValidatePurchase()
    {
        return !currentlyOrderingDish && petriDishes.Count < petriDishStackMaxCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        petriDishes = new Stack<GameObject>(8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
