using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneumaticTube : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void diskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {   
        a.Detach(g);
        Destroy(g.GetComponentInChildren<Rigidbody>());
        StartCoroutine(TakeDisk(g, 1f));
    }

    private IEnumerator TakeDisk(GameObject disk, float duration)
    {

        float t = 0.0f;
        Vector3 startingPos = disk.transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);
            
            disk.transform.localPosition = Vector3.Lerp(startingPos, startingPos + new Vector3(0, 10f, 0), t);
            yield return 0;
        }

        Destroy(disk, 0.1f);
    }
}
