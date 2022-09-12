using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic walking enemy that just walks back and forth.
public class WalkerAI : EnemyAI
{
    [SerializeField] private GameObjectReference playerObject;
    private float acceleration = 20f;
    private float currentSpeed = 0f;
    private LayerMask unpassableMask;
    private bool onGround = false;
    private ColoredFlash flashFeedback;


    // Start is called before the first frame update
    void Start()
    {
        OnStart();
        unpassableMask = LayerMask.GetMask("UnpassableEnvironment");
        flashFeedback = GetComponent<ColoredFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
        //CheckFront();
        CheckFloor();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Referenced this post on LayerMasks
        // https://answers.unity.com/questions/422472/how-can-i-compare-colliders-layer-to-my-layermask.html
        if (((1 << col.gameObject.layer) & unpassableMask) != 0)
        {
            if (!onGround && col.collider.bounds.max.y <= enemyCollider.bounds.min.y)
                onGround = true;

            if (col.collider.bounds.max.y > enemyCollider.bounds.min.y)
                FlipDirection();

        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (onGround && ((1 << col.gameObject.layer) & unpassableMask) != 0 && col.collider.bounds.max.y <= enemyCollider.bounds.min.y)
            onGround = false;
    }

    override protected void MoveEnemy()
    {
        currentSpeed = AccelerateVelocity(currentSpeed, movementDirection);

        float force = currentSpeed * Time.deltaTime;
    
        transform.position += Vector3.right * force;

        
        if (movementDirection > 0)              // Enemy is moving right
            enemyGFX.flipX = false;
        else                            // Enemy is moving left
            enemyGFX.flipX = true;
    }

    // Returns updated velocity after accelerating relative to previous frame
    private float AccelerateVelocity(float currentSpeed, float direction)
    {
        currentSpeed += direction * acceleration * Time.deltaTime;

        if (Mathf.Abs(currentSpeed) > moveSpeed)
            currentSpeed = direction * moveSpeed;

        return currentSpeed;
    }

    private void FlipDirection()
    {
        movementDirection *= -1;
    }

    // Checks for unpassable objects in front of the enemy, but no longer used.
    // Now I'm using colliders instead.
    private void CheckFront()
    {
        float verticalOffset = enemyCollider.bounds.max.y - transform.position.y;
        float horizontalOffset = movementDirection * (enemyCollider.bounds.max.x - transform.position.x);
        Vector2 checkStartPos = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);        
        
        Debug.DrawRay(checkStartPos, Vector2.right * movementDirection, Color.green, 0, false);
        if (Physics2D.Raycast(checkStartPos, Vector3.right * movementDirection, 1.5f, unpassableMask))
            FlipDirection();

    }

    private void CheckFloor()
    {
        float verticalOffset = enemyCollider.bounds.min.y - transform.position.y;
        float horizontalOffset = movementDirection * (1f + enemyCollider.bounds.max.x - transform.position.x);
        Vector2 checkStartPos = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);        
        
        Debug.DrawRay(checkStartPos, Vector2.down, Color.green, 0, false);
        if (onGround && !Physics2D.Raycast(checkStartPos, Vector3.down, 1f, unpassableMask))
        {
            Debug.Log("CheckFloor clipping direction");
            FlipDirection();
        }
    }
}
