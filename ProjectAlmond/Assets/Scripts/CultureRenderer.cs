using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CultureRenderer : MonoBehaviour
{
    public Material cellMaterialBase;
    public Material dishMaterial;

    public GameObject dishPrefab;

    public int seed = 0;
    [Range(0.0f, 1.0f)]
    public float scale = 1.0f;
    [Range(1, 8)]
    public int octaves = 4;
    [Range(0.0f, 1.0f)]
    public float persistance = 1.0f;
    [Range(0.0f, 1.0f)]
    public float lacunarity = 1.0f;
    public Vector2 offset = new Vector2(0.0f, 0.0f);

    float _growth = 0.5f;
    float growthFudgeFactor = 0.25f;
    public float Growth
    {
        get
        {
            return _growth;
        }
        set
        {
            _growth = value;
            float threshold = (1 - _growth) + growthFudgeFactor;
            for (int i = 0; i < cells.Count; i++)
            {
                if (cellRadii[i] < threshold)
                {
                    cells[i].SetActive(false);
                } else
                {
                    cells[i].SetActive(true);
                }
            }
            CalculateCellSize();
        }
    }

    [Range(1, 500)]
    public int cellCount;

    [Range(1, 20)]
    public int cellGroupCount;

    GameObject petriDishContainer;
    GameObject dish;
    GameObject culture;
    List<GameObject> cellGroups;
    List<GameObject> cells;
    
    float petriDishRadius = 1.0f; // assume a circular petri dish
    float petriDishHeight = 0.05f;
    float cellRadius = 0.1f;
    List<Vector3> cellPositions;
    List<float> cellRadii;

    List<Material> cellMaterials;

    int randomOffset;

    bool initalized;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Initialize(CultureGenome genome)
    {
        if(initalized)
        {
            return;
        }

        Debug.Log("Initializing culture renderer");

        initalized = true;

        petriDishContainer = new GameObject
        {
            name = "Petri Dish Container"
        };
        petriDishContainer.transform.parent = transform;
        petriDishContainer.transform.localPosition = Vector3.zero;
        petriDishContainer.transform.rotation = transform.rotation;

        dish = Instantiate(dishPrefab);
        dish.transform.parent = transform;
        dish.transform.localScale = new Vector3(petriDishRadius * 2.0f, petriDishHeight * 2, petriDishRadius * 2.0f);
        dish.transform.localPosition = Vector3.zero;
        dish.transform.rotation = transform.rotation;
        dish.GetComponent<MeshRenderer>().material = dishMaterial;
        dish.name = "Dish";

        randomOffset = Random.Range(0, 250000);

        SetGenome(genome);
    }


    //static int counter = 0;
    private void FixedUpdate()
    {
        if(!initalized)
        {
            return;
        }

        //counter++;
        //if (counter % 4 == 0)
        //{
        //    Growth = Mathf.PingPong(Time.time / 4.0f, 1.0f);
        //}

        for (int i = 0; i < cellGroups.Count; i++)
        {
            float offset = i / (float)cellGroups.Count * 25000 + randomOffset;

            if(i % 2 == 0)
            {
                cellGroups[i].transform.localScale += 0.00085f *
                    new Vector3(Mathf.Sin(Time.time + offset), Mathf.Sin(Time.time + offset), Mathf.Cos(Time.time + offset));
            }
            else
            {
                cellGroups[i].transform.localScale += 0.00085f *
                    new Vector3(Mathf.Cos(Time.time + offset), Mathf.Cos(Time.time + offset), Mathf.Sin(Time.time + offset));
            }
        }
    }

    void GenerateCulture()
    {

        if (!culture)
        {
            culture = new GameObject
            {
                name = "Culture"
            };
            culture.transform.parent = petriDishContainer.transform;
            culture.transform.localPosition = Vector3.zero;
            culture.transform.localRotation = Quaternion.identity;

            cellGroups = new List<GameObject>();
            for (int i = 0; i < cellGroupCount; i++)
            {
                GameObject cellGroup = new GameObject {
                    name = "Cell Group"
                };

                cellGroup.transform.parent = culture.transform;
                cellGroup.transform.localPosition = Vector3.zero;
                cellGroup.transform.localRotation = Quaternion.identity;
                cellGroups.Add(cellGroup);
            }

            cells = new List<GameObject>();
            for (int i = 0; i < cellCount; i++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                cell.transform.parent = cellGroups[Random.Range(0, cellGroups.Count)].transform;
                cell.transform.localRotation = Quaternion.identity;
                Destroy(cell.GetComponent<SphereCollider>());
                cell.GetComponent<MeshRenderer>().material = cellMaterials[Random.Range(0, cellMaterials.Count)];
                cell.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                cells.Add(cell);
            }
        }

        foreach (var cell in cells)
        {

            float maxRange = petriDishRadius - (cellRadius);
            Vector2 randPosition = Vector2.zero;
            do
            {
                randPosition = new Vector2(Random.Range(-maxRange, maxRange), Random.Range(-maxRange, maxRange));
            } while (randPosition.magnitude >= maxRange);
            cell.transform.localPosition = new Vector3(randPosition.x, 0, randPosition.y);
        }

        cellPositions = (from cell in cells
                                       select cell.transform.localPosition).ToList();

        int seed = Random.Range(0, int.MaxValue);
        cellRadii = PerlinNoise(cellPositions, seed, scale, octaves, persistance, lacunarity, offset);
        cellRadii = (from radius in cellRadii
                     select (radius + 0.5f) * 0.75f).ToList();

        CalculateCellSize();
    }

    void CalculateCellSize()
    {
        float sqrtGrowth = Mathf.Sqrt(_growth);
        for (int i = 0; i < cells.Count; i++)
        {
            GameObject cell = cells[i];
            Transform oldParent = cell.transform.parent;
            cell.transform.parent = null;
            //float cellRadiusModifier = Mathf.Max(0.7f, 1.25f * sqrtGrowth) * cellRadii[i] + (0.15f * Mathf.Sin(Time.time + cell.transform.localPosition.x + cell.transform.localPosition.y));
            float cellRadiusModifier = _growth * cellRadii[i] + 0.15f; // + (0.15f * Mathf.Sin(Time.time + cell.transform.localPosition.x + cell.transform.localPosition.y));
            Vector3 cellScale = new Vector3(cellRadius * cellRadiusModifier, cellRadius * cellRadiusModifier, cellRadius * cellRadiusModifier);
            cell.transform.localScale = cellScale;
            cell.transform.parent = oldParent;
            cell.transform.localPosition = cellPositions[i] + new Vector3(0.0f, cellScale.y / 2.0f, 0.0f);
        }
    }

    // inspired by code from Sebastian Lague
    public List<float> PerlinNoise(List<Vector3> positions, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }
        

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        List<float> noise = new List<float>(positions.Count);

        foreach (var position in positions) {
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for (int i = 0; i < octaves; i++)
            {
                float sampleX = position.x / scale * frequency + octaveOffsets[i].x;
                float sampleY = position.z / scale * frequency + octaveOffsets[i].y;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseHeight += perlinValue * amplitude;

                amplitude *= persistance;
                frequency *= lacunarity;
            }

            if (noiseHeight > maxNoiseHeight)
            {
                maxNoiseHeight = noiseHeight;
            }
            else if (noiseHeight < minNoiseHeight)
            {
                minNoiseHeight = noiseHeight;
            }

            noise.Add(Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseHeight));
        }

        return noise;
    }

    public void RecalculateCulture()
    {
        GenerateCulture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGenome(CultureGenome genome)
    {
        Debug.Log("Setting cell genome");

        Color baseColor = genome.color;
        cellMaterials = new List<Material>(cellGroupCount);
        Debug.Log(baseColor);
        for (int i = 0; i < cellGroupCount; i++)
        {
            float H, S, V;
            Color.RGBToHSV(baseColor, out H, out S, out V);
            H += Random.Range(-0.1f, 0.1f);
            Color alteredColor = Color.HSVToRGB(H, S, V);
            Color finalColor = alteredColor * Mathf.LinearToGammaSpace(Random.Range(0.25f, 0.35f));
            Material cellGroupMaterial = new Material(cellMaterialBase);
            cellGroupMaterial.SetColor("_EmissionColor", finalColor);

            cellMaterials.Add(cellGroupMaterial);
        }

        Initialize(genome);
        GenerateCulture();
        Growth = 1.0f;
    }
}
