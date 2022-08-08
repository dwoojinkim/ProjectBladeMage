using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform target;
    protected SpriteRenderer EnemyGFX;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;

    protected Seeker seeker;
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLoop();
    }

    protected void OnStart()
    {
        EnemyGFX = GetComponent<SpriteRenderer>();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    protected void UpdateLoop()
    {
        if (path == null)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;

        MoveEnemy();
    }

    virtual protected void MoveEnemy()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
    
        // Will need to add air drag to rigidbody in order for the enemy to come to a stop.
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
            currentWaypoint++;
        
        if (rb.velocity.x >= 0.01f && force.x > 0f)         // Enemy is moving right
            EnemyGFX.flipX = false;
        else if (rb.velocity.x <= -0.01f && force.x < 0f)   // Enemy is moving left
            EnemyGFX.flipX = true;
    }
}
