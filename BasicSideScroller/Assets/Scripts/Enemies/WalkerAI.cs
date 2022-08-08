using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic walking enemy that just walks back and forth.
public class WalkerAI : EnemyAI
{
    [SerializeField] private GameObjectReference playerObject;
    private float acceleration = 10f;
    private float currentSpeed = 0f;
    private int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        EnemyGFX = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            direction *= -1;
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
}
