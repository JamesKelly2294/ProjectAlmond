using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour
{
    public Vector3 pressOffset = new Vector3(0.0f, -1.0f, 0.0f);
    public float pressDuration = 1.0f;
    public UnityEvent onPress;

    bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator OnMouseDown()
    {
        if(pressed)
        {
            yield break;
        }

        pressed = true;
        float t = 0.0f;
        Vector3 startingPos = transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / pressDuration);
            
            transform.localPosition = Vector3.Lerp(startingPos, startingPos + pressOffset, t);
            yield return 0;
        }

        onPress.Invoke();

        Vector3 newPos = transform.localPosition;
        t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / pressDuration);

            transform.localPosition = Vector3.Lerp(newPos, startingPos, t);
            yield return 0;
        }
        pressed = false;
    }
}
