using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culture : MonoBehaviour
{
    public CultureGenome Genome { get; private set; }

    CultureRenderer cultureRenderer;

    public string dishLabel;

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

        Genome = new CultureGenome(alleles.ToArray());
        cultureRenderer.Initialize(Genome);
        cultureRenderer.GetComponentInChildren<TMPro.TextMeshPro>().text = dishLabel;
    }

    // Update is called once per frame
    void Update()
    {   
        cultureRenderer.GetComponentInChildren<TMPro.TextMeshPro>().text = dishLabel;
    }
}
