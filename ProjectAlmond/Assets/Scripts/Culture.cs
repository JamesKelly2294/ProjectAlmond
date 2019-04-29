using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culture : MonoBehaviour
{
    public CultureGenome Genome { get; private set; }

    CultureRenderer cultureRenderer;

    public string dishLabel;

    public float Growth;

    public int coinValue = 20;

    // Start is called before the first frame update
    void Awake()
    {
        System.Random rand = new System.Random();

        List<Allele> alleles = new List<Allele>(CultureGenome.Length);

        for(int i = 0; i < alleles.Capacity; i++)
        {
            alleles.Add(new Allele(rand.Next(0, Allele.AlleleStrength)));
        }

        Genome = new CultureGenome(alleles.ToArray());
        cultureRenderer = GetComponent<CultureRenderer>();
        cultureRenderer.Initialize(Genome);
        cultureRenderer.GetComponentInChildren<TMPro.TextMeshPro>().text = dishLabel;
    }

    // Update is called once per frame
    void Update()
    {   
        cultureRenderer.GetComponentInChildren<TMPro.TextMeshPro>().text = dishLabel;
    }

    public void SetGenome(CultureGenome genome)
    {
        Debug.Log("Current: " + this.Genome.String);
        Debug.Log("New: " + genome.String);
        this.Genome = genome;
        cultureRenderer.SetGenome(genome);
    }
}
