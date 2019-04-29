using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool shouldPlayIntroSequence;

    List<PetriDishSlot> petriDishSlots;
    EmptyPetriDishManager emptyPetriDishManager;

    public CultureGenome winningGenome;

    public CoinDropper coinDropper;

    bool gameBegun;
    GameObject beginButton;
    GameObject learnButton;
    GameObject titleButtonsParent;
    Light fillLight;
    CameraController cameraController;
    float spotAngleHigh;
    float spotAngleLow = 23;


    /// <summary>
    /// I said a bad word.
    /// </summary>
    public float GrowthRetardingFactor = 0.01f;

    // Start is called before the first frame update
    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        petriDishSlots = FindObjectsOfType<PetriDishSlot>().ToList();
        emptyPetriDishManager = FindObjectOfType<EmptyPetriDishManager>();
        beginButton = GameObject.Find("BeginButton");
        learnButton = GameObject.Find("LearnButton");
        titleButtonsParent = GameObject.Find("TitleButtonsParent");

        if (shouldPlayIntroSequence)
        {
            fillLight = GameObject.Find("Fill Light").GetComponent<Light>();
            spotAngleHigh = fillLight.spotAngle;
            fillLight.spotAngle = spotAngleLow;

        } else
        {
            cameraController.transform.position = cameraController.baseview.position;
            cameraController.transform.rotation = cameraController.baseview.rotation;
            BeginGame();
        }
    }

    public void BeginPlayIntroSequence()
    {
        cameraController.PanToAngle(cameraController.overview, 2.0f);
        StartCoroutine(HideTitle());
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator HideTitle()
    {
        float t = 0.0f;
        float duration = 1.0f;
        Vector3 startingPos = titleButtonsParent.transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);

            titleButtonsParent.transform.localPosition = Vector3.Lerp(startingPos, startingPos +  0.5f * Vector3.forward, t);
            yield return 0;
        }
    }

    IEnumerator PlayIntroSequence()
    {
        while (!Input.GetMouseButtonUp(0) || !cameraController.RequestPanToAngle(cameraController.baseview, 2.0f))
        {
            yield return 0;
        }

        float t = 0;
        float duration = 2.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);

            fillLight.spotAngle = Mathf.Lerp(spotAngleLow, spotAngleHigh, t);

            yield return 0;
        }

        BeginGame();
    }

    void BeginGame()
    {
        if (gameBegun)
        {
            return;
        }

        GenerateWinning();

        beginButton.SetActive(false);
        learnButton.SetActive(false);
        gameBegun = true;
    }

    public bool hasGameBegun()
    {
        return gameBegun;
    }

    public void EndGame()
    {
        // TODO
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Kick off any tasks related to incrementing culture growth.
        IncrementGrowth();
    }

    // At some interval, queue up the cultures, look at their growth rate, and grow them by some amount
    // This should probably happen in the GameManager by the time will trigger the 

    private void IncrementGrowth()
    {
        var cultures = FindObjectsOfType<Culture>();
        if (cultures == null || cultures.Length == 0)
        {
            return;
        }

        foreach (var culture in cultures)
        {
            // Cap growth at 100%
            if (culture.Growth >= 1.0f)
            {
                culture.Growth = 1.0f;
            }

            // Remove the culture if it dies or is used up.
            else if (culture.Growth < 0.001f && culture.Growth > -0.001)
            {
                Destroy(culture.gameObject);
                //culture.GetComponent<Draggable>().draggableType = DraggableType.EmptyDish;
                //var renderer = culture.GetComponent<CultureRenderer>();
                //Destroy(renderer);
            }

            // Increment growth.
            else
            {
                var growthFactor = (float) culture.Genome.growRate.value;
                var delta = GrowthRetardingFactor * Time.timeScale * Time.deltaTime * growthFactor;
                culture.Growth += delta;
            }
        }
    }

    public void CultureAnchorPointEventCallback(GameObject g, Culture c, CultureAnchorPoint a)
    {
        Debug.Log(g + " " + c + " " + a);
    }

    public void GenerateWinning() {
        var rand = new System.Random();
        Allele[] allels = new Allele[CultureGenome.Length];
        for (var i = 0; i < CultureGenome.Length; i++) {
            allels[i] = new Allele(rand.Next(0, Allele.AlleleStrength));
        }

        winningGenome = new CultureGenome(allels);
        Debug.Log("Winning Genome is: " + winningGenome.String);
    }
}
