using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culture : MonoBehaviour
{
    CultureGenome genome;
    CultureRenderer cultureRenderer;

    // Start is called before the first frame update
    void Start()
    {
        cultureRenderer = GetComponent<CultureRenderer>();
        System.Random rand = new System.Random();

        List<Allele> alleles = new List<Allele>(CultureGenome.Length);

        for(int i = 0; i < alleles.Capacity; i++)
        {
            alleles.Add(new Allele(rand.Next(0, Allele.AlleleStrength)));
        }

        genome = new CultureGenome(alleles.ToArray());
        cultureRenderer.Initialize(genome);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
