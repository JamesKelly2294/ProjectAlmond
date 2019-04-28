﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform overview;
    public Transform baseview;
    public Transform petriDishes;
    public Transform genimagic;
    public Transform checkomatic;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PanToAngle(Transform angle, float transitionDuration)
    {
        StartCoroutine(BeginPanToAngle(angle, transitionDuration));
    }

    IEnumerator BeginPanToAngle(Transform angle, float transitionDuration)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingRotation = transform.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.rotation = Quaternion.Slerp(startingRotation, angle.rotation, t);
            transform.position = Vector3.Lerp(startingPos, angle.position, t);
            yield return 0;
        }
    }
}