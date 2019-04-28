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

    Light fillLight;
    CameraController cameraController;
    float spotAngleStart;
    // Start is called before the first frame update
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        petriDishSlots = FindObjectsOfType<PetriDishSlot>().ToList();
        emptyPetriDishManager = FindObjectOfType<EmptyPetriDishManager>();

        if (shouldPlayIntroSequence)
        {
            fillLight = GameObject.Find("Fill Light").GetComponent<Light>();
            spotAngleStart = fillLight.spotAngle;
            fillLight.spotAngle = 0;

        } else
        {
            cameraController.PanToAngle(cameraController.baseview, 2.0f);
            BeginGame();
        }
    }

    public void BeginPlayIntroSequence()
    {
        cameraController.PanToAngle(cameraController.overview, 2.0f);
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        while (!Input.GetMouseButtonUp(0))
        {
            yield return 0;
        }

        float t = 0;
        float duration = 2.0f;
        cameraController.PanToAngle(cameraController.baseview, 2.0f);
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);

            fillLight.spotAngle = Mathf.Lerp(0.0f, spotAngleStart, t);

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

        gameBegun = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
