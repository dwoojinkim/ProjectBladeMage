using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,               // Standing still
        Scout,              // Currently moving (not towards anything in particular though)
        Aggro,              // Aggo to player and walking towards them
        WindUp,             // Attack wind up state
        Attack              // Attack initiated/in the middle of attacking
    };

    #region Common variables
    [SerializeField] protected float moveSpeed = 5f;

    protected SpriteRenderer EnemyGFX;
    protected Collider2D enemyCollider;
    protected Rigidbody2D rb;
    protected Enemy enemyScript;
    protected Animator enemyAnimator;
    protected EnemyState currentState = EnemyState.Idle;
    protected int movementDirection = 1;                      // -1 = left; 1 = right;
    #endregion

    #region Pathfinding Variables
    // A* Pathfinding variables. TODO: Remove from basic EnemyAI script and move them to enemy scripts that would need pathfinding.
    public Transform target;
    public float nextWaypointDistance = 3f;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;
    protected Seeker seeker;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, .5f);
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
        enemyCollider = GetComponent<Collider2D>();
        enemyScript = GetComponent<Enemy>();
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        Vector2 force = direction * moveSpeed * Time.deltaTime;
    
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

    virtual public void Attack()
    {

    }
}
