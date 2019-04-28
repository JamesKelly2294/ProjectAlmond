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
    public CultureAnchorPoint anchorPoint;

    Vector3 petriDishSpawnOffset = new Vector3(0.0f, 15.0f, 0.0f);
    Stack<GameObject> petriDishes;
    bool currentlyOrderingDish;

    Vector3 FutureTopOffset
    {
        get
        {
            return (petriDishes.Count - 1) * new Vector3(0.0f, petriDishStackHeightBuffer, 0.0f);
        }
    }

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

    bool ignoreNextDetach;
    public void PurchasePetriDish()
    {
        if (!ValidatePurchase())
        {
            return;
        }

        currentlyOrderingDish = true;

        GameObject petriDish = Instantiate(emptyPetriDishPrefab);
        petriDish.GetComponent<Draggable>().AttachToAnchor(anchorPoint);
        ignoreNextDetach = true; // THIS IS SO FUCKING HACKY
        petriDish.transform.parent = transform;
        petriDish.transform.localPosition = Vector3.zero + petriDishSpawnOffset;
        StartCoroutine(MovePetriDishToStack(petriDish, FutureTopOffset, petriDishFallTime));

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
        anchorPoint.onAttach.AddListener(OnAnchorPointAttach);
        anchorPoint.onDetach.AddListener(OnAnchorPointDetach);
    }

    void OnAnchorPointAttach(GameObject go, Culture c, CultureAnchorPoint cap)
    {
        if (petriDishes.Contains(go))
        {
            return;
        }

        if (petriDishes.Count > 0)
        {
            Draggable previousDraggable = petriDishes.Peek().GetComponent<Draggable>();
            previousDraggable.DetachFromAnchor();
            previousDraggable.enabled = false;
        }

        Debug.Log("ATTACH");
        anchorPoint.transform.localPosition = CurrentTopOffset;
        petriDishes.Push(go);
    }

    void OnAnchorPointDetach(GameObject go, Culture c, CultureAnchorPoint cap)
    {
        if(ignoreNextDetach)
        {
            ignoreNextDetach = false;
            return;
        }
        petriDishes.Pop();
        anchorPoint.transform.localPosition = CurrentTopOffset;

        Debug.Log("DETACH");
        if (petriDishes.Count > 0)
        {
            Draggable previousDraggable = petriDishes.Peek().GetComponent<Draggable>();
            previousDraggable.enabled = true;
            previousDraggable.AttachToAnchor(anchorPoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
