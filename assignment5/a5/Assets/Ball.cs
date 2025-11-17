using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    private ScoreKeeper sk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sk = GameObject.Find("ScoreKeeper").GetComponent<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj.CompareTag("Goal 2"))
        {
            obj.GetComponent<AudioSource>().Play();
            sk.Score(1);
            Reset();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().Reset();
            }
        } else if (obj.CompareTag("Goal 1"))
        {
            obj.GetComponent<AudioSource>().Play();
            sk.Score(2);
            Reset();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerController>().Reset();
            }
        }
        
    }

    public void Reset()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = new Vector3(0f, 0f, -1f);
    }
}
