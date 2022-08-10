using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic walking enemy that just walks back and forth.
public class WalkerAI : EnemyAI
{
    [SerializeField] private GameObjectReference playerObject;
    private float acceleration = 20f;
    private float currentSpeed = 0f;
    private int direction = 1;
    private LayerMask unpassableMask;

    // Start is called before the first frame update
    void Start()
    {
        EnemyGFX = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        unpassableMask = LayerMask.GetMask("UnpassableEnvironment");
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
            if (col.collider.bounds.max.y > enemyCollider.bounds.min.y)
                FlipDirection();
        }
    }

    override protected void MoveEnemy()
    {
        currentSpeed = AccelerateVelocity(currentSpeed, direction);

        float force = currentSpeed * Time.deltaTime;
    
        transform.position += Vector3.right * force;

        
        if (direction > 0)              // Enemy is moving right
            EnemyGFX.flipX = false;
        else                            // Enemy is moving left
            EnemyGFX.flipX = true;
    }

    // Returns updated velocity after accelerating relative to previous frame
    private float AccelerateVelocity(float currentSpeed, float direction)
    {
        currentSpeed += direction * acceleration * Time.deltaTime;

        if (Mathf.Abs(currentSpeed) > speed)
            currentSpeed = direction * speed;

        return currentSpeed;
    }

    private void FlipDirection()
    {
        direction *= -1;
    }

    // Checks 
    private void CheckFront()
    {
        float verticalOffset = enemyCollider.bounds.max.y - transform.position.y;
        float horizontalOffset = direction * (enemyCollider.bounds.max.x - transform.position.x);
        Vector2 checkStartPos = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);        
        
        Debug.DrawRay(checkStartPos, Vector2.right * direction, Color.green, 0, false);
        if (Physics2D.Raycast(checkStartPos, Vector3.right * direction, 1.5f, unpassableMask))
            FlipDirection();

    }

    private void CheckFloor()
    {

    }
}
