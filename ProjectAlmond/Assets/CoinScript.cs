using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity.magnitude);
        if(collision.relativeVelocity.magnitude < 1)
        {
            return;
        }
        if (Random.Range(0, 100) == 1)
        {
            GameManager.Instance.RequestPlayCoinCoinCollisionSound();
        }
    }
}
