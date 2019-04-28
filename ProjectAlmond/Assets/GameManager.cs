using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool shouldPlayIntroSequence;

    List<PetriDishSlot> petriDishSlots;
    EmptyPetriDishManager emptyPetriDishManager;

    bool gameBegun;
    GameObject beginButton;
    GameObject learnButton;
    GameObject titleButtonsParent;
    Light fillLight;
    CameraController cameraController;
    float spotAngleHigh;
    float spotAngleLow = 23;
    // Start is called before the first frame update
    void Start()
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
    }

    public void CultureAnchorPointEventCallback(GameObject g, Culture c, CultureAnchorPoint a)
    {
        Debug.Log(g + " " + c + " " + a);
    }
}
