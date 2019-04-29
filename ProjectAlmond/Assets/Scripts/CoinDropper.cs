using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDropper : MonoBehaviour
{

    public GameObject coinPrefab;

    public GameObject coinTray;

    public GameObject dropLocation;

    List<GameObject> coins = new List<GameObject>();
    public int numberOfCoinsVisable = 0;
    public int numberOfCoinsNeedingVending = 0;

    public int numberOfCoinsNeedingConsuming = 0;

    public bool isDispensing;

    public bool isConsuming;
    
    [Range(0.001f, 1.0f)]
    public float iterval = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        coinTray.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + numberOfCoinsVisable;
    }

    float dispenseTime = 0f;
    float consumeTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (isDispensing || numberOfCoinsNeedingVending > 0) {
            dispenseTime += Time.fixedDeltaTime;
            if (dispenseTime > iterval) {
                dispenseTime -= iterval;

                if (numberOfCoinsNeedingVending > 0) {
                    numberOfCoinsNeedingVending -= 1;
                }

                var coin = Instantiate(coinPrefab);
                coin.transform.position = dropLocation.transform.position;
                coin.transform.parent = dropLocation.transform;
                coins.Add(coin);
                StartCoroutine(AddCoin(coin));
            }
        }

        if (isConsuming || numberOfCoinsNeedingConsuming > 0) {
            consumeTime += Time.fixedDeltaTime;
            
            if (consumeTime > iterval) {
                consumeTime -= iterval;

                if ( numberOfCoinsVisable > 0 ) {
                    if (numberOfCoinsNeedingConsuming > 0) {
                        numberOfCoinsNeedingConsuming -= 1;
                    }
                    
                    if (coins.Count > 0) {
                        var coinToRemove = coins[coins.Count -1];
                        coins.Remove(coinToRemove);
                        StartCoroutine(RemoveCoin(coinToRemove, 1f));
                    }
                }
            }
        }
    }

    public void give(int coins) {
        if (coins > 0 ) {
            numberOfCoinsNeedingVending += coins;
        }
    }

    public void take(int coins) {
        if (canTake(coins)) {
            if (coins < 15)
            {
                GameManager.Instance.RequestPlaySuckCoinsMicroSound();
            } else
            {
                GameManager.Instance.RequestPlaySuckCoinsSmallSound();
            }
            numberOfCoinsNeedingConsuming += coins;
        }
    }

    public bool canTake(int coins) {
        if (coins > 0 && numberOfCoinsVisable >= coins ) {
            return true;
        }

        return false;
    }

    public void prepareToTake(int coins) {
        if (coins > 0 && numberOfCoinsVisable >= coins ) {
            coinTray.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + (numberOfCoinsVisable - coins);
        } else {
            coinTray.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + numberOfCoinsVisable;
        }
    }

    private IEnumerator AddCoin(GameObject coin)
    {
        yield return new WaitForSeconds(2.0f);
        numberOfCoinsVisable += 1;
        coinTray.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + numberOfCoinsVisable;
        Destroy(coin.GetComponent<Rigidbody>(), 10.0f);
    }

    private IEnumerator RemoveCoin(GameObject coin, float duration)
    {

        numberOfCoinsVisable -= 1;
        coinTray.GetComponentInChildren<TMPro.TextMeshPro>().text = "" + numberOfCoinsVisable;

        float t = 0.0f;
        Vector3 startingPos = coin.transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);
            
            coin.transform.localPosition = Vector3.Lerp(startingPos, Vector3.zero, t);
            yield return 0;
        }

        Destroy(coin, 0.1f);
    }
}
