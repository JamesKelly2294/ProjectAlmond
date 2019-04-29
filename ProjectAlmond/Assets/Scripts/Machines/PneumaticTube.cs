using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PneumaticTube : MonoBehaviour
{
    public AudioClip sellDishAudio;
    public GameObject coinDropper;

    // Start is called before the first frame update
    void Start()
    {
        resetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void diskWasAttached(GameObject g, Culture c, CultureAnchorPoint a)
    {   
        resetText();
        a.Detach(g);

        if (c != null) {
            coinDropper.GetComponent<CoinDropper>().give(c.coinValue);
        } else {
            coinDropper.GetComponent<CoinDropper>().give(5);
        }

        Destroy(g.GetComponentInChildren<Rigidbody>());
        StartCoroutine(TakeDisk(g, 0.3f));
    }

    public void diskIsHovering(GameObject g, Culture c, CultureAnchorPoint a)
    {   
        if (c != null) {
            this.GetComponentInChildren<TMPro.TextMeshPro>().text = "Sell: " + c.coinValue + "€";
        } else {
            this.GetComponentInChildren<TMPro.TextMeshPro>().text = "Sell: " + 5 + "€";
        }
    }

    public void diskIsNotHovering(GameObject g, Culture c, CultureAnchorPoint a)
    {   
        resetText();
    }

    private IEnumerator TakeDisk(GameObject disk, float duration)
    {
        GameManager.Instance.RequestPlaySellDishSound();
        float t = 0.0f;
        Vector3 startingPos = disk.transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);
            
            disk.transform.localPosition = Vector3.Lerp(startingPos, startingPos + new Vector3(0, 10f, 0), t);
            yield return 0;
        }

        var culture = disk.GetComponent<Culture>();
        if (culture != null) {
            culture.Growth = -1;
        }
    }

    void resetText() {
        this.GetComponentInChildren<TMPro.TextMeshPro>().text = "Sell your Life";
    }
}
