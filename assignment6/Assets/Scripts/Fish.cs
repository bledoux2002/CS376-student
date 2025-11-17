using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{

    public int value;
    public float minSpeed;
    public float maxSpeed;
    private float speed;
    private int direction;
    private bool caught;

    private GameObject Spawner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        direction = Random.Range(0, 2);
        if (direction == 0) direction--;
        caught = false;

        Spawner = GameObject.Find("Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (!caught)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime * direction); // Prefab is rotated so translate up instead of right
        if (transform.position.x <= -10f || transform.position.x >= 10f)
        {
            direction = -direction;
            transform.position = new Vector3(10f * direction, transform.position.y, transform.position.z);
        }
    }

    public int Catch(UnityEngine.Transform parent)
    {
        caught = true;
        GetComponent<AudioSource>().Play();
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false; //leaving this enabled counts it towards hook collider
        float key = MathF.Round(transform.position.y, 2);
        Spawner.GetComponent<Spawner>().spawnedMines.Remove(key);
        transform.parent = parent;
        transform.position = new Vector3(parent.position.x, parent.position.y, transform.position.z);
        return value;
    }

    public void Explode()
    {
        float key = MathF.Round(transform.position.y, 2);
        Spawner.GetComponent<Spawner>().spawnedMines.Remove(key);
        Destroy(gameObject);
    }
}
