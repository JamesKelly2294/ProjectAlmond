using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Allele {
    public byte value;

    public static int AlleleStrength = 4; // Adjust range to allow stronger Alleles.

    public Allele(byte initialValue) {
        value = (byte)Mathf.Min(initialValue, Allele.AlleleStrength); 
    }

    public Allele(int initialValue) {
        value = (byte)Mathf.Min(Mathf.Max(initialValue, 0), Allele.AlleleStrength);
    }

    public void setValue(byte newValue) {
        value = (byte)Mathf.Min(newValue, Allele.AlleleStrength);
    }

    public void setValue(int newValue) {
        value = (byte)Mathf.Min(Mathf.Max(newValue, 0), Allele.AlleleStrength);
    }

    public void incrementValueByWrapping(int amountToAdd) { // Wraps.
        value = (byte)((amountToAdd + value + Allele.AlleleStrength) % Allele.AlleleStrength);
    }

    public char Char {
        get {
            return (char)(value + 65);
        }
    }
}

public class Genome {
    public Allele[] alleles;

    public Genome(Allele[] alleles) {
        this.alleles = alleles;
    }

    public Genome combine(Genome otherGenome, System.Random rand) {
        System.Diagnostics.Debug.Assert(this.alleles.Length != otherGenome.alleles.Length);

        Allele[] allels = this.alleles;
        for (var i = 0; i < this.alleles.Length; i++) {
            if (rand.Next(0, 1) == 1) {
                allels[i] = otherGenome.alleles[i];
            }
        }

        return new Genome(alleles);
    }

    public string String {
        get {
            string str = "";
            for (var i = 0; i < alleles.Length; i++) {
                str += alleles[i].Char;
            }

            return str;
        }
    }
}

public class CultureGenome: Genome {

    public static int Length = 8; // Adjusts the total number of expected Alleles.

    internal CultureGenome(Allele[] alleles): base(alleles) {
        System.Diagnostics.Debug.Assert(this.alleles.Length != CultureGenome.Length);
    }

    public CultureGenome(   Allele heatResistance, Allele aggression, Allele radiationResistance, Allele growRate, 
                            Allele coldResistance, Allele mobility, Allele size, Allele unused
                        ): base(new Allele[] { heatResistance, aggression, radiationResistance, growRate, coldResistance, mobility, size, unused }) {
        
    }

    public Allele heatResistance { get { return alleles[0]; } set { alleles[0] = value; } }
    public Allele aggression { get { return alleles[1]; } set { alleles[1] = value; } }
    public Allele radiationResistance { get { return alleles[2]; } set { alleles[2] = value; } }
    public Allele growRate { get { return alleles[3]; } set { alleles[3] = value; } }
    public Allele coldResistance { get { return alleles[4]; } set { alleles[4] = value; } }
    public Allele mobility { get { return alleles[5]; } set { alleles[5] = value; } }
    public Allele size { get { return alleles[6]; } set { alleles[6] = value; } }
    public Allele unused { get { return alleles[7]; } set { alleles[7] = value; } }

    /// Creates a color from the internal representation of the genome.
    public Color color { 
        get {
            double maxAlleleStrength = (Allele.AlleleStrength) * 10 + Allele.AlleleStrength;
            double r = (double)(((alleles[0].value) * 10) + (alleles[1].value)) / (double)(maxAlleleStrength);    
            double g = (double)(((alleles[2].value) * 10) + (alleles[3].value)) / (double)(maxAlleleStrength);    
            double b = (double)(((alleles[4].value) * 10) + (alleles[5].value)) / (double)(maxAlleleStrength);    
            double a = (double)(((alleles[6].value) * 10) + (alleles[7].value)) / (double)(maxAlleleStrength);            
            
            return new Color((float)r, (float)g, (float)b, (float)((a/2) + 0.5));
        }
     }

     public void mutate(System.Random rand) {
        for (var i = 0; i < this.alleles.Length; i++) {

            if ( rand.Next(100) <= 10 && rand.Next(100) % (this.radiationResistance.value + 1) == 0 ) {
                int mutation = rand.Next(-1, 1);
                this.alleles[i].incrementValueByWrapping(mutation);
            }
        }
    }

    public void apply(List<AlleleModifier> modifiers, System.Random rand) {
        foreach (var modifier in modifiers) {
            apply(modifier, rand);
        }
    }

    public void apply(AlleleModifier modifier, System.Random rand) {
        var current = alleles[modifier.type.GetHashCode()].value;
        var strength = 0;
        switch (modifier.strength)
        {
            case AlleleStrength.Strong: { strength = 1; break; }
            case AlleleStrength.Weak: { strength = 3; break; }
            case AlleleStrength.Random: { strength = rand.Next(0, 3); break; }
        }

        var effect = 0;
        switch (modifier.effect)
        {
            case AlleleEffect.Increase: { effect =  1; break; }
            case AlleleEffect.Decrease: { effect = -1; break; }
            case AlleleEffect.Random: { effect = rand.Next(-1, 1); break; }
        }

        var adjust = strength * effect;
        alleles[modifier.type.GetHashCode()].setValue((current + adjust));
    }
}