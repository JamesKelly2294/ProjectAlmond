using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReagentBehavior : MonoBehaviour
{
    public ReagentData reagentData;

    public MeshRenderer reagentRenderer;
    public TextMeshPro productNameLabel;
    public TextMeshPro flavorTextLabel;

    // Start is called before the first frame update
    void Start()
    {
        reagentRenderer.material = new Material(reagentRenderer.material);
        OnReagentValidation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnReagentValidation()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        Color finalColor = reagentData.color * Mathf.LinearToGammaSpace(reagentData.emissiveness);
        reagentRenderer.sharedMaterial.SetColor("_EmissionColor", finalColor);

        reagentRenderer.sharedMaterial.color = reagentData.color;
        productNameLabel.text = reagentData.productName;
        flavorTextLabel.text = reagentData.flavorText;
    }

    void OnValidate()
    {
        reagentData.OnValidation.AddListener(OnReagentValidation);
    }
}
