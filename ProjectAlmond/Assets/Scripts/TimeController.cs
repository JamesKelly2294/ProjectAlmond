using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    /// <summary>
    /// How long an in-game day is in real-world seconds.
    /// </summary>
    public float RealWorldSecondsPerIngameDay = 30.0f;

    /// <summary>
    /// The days per game.
    /// </summary>
    public int DaysPerGame = 30;

    /// <summary>
    /// The accumulated time.
    /// </summary>
    private float accumulatedTime = 0.0f;

    /// <summary>
    /// The number of days which have passed.
    /// </summary>
    private int daysPassed = 0;

    private GameManager gm;

    private bool isDone = false;

    // Use this for initialization
    void Start()
    {
        this.gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.hasGameBegun() || isDone ) {
            return;
        }

        accumulatedTime += Time.timeScale * Time.deltaTime;

        if (accumulatedTime > RealWorldSecondsPerIngameDay)
        {
            accumulatedTime = 0.0f;
            daysPassed += 1;
            OnDayPassed(daysPassed);
        }

        if (daysPassed > DaysPerGame)
        {
            OnGameOver();
        }
    }

    public void OnDayPassed(int daysPassed)
    {
        // TODO
        Debug.Log("A Day Passed " + daysPassed);
    }

    public void OnGameOver()
    {
        // TODO
        Debug.Log("RIP?");
        isDone = true;
        gm.EndGame();
    }
}
