using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{

    public GameObject fanBlades;

    [Range(0,10.0f)]
    public float rotationSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fanBlades.transform.Rotate(0, rotationSpeed, 0);
    }
}
