using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Remove this once I no longer need the debug text
using DG.Tweening;

public class Player : MonoBehaviour
{
    private const float MAX_SPEED_CAP = 25.0f;
    private const float INIT_CAMERA_MOVESPEED = 5.0f;
    private const float MAX_FALLING_SPEED = -50.0f;

    public FloatPositionVariable playerPositionTracker;

    public GameObject DebugTextObj;
    public GameObject SlashHitbox;
    public GameObject SmashHitbox;
    public FloatVariable playerHP;
    public FloatVariable playerMaxHP;
    public FloatVariable playerMP;
    public FloatVariable playerMaxMP;
    public GameObjectReference playerObject;
    public Animator playerAnimator;

    public float jumpVelocity = 7.5f;
    public float jumpGravity = 1f;
    public float normalGravity = 10f;
    public float extraGravity = 20f;

    public float HP {get; private set;}
    public float MP {get; private set;}
    public int FaceDirection {get; private set;}
    public int BuffStacks {get; private set;}

    private Rigidbody2D playerRigidbody;
    private SpriteRenderer playerSprite;

    private int baseMaxHP = 100;    // Base Max HP before any mods to it via additional levels, upgrades, etc.
    private int baseMaxMP = 100;
    private int maxHP;              // True Max HP after all mods via level,s buffs/debuffs, upgrades, etc. But so far, just using this since those mechanics haven't been added yet.
    private int maxMP;
    private int hpRegenRate = 1;
    private int mpRegenRate = 5;
    private int maxBuffStacks = 5;
    private Text debugText;
    private bool jumping = true;
    private bool slashRequest = false;
    private bool slashing = false;
    private bool smashRequest = false;
    private bool smashing = false;
    private bool inPortal = false;
    private bool hitInvulnerability = false;
    private float hitInvulnerabilityDuration = 1f;
    private float hitInvulnerabilityTimer = 0f;
    private float hitboxStartup = 0.05f;
    private float hitboxDuration = 0.05f;
    private float attackCooldown = 1f;
    private float attackTimer = 0f;
    private float playerMovespeedValue = 10; // Magnitude of speed when left/right is input
    private float playerMovespeed = 0;      // Actual player's speed
    private float cameraMovespeed = INIT_CAMERA_MOVESPEED;
    private float timeToIncreaseSpeed = 1.0f;
    private float speedIncreaseTimer = 0.0f;
    private float increaseSpeedAmount = 1.0f;

    private Ricochet ricochetAbility;


    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        if (DebugTextObj != null)
        {
            debugText = DebugTextObj.GetComponent<Text>();
        }
        
        playerRigidbody = this.GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerRigidbody.gravityScale = normalGravity;
        playerObject.SetGameObject(this.gameObject);
        if (GetComponent<Ricochet>() != null)
            ricochetAbility = GetComponent<Ricochet>();

        maxHP = baseMaxHP;
        maxMP = baseMaxMP;
        HP = maxHP;
        MP = maxMP;
        FaceDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        playerPositionTracker.SetVector3Value(transform.position);
        playerHP.SetValue(HP);
        playerMaxHP.SetValue(maxHP);
        playerMP.SetValue(MP);
        playerMaxMP.SetValue(maxMP);

        playerAnimator.SetFloat("Speed", Mathf.Abs(playerMovespeed));
        playerAnimator.SetFloat("FallSpeed", playerRigidbody.velocity.y);
        
        SlashCheck();
        SmashCheck();

        PlayerRegen();

