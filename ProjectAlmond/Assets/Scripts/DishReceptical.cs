using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishReceptical : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.stopAnimating();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startAnimating() {
        this.GetComponentInChildren<Light>().enabled = true;
        var system = this.GetComponentInChildren<ParticleSystem>();
        system.Play();
    }


    public void stopAnimating() {
        this.GetComponentInChildren<Light>().enabled = false;
        var system = this.GetComponentInChildren<ParticleSystem>();
        system.Stop();
    }
}
