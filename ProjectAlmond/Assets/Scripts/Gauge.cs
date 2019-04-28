using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gauge : MonoBehaviour
{
    /// <summary>
    /// The object to pivot the needle around.
    /// </summary>
    public GameObject pivot;

    /// <summary>
    /// The game object to use for progress markers on the guage face.
    /// </summary>
    public GameObject progressMarkerObject;

    /// <summary>
    /// The number of tick marks to add to the gauge to indicate
    /// how far the needle has moved across it.
    /// </summary>
    [Range(0, 100)]
    public int progressMarkerCount = 10;

    /// <summary>
    /// The angle between the min/max rotation range of the gauge's needle. 
    /// </summary>
    [Range(0, 180)]
    public int sweepAngle = 180;

    private int halfAngle;

    /// <summary>
    /// 
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float needleProgress = 0.0f;

    /// <summary>
    /// The distance from the pivot point that markers should be placed.
    /// </summary>
    public float progressMarkerDistance = 1.0f;

    /// <summary>
    /// A list of progress markers on the dial of the gauge.
    /// </summary>
    private List<GameObject> progressMarkers;

    // Start is called before the first frame update
    void Start()
    {
        progressMarkers = new List<GameObject>();
        halfAngle = this.sweepAngle / 2;

        pivot.transform.rotation = new Quaternion(0.0f, 0.0f, calculateZ(this.needleProgress), 1.0f);

        //progressMarkerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //progressMarkerCount = 4;

        //sweepAngle = 60;
        //needleProgress = 0.5f;
        //progressMarkerDistance = 5.0f;

        SetNeedleProgress(1.0f, 5.0f);
    }

    float calculateZ(float progress)
    {
        //return -1.0f * progress * (( 90.0f )/ 360.0f);
        return (this.sweepAngle - (2.0f * this.sweepAngle * progress)) / 180.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNeedleProgress(float progress, float transitionDuration)
    {
        StartCoroutine(BeginSetNeedleProgress(progress, transitionDuration));
    }

    IEnumerator BeginSetNeedleProgress(float progress, float transitionDuration)
    {
        var start = pivot.transform.rotation;
        var end = new Quaternion(0.0f, 0.0f, calculateZ(progress), 1.0f);

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);
            pivot.transform.rotation = Quaternion.Slerp(start, end, t);

            yield return 0;
        }
    }
}
