using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{

    public GameObject coinPrefab;

    List<GameObject> coins = new List<GameObject>();

    public bool isDispensing;
    
    [Range(0.001f, 1.0f)]
    public float iterval;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float time = 0f;

    // Update is called once per frame
    void Update()
    {
        if (isDispensing) {
            time += Time.fixedDeltaTime;
            if (time > iterval) {
                time -= iterval;

                var coin = Instantiate(coinPrefab);
                coin.transform.position = this.transform.position;
                coins.Add(coin);
            }
        }
    }
}
