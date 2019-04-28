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

        TimeStarted();
        IncrementTime();
        CheckForGameEnd();
    }

    private void TimeStarted()
    {
        if (clock != null && !clock.isLive)
        {
            clock.startClock(RealWorldSecondsPerIngameDay);
        }
    }

    private void IncrementTime()
    {
        accumulatedTime += Time.timeScale * Time.deltaTime;
        if (accumulatedTime > RealWorldSecondsPerIngameDay)
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
        accumulatedTime = 0.0f;
        daysPassed += 1;

        clock.setDay(daysPassed);
    }

    private void OnGameOver()
    {
        isDone = true;

        if(clock != null)
        {
            clock.stopClock();
        }

        gm.EndGame();
    }
}
