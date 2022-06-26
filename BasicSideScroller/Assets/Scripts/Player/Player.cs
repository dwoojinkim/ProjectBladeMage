using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Remove this once I no longer need the debug text

public class Player : MonoBehaviour
{
    private const float MAX_SPEED_CAP = 25.0f;
    private const float INIT_CAMERA_MOVESPEED = 5.0f;

    public GameObject DebugTextObj;
    public GameObject SlashHitbox;
    public GameObject SmashHitbox;

    public float jumpVelocity = 7.5f;
    public float jumpGravity = 1f;
    public float normalGravity = 10f;
    public float extraGravity = 20f;

    public int Health {get; private set;}

    private Rigidbody2D playerRigidbody;

    private Text debugText;
    private bool jumping = true;
    private bool slashRequest = false;
    private bool slashing = false;
    private bool smashRequest = false;
    private bool smashing = false;
    private float hitboxStartup = 0.05f;
    private float hitboxDuration = 0.05f;
    private float attackCooldown = 1f;
    private float attackTimer = 0f;
    private float playerMovespeedValue = 10; // Magnitude of speed when left/right is input
    private float playerMovespeed = 0;      // Actual player's speed in reference to the camera
    private float cameraMovespeed = INIT_CAMERA_MOVESPEED;
    private float timeToIncreaseSpeed = 1.0f;
    private float speedIncreaseTimer = 0.0f;
    private float increaseSpeedAmount = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if (DebugTextObj != null)
        {
            debugText = DebugTextObj.GetComponent<Text>();
        }
        
        playerRigidbody = this.GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = normalGravity;

        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        SlashCheck();
        SmashCheck();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            jumping = false;
        }
        else if (col.gameObject.tag == "Enemy" && col.enabled)
        {
            Debug.Log("PLAYER HAS BEEN HIT BY ENEMY");
                Health -= 10;
        }
    }
    

    public void SetDebugText(string text)
    {
        debugText.text = text;
    }

    //Calculates the total movement for the player (Global stage movement + local stage movement)
    public void MovePlayer()
    {
        transform.position += transform.right * playerMovespeed * Time.fixedDeltaTime;
    }

    public void Move(float direction)
    {
        if (direction < 0)      //Move left
            playerMovespeed = -playerMovespeedValue;
        else if (direction > 0) // Move right
            playerMovespeed = playerMovespeedValue;
        else
            playerMovespeed = 0;
    }

    public void Jump()
    {
        SetDebugText("Jumping");

        //If not already in a jumping state, then do below
        if (!jumping)
        {
            playerRigidbody.velocity = transform.up * jumpVelocity;
            playerRigidbody.gravityScale = jumpGravity;
            jumping = true;
        }

    }

    public void ReleaseJump()
    {
        playerRigidbody.gravityScale = normalGravity;
    }

    public void Down()
    {
        playerRigidbody.gravityScale = extraGravity;
    }
    public void Crouch()
    {

    }

    public void Slash()
    {
        if (!slashing && !smashing)
            slashRequest = true;
    }

    private void SlashCheck()
    {
        if (slashRequest)
        {  
            if (attackTimer >= hitboxStartup)
            {
                slashing = true;
                slashRequest = false;

                SlashHitbox.GetComponent<SpriteRenderer>().enabled = true;
                SlashHitbox.GetComponent<BoxCollider2D>().enabled = true;
                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
        }
        if (slashing)
        {
            if (attackTimer >= hitboxDuration)
            {
                slashing = false;
                SlashHitbox.GetComponent<SpriteRenderer>().enabled = false;
                SlashHitbox.GetComponent<BoxCollider2D>().enabled = false;
                attackTimer = 0f;
            }

            attackTimer += Time.deltaTime;
        }
    }

    public void Smash()
    {
        if (!smashing && !slashing)
            smashRequest = true;
    }

    public void SmashCheck()
    {
        if (smashRequest)
        {  
            if (attackTimer >= hitboxStartup)
            {
                smashing = true;
                smashRequest = false;

                SmashHitbox.GetComponent<SpriteRenderer>().enabled = true;
                SmashHitbox.GetComponent<BoxCollider2D>().enabled = true;
                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
        }
        if (smashing)
        {
            if (attackTimer >= hitboxDuration)
            {
                smashing = false;
                SmashHitbox.GetComponent<SpriteRenderer>().enabled = false;
                SmashHitbox.GetComponent<BoxCollider2D>().enabled = false;
                attackTimer = 0f;
            }

            attackTimer += Time.deltaTime;
        }
    }

    public void Kill()
    {
        speedIncreaseTimer = 0.0f;
        playerMovespeed = 0;
        LevelManager.LMinstance.ResetGame();
    }
}
