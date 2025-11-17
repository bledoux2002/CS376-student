using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Status
{
    WAITING,
    DROPPING,
    BURSTING,
    REELING
}

public class Hook : MonoBehaviour
{
    private InputAction moveAction; //old input manager interprets xbox and dualsense inputs differently, so I am using the new input system 
    private InputAction mainAction; //start and burst
    private InputAction buyAction;
    
    public float speed;
    public float dropSpeed;
    public float burstSpeed;
    public float burstTime;
    private float burstEnd;
    public float reelSpeed;
    
    private int money;
    private int bursts;
    public int burstPrice;
    private float depth;
    private float maxDepth;
    private float depthScale;
    private Status status;

    private List<GameObject> fish;
    private GameObject cam;
    private bool camLocked;

    public AudioSource splash;
    public AudioSource burst;
    
    [SerializeField]
    private TMPro.TMP_Text moneyText;
    [SerializeField]
    private TMPro.TMP_Text burstsText;
    [SerializeField]
    private TMPro.TMP_Text depthText;

    public GameObject Spawner;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        mainAction = InputSystem.actions.FindAction("Main");
        buyAction = InputSystem.actions.FindAction("Buy");
        
        money = 500;
        bursts = 3;
        depth = 0f;
        
        moneyText.text = "Fish Bucks: $" + money.ToString();
        burstsText.text = "Bursts ($500): " + bursts.ToString();
        depthText.text = "Depth: " + depth.ToString() + "m (Best " + maxDepth.ToString() + "m)";
        
        status = Status.WAITING;
        
        fish = new List<GameObject>();
        cam = GameObject.FindWithTag("MainCamera");
        camLocked = false;
        
        depthScale = Spawner.GetComponent<Spawner>().depthScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!camLocked && transform.position.y <= 0)
        {
            camLocked = true;
        } else if (camLocked && transform.position.y > 0)
        {
            camLocked = false;
        }
        
        if (status == Status.WAITING)
        {
            if (buyAction.WasPressedThisDynamicUpdate() && money >= burstPrice)
            {
                money -= burstPrice;
                bursts++;
                moneyText.text = $"Fish Bucks: ${money}"; 
                burstsText.text = $"Bursts ($500): {bursts}";
            }
        }
        if (Time.time >= burstEnd && status == Status.BURSTING) status = Status.DROPPING;
        if (mainAction.WasPressedThisDynamicUpdate())
        {
            if (status == Status.WAITING)
            {
                status = Status.DROPPING;
            } else if (status == Status.DROPPING && bursts > 0)
            {
                burstEnd = Time.time + burstTime;
                bursts--;
                burst.Play();
                burstsText.text = $"Bursts ($500): {bursts}";
                status = Status.BURSTING;
            }
        }

        Move();
        
        depth = MathF.Round((4f - transform.position.y) * depthScale, 2);
        maxDepth = (depth > maxDepth ? depth : maxDepth);
        depthText.text = $"Depth: {depth:F2}m (Best {maxDepth:F2}m)";
    }

    private void Move()
    {
        float h = moveAction.ReadValue<Vector2>().x * speed * Time.deltaTime;
        h = (transform.position.x + h > -9f && transform.position.x + h < 9f ? h : 0f);

        float v = 0f;
        if (status == Status.REELING)
        {
            v = -1f * reelSpeed * Time.deltaTime;
        } else if (status == Status.DROPPING)
        {
            v = dropSpeed * Time.deltaTime;
        } else if (status == Status.BURSTING)
        {
            v = burstSpeed * Time.deltaTime;
        }
        
        transform.Translate(v, h, 0f);
        // if (camLocked) cam.transform.position = new Vector3(0f, transform.position.y, -10f);
        if (camLocked) cam.transform.Translate(0f, -v, 0f);
        // If reeling in and the top is reached, destroy all caught fish
        if (status == Status.REELING && transform.position.y > 4f)
        {
            status = Status.WAITING;
            splash.Play();
            transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
            moneyText.text = $"Fish Bucks: ${money}";
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;

            foreach (GameObject f in fish)
            {
                Destroy(f);
            }
            
            Spawner.GetComponent<Spawner>().ClearSpawner();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mine"))
        {
            if (status != Status.REELING)
            {
                other.gameObject.GetComponent<Mine>().Explode();
                // gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<CapsuleCollider2D>().enabled = false; // cant catch any fish on the way up
                status = Status.REELING;
            }
        } else if (other.CompareTag("Fish"))
        {
            if (status == Status.BURSTING)
            {
                other.gameObject.GetComponent<Fish>().Explode();
            }
            else
            {
                fish.Add(other.gameObject);
                int value = other.gameObject.GetComponent<Fish>().Catch(transform);
                money += value;
                status = Status.REELING;
            }
        }
    }

}
