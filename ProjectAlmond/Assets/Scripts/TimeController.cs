using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    /// <summary>
    /// How long an in-game day is in real-world seconds.
    /// </summary>
    public float SecondsPerInGameDay = 30.0f;

    /// <summary>
    /// The days per game.
    /// </summary>
    public int DaysPerGame = 30;

    /// <summary>
    /// The accumulated time.
    /// </summary>
    private float timeSinceInGameDayStarted = 0.0f;

    /// <summary>
    /// The number of days which have passed.
    /// </summary>
    private int daysPassed = 1;

    private GameManager gm;
    private bool isDone = false;

    private Clock clock;

    // Use this for initialization
    void Start()
    {
        this.gm = FindObjectOfType<GameManager>();

        var clock = FindObjectOfType<Clock>();
        if (clock != null)
        {
            this.clock = clock;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.hasGameBegun() || isDone ) {
            return;
        }

        // Kick off any tasks which were waiting for time to begin.
        TimeStarted();

        // Track the accumulated time so far.
        IncrementTime();

        // Kick off any tasks which are dependent upon the game ending.
        CheckForGameEnd();

        // Kick off tasks which are dependent upon a new day starting.
        CheckForNewDay();
    }

    private void IncrementTime()
    {
        var delta = Time.timeScale * Time.deltaTime;
        timeSinceInGameDayStarted += delta;
    }

    private void TimeStarted()
    {
        if (clock != null && !clock.isLive)
        {
            clock.startClock(SecondsPerInGameDay);
        }
    }

    private void CheckForNewDay()
    {
        // Check to see if an ingame day has passed and trigger updates
        // for things that care.
        if (timeSinceInGameDayStarted > SecondsPerInGameDay)
        {
            OnDayPassed();
        }
    }

    private void CheckForGameEnd()
    {
        if (daysPassed > DaysPerGame)
        {
            OnGameOver();
        }
    }

    private void OnDayPassed()
    { 
        timeSinceInGameDayStarted = 0.0f;
        daysPassed += 1;

        // Update the day displayed on the clock so the user knows wtf is going on.
        clock.setDay(daysPassed);
    }

    private void OnGameOver()
    {
        isDone = true;

        if(clock != null)
        {
            clock.stopClock();
        }

        gm.EndGame("You ran out of time. You succumb to the disease");
    }
}
