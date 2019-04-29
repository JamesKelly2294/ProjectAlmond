using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyPetriDishManager : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float petriDishStackHeightBuffer;

    [Range(0.1f, 5.0f)]
    public float petriDishFallTime = 2.0f;

    [Range(0, 50)]
    public int petriDishCost = 5;

    [Range(0, 10)]
    public int petriDishStackMaxCount = 5;
    public GameObject emptyPetriDishPrefab;

    public GameObject coinDropper;
    public GameObject anchorPointPrefab;

    Vector3 petriDishSpawnOffset = new Vector3(0.0f, 15.0f, 0.0f);
    Stack<GameObject> petriDishes;
    List<CultureAnchorPoint> anchorPoints;
    bool currentlyOrderingDish;

    public bool HasEmptyPetriDishes
    {
        get { return petriDishes.Count > 0; }
    }
    
    Vector3 OffsetForIndex(int index)
    {
        return index * new Vector3(0.0f, petriDishStackHeightBuffer, 0.0f);
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

        coinDropper.GetComponent<CoinDropper>().take(petriDishCost);

        if(petriDishes.Count > 0)
        {
            petriDishes.Peek().GetComponent<Draggable>().enabled = false;
        }

        currentlyOrderingDish = true;

        GameObject petriDish = Instantiate(emptyPetriDishPrefab);
        int index = petriDishes.Count;
        CultureAnchorPoint anchorPoint = anchorPoints[index];
        anchorPoint.gameObject.SetActive(true);
        petriDish.GetComponent<Draggable>().AttachToAnchor(anchorPoint);
        petriDish.transform.parent = transform;
        petriDish.transform.localPosition = Vector3.zero + petriDishSpawnOffset;
        StartCoroutine(MovePetriDishToStack(petriDish, OffsetForIndex(index), petriDishFallTime));

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
        if (!coinDropper.GetComponent<CoinDropper>().canTake(petriDishCost)) {
            return false;
        }

        return !currentlyOrderingDish && petriDishes.Count < petriDishStackMaxCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        anchorPoints = new List<CultureAnchorPoint>(petriDishStackMaxCount);
        petriDishes = new Stack<GameObject>(petriDishStackMaxCount);

        for (int i = 0; i < petriDishStackMaxCount; i++)
        {
            int j = i;
            GameObject go = Instantiate(anchorPointPrefab);
            go.transform.parent = transform;
            go.transform.localPosition = OffsetForIndex(i);
            go.name = "Empty Dish Anchor Point - " + i;
            CultureAnchorPoint ab = go.GetComponent<CultureAnchorPoint>();
            anchorPoints.Add(ab);
            go.SetActive(false);
            ab.onAttach.AddListener((GameObject g, Culture c, CultureAnchorPoint cap) =>
            {
                OnAnchorPointAttach(g, c, cap, j);
            });

            ab.onDetach.AddListener((GameObject g, Culture c, CultureAnchorPoint cap) =>
            {
                OnAnchorPointDetach(g, c, cap, j);
            });
        }

        anchorPoints[0].gameObject.SetActive(true);
    }

    void OnAnchorPointAttach(GameObject go, Culture c, CultureAnchorPoint cap, int index)
    {
        cap.gameObject.SetActive(false);

        if(petriDishes.Count > 0)
        {
            petriDishes.Peek().GetComponent<Draggable>().enabled = false;
        }
        petriDishes.Push(go);

        if(index >= petriDishStackMaxCount - 1)
        {
            return;
        }

        anchorPoints[index + 1].gameObject.SetActive(true);
        //petriDishes.ToArray()[index].GetComponent<Draggable>().enabled = true; // lol this is terrible
    }

    void OnAnchorPointDetach(GameObject go, Culture c, CultureAnchorPoint cap, int index)
    {
        petriDishes.Pop();
        anchorPoints[index].gameObject.SetActive(true);

        if (index < petriDishStackMaxCount - 1)
        {
            anchorPoints[index + 1].gameObject.SetActive(false);
        }

        if (petriDishes.Count > 0)
        {
            petriDishes.Peek().GetComponent<Draggable>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
