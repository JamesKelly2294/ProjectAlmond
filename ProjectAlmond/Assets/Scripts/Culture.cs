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

    public bool shouldInitializeGenome = true;

    TMPro.TextMeshPro textMeshPro;
    
    void Awake()
    {
        cultureRenderer = GetComponent<CultureRenderer>();
        if (shouldInitializeGenome)
        {
            System.Random rand = new System.Random();

            List<Allele> alleles = new List<Allele>(CultureGenome.Length);

            for (int i = 0; i < alleles.Capacity; i++)
            {
                alleles.Add(new Allele(rand.Next(0, Allele.AlleleStrength)));
            }

            Genome = new CultureGenome(alleles.ToArray());
            cultureRenderer.Initialize(Genome);
        }


    }

    void Start()
    {
        textMeshPro = cultureRenderer.GetComponentInChildren<TMPro.TextMeshPro>();
        if (textMeshPro)
        {
            textMeshPro.text = dishLabel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (textMeshPro)
        {
            textMeshPro.text = dishLabel;
        }
    }

    public void SetGenome(CultureGenome genome)
    {
        if (Genome != null)
        {
            Debug.Log("Current: " + Genome.String);
        }
        Debug.Log("New: " + genome.String);
        this.Genome = genome;
        cultureRenderer.SetGenome(genome);
    }
}
