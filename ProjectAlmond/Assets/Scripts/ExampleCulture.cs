using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCulture : MonoBehaviour
{

    CultureGenome genome;
    System.Random  rand;

    // Start is called before the first frame update
    void Start()
    {
        Allele heatResistance = new Allele(Allele.AlleleStrength);
        Allele aggression = new Allele(0);
        Allele radiationResistance = new Allele(0);
        Allele growRate = new Allele(0);
        Allele coldResistance = new Allele(0);
        Allele mobility = new Allele(0);
        Allele size = new Allele(Allele.AlleleStrength);
        Allele unused = new Allele(0);

        rand = new System.Random(0);
        genome = new CultureGenome(heatResistance, aggression, radiationResistance, growRate, coldResistance, mobility, size, unused);
    }

    // Update is called once per frame
    void Update()
    {
        genome.mutate(rand);
        this.GetComponent<MeshRenderer>().material.color = genome.color;
        this.GetComponentInChildren<TextMesh>().text = genome.String;
    }
}
