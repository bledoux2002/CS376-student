using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public float minSpacing;
    public float maxSpacing;
    private float nextPosition;
    
    // I'd prefer to keep these public and serialized in the editor.
    // More effort than necessary to serialize dictionaries, and our
    // specific Unity version (6000.2.6f1) has a bug where lists and
    // arrays aren't displayed properly, so I have to serialize them
    // separately.
    public GameObject tinyPrefab;
    public GameObject smallPrefab;
    public GameObject mediumPrefab;
    public GameObject largePrefab;
    public GameObject giantPrefab;
    public GameObject thePrefab;
    public GameObject minePrefab;
    public float tinySpawnRate;
    public float smallSpawnRate;
    public float mediumSpawnRate;
    public float largeSpawnRate;
    public float giantSpawnRate;
    public float theSpawnRate;
    public float mineSpawnRate;
    [HideInInspector]
    public Dictionary<float, GameObject> spawnedFish;
    [HideInInspector]
    public Dictionary<float, GameObject> spawnedMines;

    public float depthScale;
    public float maxDepth;
    private SpriteRenderer waterSprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float totalSpawnRate = tinySpawnRate + smallSpawnRate + mediumSpawnRate + largeSpawnRate + giantSpawnRate + theSpawnRate + mineSpawnRate;
        // Debug.Assert(totalSpawnRate == 1f, $"Spawn rates must add up to 1f, only add up to {totalSpawnRate}");
        nextPosition = transform.position.y;
        spawnedFish = new Dictionary<float, GameObject>();
        spawnedMines = new Dictionary<float, GameObject>();
        waterSprite = GameObject.Find("Water").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float colorShift = (maxDepth + (transform.parent.position.y * depthScale)) / maxDepth;
        waterSprite.color = new Color(0f, colorShift, colorShift);

        if (transform.position.y <= nextPosition)
        {
            float spacing = Random.Range(minSpacing, maxSpacing);
            nextPosition = transform.position.y - spacing;

            Spawn();
        }
    }
    
    // use rounded to 2 decimals for the key of each object, then remove it from dict when gameobject is destroyed
    // only destroy fish not caught, cuaght are handled by hook.
    // fish willremove itself from dict when destroyed by burst or caught by hook
    // mine will remove itself from dict when exploded
    
    private void Spawn()
    {
        float decider = Random.Range(0f, 1f);

        if (decider > 1f - mineSpawnRate)
        {
            Vector3 pos = new Vector3(Random.Range(-10f, 10f), transform.position.y, -0.75f);
            GameObject mine = Instantiate(minePrefab, pos, Quaternion.identity);
            float key = MathF.Round(mine.transform.position.y, 2);
            spawnedMines[key] = mine;
        } else if (decider < tinySpawnRate)
        {
            SpawnFish(tinyPrefab, -0.5f);
        } else if (decider < tinySpawnRate + smallSpawnRate)
        {
            SpawnFish(smallPrefab, -0.25f);
        } else if (decider < tinySpawnRate + smallSpawnRate + mediumSpawnRate)
        {
            SpawnFish(mediumPrefab, 0f);
        } else if (decider < tinySpawnRate + smallSpawnRate + mediumSpawnRate + largeSpawnRate)
        {
            SpawnFish(largePrefab, 0.25f);
        } else if (decider < tinySpawnRate + smallSpawnRate + mediumSpawnRate + largeSpawnRate + giantSpawnRate)
        {
            SpawnFish(giantPrefab, 0.5f);
        } else if (decider < tinySpawnRate + smallSpawnRate + mediumSpawnRate + largeSpawnRate + giantSpawnRate + theSpawnRate)
        {
            SpawnFish(thePrefab, 0.75f);
        }
    }

    private void SpawnFish(GameObject prefab, float z)
    {
        Vector3 pos = new Vector3(Random.Range(-10f, 10f), transform.position.y, z);
        GameObject fish = Instantiate(prefab, pos, Quaternion.Euler(0f, 0f, 90f));
        float key = MathF.Round(fish.transform.position.y, 2);
        spawnedFish[key] = fish;
    }

    public void ClearSpawner()
    {
        foreach (GameObject fish in spawnedFish.Values)
        {
            Destroy(fish);
        }
        spawnedFish.Clear();

        foreach (GameObject mine in spawnedMines.Values)
        {
            Destroy(mine);
        }
        spawnedMines.Clear();
        
        nextPosition = transform.position.y;
    }
}
