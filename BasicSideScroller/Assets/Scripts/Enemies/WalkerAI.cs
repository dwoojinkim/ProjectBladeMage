using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAI : MonoBehaviour
{
    [SerializeField] private GameObjectReference playerObject;
    [SerializeField] private float speed = 10f;
    private float acceleration = 10f;
    private float currentSpeed = 0f;

    private SpriteRenderer EnemyGFX;

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

    private void MoveEnemy()
    {
        Debug.Log("Using WalkerAI MoveEnemy() Method!");
        float direction = playerObject.gameObject.transform.position.x - transform.position.x;
        Debug.Log("Current direction: " + direction);

        if (direction != 0)
            direction = Mathf.Abs(direction) / direction;

        currentSpeed = AccelerateVelocity(currentSpeed, direction);

        float force = currentSpeed * Time.deltaTime;
    
        // Will need to add air drag to rigidbody in order for the enemy to come to a stop.
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


        //Debug.Log("Current direction: " + direction);
        Debug.Log("Current speed: " + currentSpeed);
        return currentSpeed;
    }
}
