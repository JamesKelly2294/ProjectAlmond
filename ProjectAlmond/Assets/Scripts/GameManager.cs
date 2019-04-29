using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    // borrowed from https://wiki.unity3d.com/index.php/Singleton
    static bool m_ShuttingDown = false;
    static object m_Lock = new object();
    static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(GameManager) +
                    "' already destroyed. Returning null.");
                return null;
            }
 
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = FindObjectOfType<GameManager>();
 
                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
m_Instance = singletonObject.AddComponent<GameManager>();
                        singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
 
                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }
 
                return m_Instance;
            }
        }
    }

    public List<AudioClip> CoinCoinCollisionSounds;
    public AudioClip DishPickUpSound;
    public AudioClip SellDishSound;
    public AudioClip SuckCoinsSmallSound;
    public AudioClip SuckCoinsMicroSound;
    public AudioClip DropReagentSound;
    public AudioClip ButtonClickSound;
    public void RequestPlayCoinCoinCollisionSound()
    {
        if(CoinCoinCollisionSounds.Count <= 0)
        {
            return;
        }

        QuietAudioSource.PlayOneShot(CoinCoinCollisionSounds[Random.Range(0, CoinCoinCollisionSounds.Count)]);
    }

    public void RequestPlayDishPickUpSound()
    {
        LoudAudioSource.PlayOneShot(DishPickUpSound);
    }

    public void RequestPlaySellDishSound()
    {
        LoudAudioSource.PlayOneShot(SellDishSound);
    }

    public void RequestPlayDropReagentSound()
    {
        LoudAudioSource.PlayOneShot(DropReagentSound);
    }

    public void RequestPlaySuckCoinsMicroSound()
    {
        LoudAudioSource.PlayOneShot(SuckCoinsMicroSound);
    }

    public void RequestPlaySuckCoinsSmallSound()
    {
        LoudAudioSource.PlayOneShot(SuckCoinsSmallSound);
    }

    public void RequestPlayButtonClickSound()
    {
        LoudAudioSource.PlayOneShot(ButtonClickSound);
    }

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }

    public bool shouldPlayIntroSequence;

    List<PetriDishSlot> petriDishSlots;
    EmptyPetriDishManager emptyPetriDishManager;

    public CultureGenome winningGenome;
    public CultureGenome diseaseGenome;

    public CoinDropper coinDropper;

    bool gameBegun;
    GameObject beginButton;
    GameObject learnButton;
    GameObject titleButtonsParent;
    Light fillLight;
    CameraController cameraController;
    float spotAngleHigh;
    float spotAngleLow = 23;

    Clock clock;

    // Cultures that will be updated during rendering.
    public List<Culture> GrowableCultures;

    // How long ago the last growth update was kicked off, in seconds.
    private float lastGrowthRenderUpdate = 0.0f;

    // Once a second for the lulz.
    public float TimeBetweenCultureRenderUpdates = 0.04f;

    /// <summary>
    /// I said a bad word.
    /// </summary>
    public float GrowthRetardingFactor = 0.01f;

    public AudioSource LoudAudioSource; // lol what a hack
    public AudioSource QuietAudioSource;
    // Start is called before the first frame update
    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        petriDishSlots = FindObjectsOfType<PetriDishSlot>().ToList();
        emptyPetriDishManager = FindObjectOfType<EmptyPetriDishManager>();
        beginButton = GameObject.Find("BeginButton");
        learnButton = GameObject.Find("LearnButton");
        titleButtonsParent = GameObject.Find("TitleButtonsParent");
        clock = FindObjectOfType<Clock>();

        GrowableCultures = new List<Culture>();

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

    public void WinGame()
    {
        clock.stopClock();
        // TODO
    }

    public void EndGame(string message="You died")
    {
        clock.stopClock();
        // TODO
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Check to see if we need to end the game.
        CheckForGameOver();

        // Kick off any tasks related to incrementing culture growth.
        IncrementGrowth();

        // See if the culture needs to be rerendered.
        CheckForCultureRenderUpdate();
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
            else if (culture.Growth < 0.0f)
            {
                Debug.Log("Destroying culture " + culture.name);
                GrowableCultures.Remove(culture);
                Destroy(culture.gameObject);
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

    private void CheckForGameOver()
    {
        var minPlateCost = 5.0f;
        var noPlatesLeft = !emptyPetriDishManager.HasEmptyPetriDishes;
        var canAffordPlate = coinDropper.numberOfCoinsVisable < minPlateCost;

        if ( noPlatesLeft && canAffordPlate )
        {
            EndGame("You can't afford any more petri dishes");
        }
    }

    private void CheckForCultureRenderUpdate()
    {
        lastGrowthRenderUpdate += Time.deltaTime * Time.timeScale;
        if (lastGrowthRenderUpdate > TimeBetweenCultureRenderUpdates)
        {
            lastGrowthRenderUpdate = 0.0f;
            UpdateCultureRenderForGrowth();
        }

    }

    private void UpdateCultureRenderForGrowth()
    {
        foreach( var culture in GrowableCultures )
        {
            var renderer = culture.GetComponent<CultureRenderer>();
            renderer.Growth = culture.Growth;
        }
    }

    public void CultureAnchorPointEventCallback(GameObject g, Culture c, CultureAnchorPoint a)
    {
        Debug.Log(g + " " + c + " " + a);
    }

    public void GenerateWinning() {
        var rand = new System.Random();
        Allele[] wAllels = new Allele[CultureGenome.Length];
        Allele[] lAllels = new Allele[CultureGenome.Length];
        for (var i = 0; i < CultureGenome.Length; i++) {
            wAllels[i] = new Allele(rand.Next(0, Allele.AlleleStrength));
            lAllels[i] = new Allele(Allele.AlleleStrength - wAllels[i].value);
        }

        winningGenome = new CultureGenome(wAllels);
        diseaseGenome = new CultureGenome(lAllels);
        
        Debug.Log("Winning Genome is: " + winningGenome.String);
        Debug.Log("Disease Genome is: " + diseaseGenome.String);
    }
}