        CheckPlayerInvulnerability();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Platform")
        {
            if (transform.position.y > col.transform.position.y)
            {
                jumping = false;
                playerAnimator.SetBool("OnGround", true);
            }
        }
        // Added a trigger hitbox to enemies to detect when they hurt the player.
        //else if (col.gameObject.tag == "Enemy" && col.enabled)
        //{
        //    Debug.Log("PLAYER HAS BEEN HIT BY ENEMY");
        //    DamagePlayer(col.gameObject.GetComponent<Enemy>().Damage); // GetComponent call is bad okay? Find another method to extract this data?
        //}
    }
    
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Projectile" && obj.GetComponent<Projectile>().Owner == "Enemy")
        {
            if (HP > 0)
                DamagePlayer(obj.GetComponent<Projectile>().Damage); // GetComponent call is bad okay? Find another method to extract this data?
        }
        else if (obj.gameObject.tag == "Portal")
        {
            inPortal = true;
            Debug.Log("Player is at " + obj.gameObject.name);
        }
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        if (!hitInvulnerability && obj.gameObject.tag == "EnemyHitbox" && obj.enabled)
        {
            //Debug.Log("PLAYER HAS BEEN HIT BY ENEMY");
            DamagePlayer(obj.gameObject.GetComponent<EnemyHitbox>().DoDamage()); // GetComponent call is bad okay? Find another method to extract this data?
            PlayerKnockback(obj.transform.position);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (transform.position.y > col.transform.position.y)
            {
                playerAnimator.SetBool("OnGround", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Portal")
        {
            inPortal = false;
            Debug.Log("Player left " + obj.gameObject.name);
        }
    }

    private void DamagePlayer(int damage)
    {
        HP -= damage;

        hitInvulnerability = true;
        playerSprite.DOFade(0, hitInvulnerabilityDuration).SetEase(Ease.Flash, 16, 0);

        if (HP <= 0)
        {
            // Kill Player
            Debug.Log("Player has died!");
        }
    }

    private void PlayerKnockback(Vector3 sourcePosition)
    {
        float force = 1000f;
        Vector3 direction = Vector3.Normalize(transform.position - sourcePosition);

        Debug.DrawLine(transform.position, direction, Color.red, 2f);

        playerRigidbody.AddForce(direction * force);
    }

    public void SetDebugText(string text)
    {
        debugText.text = text;
    }

    //Calculates the total movement for the player (Global stage movement + local stage movement)
    public void MovePlayer()
    {
        transform.position += transform.right * playerMovespeed * Time.fixedDeltaTime;

        if (playerRigidbody.velocity.y < MAX_FALLING_SPEED)
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, MAX_FALLING_SPEED);
    }

    public void Move(float direction)
    {
        if (direction < 0)      //Move left
        {
            playerMovespeed = -playerMovespeedValue;
            playerSprite.flipX = true;
            FaceDirection = -1;
        }
        else if (direction > 0) // Move right
        {
            playerMovespeed = playerMovespeedValue;
            playerSprite.flipX = false;
            FaceDirection = 1;
        }
        else
            playerMovespeed = 0;
    }

    // TODO: Change jumping so it applies additional force as the jump button is held down, instead of an instantaneous velocity.
    // Use Hollow Knight as a reference.
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

    public void Ability1()
    {
        // TODO: Use a scriptable object so it can be a generic call to use the ability. For now, using a hardcoded approach to test next level portals

        if (inPortal)
        {
            // Event for next level
            Debug.Log("Player has entered the portal!");
        }
        else
        {
            if (ricochetAbility != null)
            {
                if (MP >= ricochetAbility.GetManaCost())
                {
                    playerAnimator.SetTrigger("ThrowWeapon");
                    ricochetAbility.ThrowWeapon(FaceDirection);
                    MP -= ricochetAbility.GetManaCost();
                }
            }
        }
    }

    private void CheckPlayerInvulnerability()
    {
        if (hitInvulnerability && hitInvulnerabilityTimer < hitInvulnerabilityDuration)
        {
            hitInvulnerabilityTimer += Time.deltaTime;

            if (hitInvulnerabilityTimer >= hitInvulnerabilityDuration)
            {
                hitInvulnerabilityTimer = 0;
                hitInvulnerability = false;
            }

        }
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

    private void PlayerRegen()
    {
        HP += hpRegenRate * Time.deltaTime;
        MP += mpRegenRate * Time.deltaTime;

        if (HP >= maxHP)
            HP = maxHP;

        if (MP >= maxMP)
            MP = maxMP;
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

    public void CatchWeapon(float manaRecovery)
    {
        MP += manaRecovery;
        if (MP >= maxMP)
            MP = maxMP;

        BuffStacks++;
        if (BuffStacks > maxBuffStacks)
            BuffStacks = maxBuffStacks;
    }

    public void ResetBuffStack()
    {
        BuffStacks = 0;
    }
}
