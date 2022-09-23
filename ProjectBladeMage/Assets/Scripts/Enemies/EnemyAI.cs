using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;

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
    [SerializeField] protected Vector2 scoutRange;          // Range Squire will scout before going idle. Index 0 = min; Index 1 = max;
    [SerializeField] protected Vector2 idleTimeRange;       // Range of time Squire will be idle in seconds.

    protected float setScoutRange;                          // Scout range set within range of scoutRange;
    protected float setIdleTime;                            // Idle time set within range of idleTimeRange.
    protected float distanceScouted;                        // Distance enemy moved so far during scouting.
    protected float idleTimer;                              // Actual timer keeping track of how long player is idle.
    protected float startPositionX;

    [SerializeField] protected GameObject EnemyGFXObj;
    protected SpriteRenderer enemyGFX;
    protected Collider2D enemyCollider;
    protected Rigidbody2D rb;
    protected Enemy enemyScript;
    protected Animator enemyAnimator;
    protected EnemyState currentState = EnemyState.Idle;
    protected int movementDirection = 1;                      // -1 = left; 1 = right;
    protected LayerMask unpassableMask;

    protected bool onGround = false;
    #endregion

    public TextMeshProUGUI DebugText;

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
        enemyGFX = EnemyGFXObj.GetComponent<SpriteRenderer>();
        enemyAnimator = EnemyGFXObj.GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        enemyScript = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();

        if (GetComponent<TextMeshProUGUI>() != null)
            DebugText = GetComponent<TextMeshProUGUI>();

        unpassableMask = LayerMask.GetMask("UnpassableEnvironment");
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
            enemyGFX.flipX = false;
        else if (rb.velocity.x <= -0.01f && force.x < 0f)   // Enemy is moving left
            enemyGFX.flipX = true;
    }

    // General script to flip the enemy. Can be overrided if necessary.
    virtual protected void FlipEnemy()
    {
        //enemyGFX.flipX = movementDirection == 1 ? false : true;
        enemyCollider.transform.localScale = new Vector3(movementDirection, enemyCollider.transform.localScale.y, 1);
    }

    protected void SetIdleTime()
    {
        setIdleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
    }

    protected void SetScoutRange()
    {
        setScoutRange = Random.Range(scoutRange.x, scoutRange.y);
    }

    virtual public void DetectPlayer()
    {

    }

    protected bool IsGrounded()
    {
        float extraHeightCheck = 0.5f;

        RaycastHit2D groundHit = Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size - new Vector3(0.2f, 0f, 0f), 0f, Vector2.down, extraHeightCheck, unpassableMask);

        return groundHit.collider != null;
    }

    protected bool TouchingWallLeft()
    {
        float extraDistanceCheck = 0.5f;

        RaycastHit2D wallHit = Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size - new Vector3(0.2f, 0f, 0f), 0f, Vector2.left, extraDistanceCheck, unpassableMask);

        return wallHit.collider != null;
    }

    protected bool TouchingWallRight()
    {
        float extraDistanceCheck = 0.5f;

        RaycastHit2D wallHit = Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size - new Vector3(0.2f, 0f, 0f), 0f, Vector2.right, extraDistanceCheck, unpassableMask);

        return wallHit.collider != null;
    }
}
