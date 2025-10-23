using UnityEngine;

public class Bomb : MonoBehaviour {
    public float ThresholdImpulse = 5;
    public GameObject ExplosionPrefab;

    public void OnCollisionEnter2D(Collision2D collision) {
        foreach (var contact in collision.contacts)
        {
            if (contact.normalImpulse > ThresholdImpulse)
            {
                Boom();
            }
        }
    }

    private void Destruct()
    {
        Destroy(gameObject);
    }

    private void Boom()
    {
        gameObject.GetComponent<PointEffector2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity, transform.parent);
        Invoke("Destruct", 0.1f);
    }
}
