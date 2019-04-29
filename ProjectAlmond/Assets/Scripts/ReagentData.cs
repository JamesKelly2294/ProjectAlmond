using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AlleleType
{
    HeatResistance = 0,
    Aggression,
    RadiationResistance,
    GrowRate,
    ColdResistance,
    Mobility,
    Size,
    Unused
}

public enum AlleleEffect
{
    Increase,
    Decrease,
    Random
}

public enum AlleleStrength
{
    Strong,
    Weak,
    Random
}

[System.Serializable]
public struct AlleleModifier
{
    public AlleleType type;
    public AlleleEffect effect;
    public AlleleStrength strength;

    public AlleleModifier(AlleleType type, AlleleEffect effect, AlleleStrength strength) {
        this.type = type;
        this.effect = effect;
        this.strength = strength;
    }
}

[CreateAssetMenu(fileName = "Reagent", menuName = "Reagent", order = 1)]
public class ReagentData : ScriptableObject
{
    public string productName = "Reagent A";
    public string flavorText = "Yeet";
    public Color color = Color.red;
    public float emissiveness = 0.1f;
    public List<AlleleModifier> modifiers;

    [HideInInspector]
    public UnityEvent OnValidation;

    public void OnValidate()
    {
        OnValidation.Invoke();
    }
}