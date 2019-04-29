using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    public GameObject hourPivot;
    public GameObject minutePivot;

    public bool isLive;

    private float realWorldSecondsPerDay;
    private int currentDay;

    private float hourRotation;
    private float minuteRotation;

    // Start is called before the first frame update
    void Start()
    {
        isLive = false;

        hourRotation = -90.0f;
        minuteRotation = -90.0f;

        // uhhh
        realWorldSecondsPerDay = 3600.0f;
        currentDay = 1;
        setDay(1);

        // Hack becausee the prefab is being dumb.

        setHandPosition(hourPivot, hourRotation);
        setHandPosition(minutePivot, minuteRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLive)
        {
            return;
        }

        var scale = (60.0f / realWorldSecondsPerDay);

        // Rotate twice per in-game day (2.0f * 360.0f / 60.0f = 12.0f),
        // scaled by how many real world seconds each in-game second takes.
        hourRotation -= Time.deltaTime * 12.0f * scale;

        // Track the minute hand by rotating 60 times as fast as the hour, but only rotating once per in-game hour.
        minuteRotation -= Time.deltaTime * 180.0f * scale;

        // Spin the dial!
        setHandPosition(hourPivot, hourRotation);
        setHandPosition(minutePivot, minuteRotation);
    }

    private void setHandPosition(GameObject pivot, float rotation)
    {
        var rot = new Vector3(0.0f, 0.0f, rotation);
        pivot.transform.rotation = Quaternion.Euler(rot);
    }

    public void setDay(int day)
    {
        this.currentDay = day;
        this.GetComponentsInChildren<TextMeshPro>()[1].text = "" + day;
        this.hourRotation = -90.0f;
        this.minuteRotation = -90.0f;
    }

    public void startClock(float realWorldSecondsPerDay) {
        this.realWorldSecondsPerDay = realWorldSecondsPerDay;
        this.isLive = true;
    }

    public void stopClock()
    {
        setDay(1);
        this.isLive = false;
    }
}
