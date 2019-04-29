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
    public TextMeshPro priceLabel;

    CoinDropper coinDropper;

    // Start is called before the first frame update
    void Start()
    {
        reagentRenderer.material = new Material(reagentRenderer.material);
        coinDropper = FindObjectOfType<GameManager>().coinDropper;
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
        priceLabel.text = reagentData.price.ToString();
    }

    void OnValidate()
    {
        reagentData.OnValidation.AddListener(OnReagentValidation);
    }

    public GameObject PluckedReagent()
    {
        if (coinDropper.numberOfCoinsVisable < reagentData.price)
        {
            // TODO: play sound here
            return null;
        }

        GameObject plucked = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        plucked.GetComponent<MeshRenderer>().material = reagentRenderer.material;
        Draggable draggable = plucked.AddComponent<Draggable>();
        draggable.Data = reagentData;
        draggable.draggableType = DraggableType.PluckedReagent;
        draggable.DiesOnRelease = true;
        plucked.transform.name = reagentData.name + " Sample";
        plucked.transform.localScale = Vector3.one * 0.5f;

        return plucked;
    }
}
