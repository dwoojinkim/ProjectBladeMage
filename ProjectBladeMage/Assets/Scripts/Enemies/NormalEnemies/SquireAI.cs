using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquireAI : EnemyAI
{
    private enum SquireState
    {
        Idle,               // Standing still
        Scout,              // Currently moving (not towards anything in particular though)
        Aggro,              // Aggo to player and walking towards them
        Attack              // Attack initiated/in the middle of attacking
    };

    private SquireState currentState = SquireState.Idle;
    private Vector2 scoutRange;         // Range Squire will scout before going idle. Index 0 = min; Index 1 = max;
    private Vector2 idleTimeRange;      // Range of time Squire will be idle in seconds.
    private Enemy enemyScript;
    private float idleTimer;            // Actual timer keeping track of how long player is idle.
    private float setIdleTime;          // Idle time set within range of idleTimeRange.
    private float setScoutRange;        // Scout range set within range of scoutRange;
    private float startPositionX;
    private int movementDirection = 1;      // -1 = left; 1 = right;
    private float moveSpeed = 5;
    private float attackDashDistance = 7;
    private float attackDashSpeed = 15;
    private float attackCooldown = 1;
    private float attackCooldownTimer = 0;
    private bool canAttack = true;


    // Start is called before the first frame update
    void Start()
    {
        scoutRange = new Vector2(3, 6);       
        idleTimeRange = new Vector2(1, 3);

        enemyScript = GetComponent<Enemy>();

        EnemyGFX = GetComponent<SpriteRenderer>();
        enemyAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        SetIdleTime();
        SetScoutRange();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == SquireState.Idle)
        {
            idleTimer += Time.deltaTime;
            enemyAnimator.SetBool("isMoving", false);

            if (idleTimer >= setIdleTime)
            {
                idleTimer = 0;
                SetIdleTime();

                currentState = SquireState.Scout;
                startPositionX = transform.position.x;
                movementDirection = Random.Range(-1, 1) < 0 ? -1 : 1;
                EnemyGFX.flipX = movementDirection == 1 ? false : true;
            }
        }
        else if (currentState == SquireState.Scout)
        {
            enemyAnimator.SetBool("isMoving", true);
            MoveSquire();
        }
        else if (currentState == SquireState.Attack)
        {
            AttackDash();
        }

        if (!canAttack)
        {
            attackCooldownTimer += Time.deltaTime;

            if (attackCooldownTimer >= attackCooldown)
            {
                attackCooldownTimer = 0;
                canAttack = true;
            }
        }
    }

    public void DetectPlayer()
    {
        if (currentState !=SquireState.Attack)
        {
            currentState = SquireState.Attack;
            idleTimer = 0;

            SetIdleTime();
            SetScoutRange();
        }
    }

    override public void Attack()
    {
        if (currentState != SquireState.Attack && canAttack)
        {
            movementDirection = enemyScript.playerObject.GetTransform().position.x - transform.position.x < 0 ? -1 : 1;
            EnemyGFX.flipX = movementDirection == 1 ? false : true;

            // Add in delay before attacking
            enemyAnimator.SetTrigger("AttackTrigger");

            currentState = SquireState.Attack;

            startPositionX = transform.position.x;

            canAttack = false;
        }
    }

    private void MoveSquire()
    {
        transform.position += Vector3.right * moveSpeed * movementDirection * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - startPositionX) >= setScoutRange)
        {
            currentState = SquireState.Idle;

            SetIdleTime();
        }
    }

    private void AttackDash()
    {
        transform.position += Vector3.right * attackDashSpeed * movementDirection * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - startPositionX) >= attackDashDistance)
        {
            currentState = SquireState.Idle;

            SetIdleTime();
        }
    }

    private void SetIdleTime()
    {
        setIdleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
    }

    private void SetScoutRange()
    {
        setScoutRange = Random.Range(scoutRange.x, scoutRange.y);
    }
}
