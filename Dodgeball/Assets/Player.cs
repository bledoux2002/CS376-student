using UnityEngine;

/// <summary>
/// Control the player on screen
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Prefab for the orbs we will shoot
    /// </summary>
    public GameObject OrbPrefab;

    /// <summary>
    /// How fast our engines can accelerate us
    /// </summary>
    public float EnginePower = 1;

    /// <summary>
    /// rigidbody component
    /// </summary>
    public Rigidbody2D RigidBody;
    
    /// <summary>
    /// How fast we turn in place
    /// </summary>
    public float RotateSpeed = 1;

    /// <summary>
    /// How fast we should shoot our orbs
    /// </summary>
    public float OrbVelocity = 10;

    /// <summary>
    /// Keep track of calls to MaybeFire
    /// </summary>
    public int fire = 0;

    /// <summary>
    /// Start method to initialize rigidbody
    /// </summary>
    void Start()
    {
        RigidBody =  GetComponent<Rigidbody2D>();
    }
    
    /// <summary>
    /// Handle moving and firing.
    /// Called by Uniity every 1/50th of a second, regardless of the graphics card's frame rate
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void FixedUpdate()
    {
        Manoeuvre();
        MaybeFire();
    }

    /// <summary>
    /// Fire if the player is pushing the button for the Fire axis
    /// However, only fire every other call to MaybeFire().  We don't care
    /// whether you only fire on odd numbered calls or even numbered calls.
    /// </summary>
    void MaybeFire()
    {
        // TODO
        if (Input.GetAxis("Fire") > 0)
        {
            if (fire % 2 == 0)
            {
                FireOrb();
            }
            fire++;

        }
    }

    /// <summary>
    /// Fire one orb.  The orb should be placed one unit "in front" of the player.
    /// transform.right will give us a vector in the direction the player is facing.
    /// It should move in the same direction (transform.right), but at speed OrbVelocity.
    /// </summary>
    private void FireOrb()
    {
        // TODO
        Vector3 playerDirection = transform.right.normalized;
        Vector3 playerPosition = transform.position;
        Vector3 orbPosition = playerPosition + playerDirection;
        GameObject orb = Instantiate(OrbPrefab, orbPosition, Quaternion.identity);
        Rigidbody2D orbBody = orb.GetComponent<Rigidbody2D>();
        
        Vector2 force = new Vector2(playerDirection.x, playerDirection.y);
        orbBody.linearVelocity = force * OrbVelocity;
        
    }

    /// <summary>
    /// Accelerate and rotate as directed by the player
    /// Apply a force in the direction (Horizontal, Vertical) with magnitude EnginePower
    /// Note that this is in *world* coordinates, so the direction of our thrust doesn't change as we rotate
    /// Set our angularVelocity to the Rotate axis time RotateSpeed
    /// </summary>
    void Manoeuvre()
    {
        // TODO
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = input * EnginePower;
        RigidBody.AddForce(input);
        var inputRotate =  Input.GetAxis("Rotate");
        inputRotate = inputRotate * RotateSpeed;
        RigidBody.angularVelocity = inputRotate;
    }

    /// <summary>
    /// If this is called, we got knocked off screen.  Deduct a point!
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        ScoreKeeper.ScorePoints(-1);
    }
}
