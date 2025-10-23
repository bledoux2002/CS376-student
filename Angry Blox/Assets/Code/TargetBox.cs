using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TargetBox : MonoBehaviour
{
    /// <summary>
    /// Targets that move past this point score automatically.
    /// </summary>
    public static float OffScreen;

    internal void Start() {
        OffScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width-100, 0, 0)).x;
    }

    internal void Update()
    {
        if (transform.position.x > OffScreen)
            Scored();
    }

    private void Scored()
    {
        if (gameObject.GetComponent<SpriteRenderer>().color != Color.green)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            ScoreKeeper.AddToScore(gameObject.GetComponent<Rigidbody2D>().mass);
        }
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ground")
        {
            Scored();
        }
    }
}
