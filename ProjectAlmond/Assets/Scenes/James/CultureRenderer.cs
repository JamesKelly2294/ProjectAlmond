using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CultureRenderer : MonoBehaviour
{
    public Material cellMaterial;
    public Material dishMaterial;

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

    [Range(0.0f, 1.0f)]
    public float growth = 0.5f;

    GameObject dish;
    GameObject culture;
    List<GameObject> cells;
    
    float petriDishRadius = 1.0f; // assume a circular petri dish
    float petriDishHeight = 0.05f;
    float cellRadius = 0.1f;
    List<Vector3> cellPositions;
    List<float> cellRadii;

    // Start is called before the first frame update
    void Start()
    {
        dish = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        dish.transform.parent = transform;
        dish.transform.localScale = new Vector3(petriDishRadius * 2.0f, petriDishHeight, petriDishRadius * 2.0f);
        dish.transform.localPosition = Vector3.zero;
        dish.GetComponent<MeshRenderer>().material = dishMaterial;
        dish.name = "Dish";

        GenerateCulture();
    }

    private void FixedUpdate()
    {
        culture.transform.localScale += 0.00025f * new Vector3(Mathf.Sin(Time.time), Mathf.Sin(Time.time), Mathf.Cos(Time.time));
    }

    void GenerateRadialCulture()
    {
        //int rings = 12;
        //int samples = 1;
        //for (int r = 0; r < rings; r++)
        //{
        //    float radius = (r / (float)rings) * petriDishRadius;
        //    for (int theta = 0; theta < samples; theta++)
        //    {
        //        float radians = (theta / (float)samples) * (Mathf.PI * 2.0f);
        //        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //        cell.transform.position = new Vector3(radius * Mathf.Cos(radians), petriDishHeight, radius * Mathf.Sin(radians));
        //        cell.transform.parent = culture.transform;
        //        cell.GetComponent<MeshRenderer>().material = cellMaterial;
        //        cells.Add(cell);
        //    }

        //    if(r % 2 == 0)
        //    {
        //        samples += 3;
        //    }
        //    else
        //    {
        //        samples += 2;
        //    }
        //}
    }

    void GenerateCulture()
    {
        Random.InitState(seed);

        if (!culture)
        {
            culture = new GameObject
            {
                name = "Culture"
            };
            culture.transform.parent = transform;
            culture.transform.localPosition = Vector3.zero;

            cells = new List<GameObject>();

            for (int i = 0; i < 200; i++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                cell.transform.parent = culture.transform;
                cell.GetComponent<MeshRenderer>().material = cellMaterial;
                cell.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                cells.Add(cell);
            }
        }

        foreach (var cell in cells)
        {

            float maxRange = petriDishRadius - (cellRadius / 2.0f);
            Vector2 randPosition = Vector2.zero;
            do
            {
                randPosition = new Vector2(Random.Range(-maxRange, maxRange), Random.Range(-maxRange, maxRange));
            } while (randPosition.magnitude >= maxRange);
            cell.transform.localPosition = new Vector3(randPosition.x, petriDishHeight, randPosition.y);
        }

        cellPositions = (from cell in cells
                                       select cell.transform.localPosition).ToList();


        cellRadii = PerlinNoise(cellPositions, seed, scale, octaves, persistance, lacunarity, offset);
        cellRadii = (from radius in cellRadii
                     select (radius + 0.5f) * 0.75f).ToList();

        CalculateCellSize();
    }

    void CalculateCellSize()
    {

        for (int i = 0; i < cells.Count; i++)
        {
            GameObject cell = cells[i];
            cell.transform.parent = null;
            float cellRadiusModifier = cellRadii[i] + (0.15f * Mathf.Sin(Time.time + cell.transform.localPosition.x + cell.transform.localPosition.y));
            Vector3 cellScale = new Vector3(cellRadius * cellRadiusModifier, cellRadius * cellRadiusModifier, cellRadius * cellRadiusModifier);
            cell.transform.localScale = cellScale;
            cell.transform.parent = culture.transform;
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
}
