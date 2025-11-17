using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private string horizontal_axis;
    private string vertical_axis;
    private KeyCode jump_button;
    private bool falling;
    public float speed;
    public float jump_force;
    
    private SpriteRenderer sr;
    private KeyCode kick_button;
    private float next_kick;
    private bool kicked;
    public float kick_frequency;
    public float kick_distance;
    public float kick_force;
    
    private GameObject ball;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        falling = true;
        
        sr = GetComponent<SpriteRenderer>();
        if (name == "Player 1")
        {
            horizontal_axis = "Horizontal1";
            vertical_axis = "Vertical1";
            jump_button = KeyCode.W;
            kick_button = KeyCode.C;
        } else if (name == "Player 2")
        {
            horizontal_axis = "Horizontal2";
            vertical_axis = "Vertical2";
            jump_button = KeyCode.UpArrow;
            kick_button = KeyCode.M;
        }

        kicked = false;
        next_kick = 0f;
        ball = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if (kicked && Time.time >= next_kick)
        {
            Color temp = sr.color;
            sr.color = new Color(temp.r + 0.25f, temp.g + 0.25f, temp.b + 0.25f, 1f);
            kicked = false;
        }
        Vector2 direction = Move(); //move player and return directional input for use in kicking
        Kick(direction);
    }

    private Vector2 Move()
    {
        Vector2 input = new Vector2(Input.GetAxis(horizontal_axis), Input.GetAxis(vertical_axis));

        float jump = 0f;
        if (Input.GetKeyDown(jump_button) && !falling)
        {
            jump = jump_force;
            falling = true;
        }
        
        rb.AddForce(new Vector2 (input.x, jump) * speed); //vertical movement from jumps only

        return input;
    }
    
    private void Kick(Vector2 direction)
    {
        // apply kick force to ball in direction of movement
        if (Input.GetKeyDown(kick_button) && Time.time >= next_kick)
        {
            // apply force in direction of movement to ball
            if (Vector2.Distance(transform.position, ball.transform.position) <= kick_distance)
            {
                Rigidbody2D ball_rb = ball.GetComponent<Rigidbody2D>();
                ball_rb.AddForce(direction * kick_force);
                next_kick = Time.time + kick_frequency;
                Color temp = sr.color;
                sr.color = new Color(temp.r - 0.25f, temp.g - 0.25f, temp.b - 0.25f, 1f);
                kicked = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            falling = false;
        }
    }

    public void Reset()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = new Vector3(6.5f * (name == "Player 1" ? -1f : 1f), -2.95f, 0f);
        next_kick = 0f;
    }
}
