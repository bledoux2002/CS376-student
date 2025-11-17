using System;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    private GameObject Spawner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawner = GameObject.Find("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Taken from Angry Blox Bomb.cs
    public void Explode()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        
        Invoke("Destruct", 0.1f);
    }

    private void Destruct()
    {
        float key = MathF.Round(transform.position.y, 2);
        Spawner.GetComponent<Spawner>().spawnedMines.Remove(key);
        Destroy(gameObject);
    }
}
