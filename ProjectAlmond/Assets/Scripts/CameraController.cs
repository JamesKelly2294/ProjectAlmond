﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform title;
    public Transform overview;
    public Transform baseview;
    public Transform petriDishes;
    public Transform genimagic;
    public Transform checkomatic;
    public Transform amalgamizer;
    public Transform resources;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    bool panning;
    bool rotating;
    public bool RequestPanToAngle(Transform angle, float transitionDuration)
    {
        Debug.Log("RequestPanToAngle");
        if (panning || rotating || (angle.position == transform.position && angle.rotation == transform.rotation))
        {
            return false;
        }

        panning = true;
        rotating = true;
        StartCoroutine(BeginPanToAngle(angle, transitionDuration));
        StartCoroutine(BeginPanToPosition(angle, transitionDuration));

        return true;
    }

    public void PanToAngle(Transform angle, float transitionDuration)
    {
        Debug.Log("PanToAngle");
        if (panning || rotating)
        {
            return;
        }


        panning = true;
        rotating = true;
        StartCoroutine(BeginPanToAngle(angle, transitionDuration));
        StartCoroutine(BeginPanToPosition(angle, transitionDuration));
    }

    IEnumerator BeginPanToAngle(Transform angle, float transitionDuration)
    {
        float t = 0.0f;
        //Vector3 startingPos = transform.position;
        Quaternion startingRotation = transform.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.rotation = Quaternion.Slerp(startingRotation, angle.rotation, t);
            //transform.position = Vector3.Lerp(startingPos, angle.position, t);
            yield return 0;
        }

        Debug.Log("Finished panning");
        rotating = false;
    }

    IEnumerator BeginPanToPosition(Transform position, float transitionDuration)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Vector3 currentVelocity = Vector3.zero;

        Quaternion startingRotation = transform.rotation;
        while (true)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            Vector3 newPosition =
                Vector3.SmoothDamp(transform.position, position.position, ref currentVelocity, transitionDuration);

            if (Vector3.Distance(newPosition, position.position) <= 0.01f)
            {
                transform.position = position.position;
                break;
            }

            transform.position = newPosition;
            transitionDuration -= Time.deltaTime;

            yield return 0;
        }

        panning = false;
    }


}
