using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAI : AttackEnemyAI
{
    //private Vector2 scoutRange;         // Range Squire will scout before going idle. Index 0 = min; Index 1 = max;
    //private Vector2 idleTimeRange;      // Range of time Squire will be idle in seconds.
    //private float idleTimer;            // Actual timer keeping track of how long player is idle.
    //private float setIdleTime;          // Idle time set within range of idleTimeRange.
    //private float setScoutRange;        // Scout range set within range of scoutRange;
    //private float startPositionX;
    //private float attackDashDistance = 7;
    //private float attackDashSpeed = 20;
    //private float attackCooldown = 2;
    //private float attackCooldownTimer = 0;
    //private float attackWindUpDuration = 0.5f;
    //private float attackWindUpTimer;
    //private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        OnStart();

        SetIdleTime();
        SetScoutRange();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug text stuff
        DebugText.text = currentState.ToString();

        if (currentState == EnemyState.Idle)
        {
            idleTimer += Time.deltaTime;
            enemyAnimator.SetBool("isMoving", false);

            if (idleTimer >= setIdleTime)
            {
                idleTimer = 0;
                SetIdleTime();
                SetScoutRange();

                currentState = EnemyState.Scout;
                startPositionX = transform.position.x;
                movementDirection = Random.Range(-1, 1) < 0 ? -1 : 1;
                FlipAttackEnemy();
            }
        }
        else if (currentState == EnemyState.Scout)
        {
            enemyAnimator.SetBool("isMoving", true);
            MoveEnemy();
        }
        else if (currentState == EnemyState.WindUp)
        {
            attackWindUpTimer += Time.deltaTime;

            if (attackWindUpTimer >= attackWindUpDuration)
            {
                attackWindUpTimer = 0;
                currentState = EnemyState.Attack;

                enemyAnimator.SetTrigger("AttackTrigger");
            }
        }
        else if (currentState == EnemyState.Attack)
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

    override public void Attack()
    {
        if (currentState != EnemyState.Attack && canAttack && !enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Attack"))
        {
            movementDirection = enemyScript.playerObject.GetTransform().position.x - transform.position.x < 0 ? -1 : 1;
            FlipAttackEnemy();

            // Add in delay before attacking
            enemyAnimator.SetTrigger("WindUpTrigger");

            currentState = EnemyState.WindUp;

            startPositionX = transform.position.x;

            canAttack = false;

        }
    }
    override protected void MoveEnemy()
    {
        transform.position += Vector3.right * moveSpeed * movementDirection * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - startPositionX) >= setScoutRange)
        {
            currentState = EnemyState.Idle;

            SetIdleTime();
        }
    }

    private void FlipKnight()
    {
        FlipEnemy();

    }

    private void AttackDash()
    {
        if (!atGroundEdge)
            transform.position += Vector3.right * attackDashSpeed * movementDirection * Time.deltaTime;

        attackDashTimer += Time.deltaTime;

        if (attackDashTimer >= attackDashDuration)
        {
            currentState = EnemyState.Idle;

            idleTimer = 0;
            SetIdleTime();

            atGroundEdge = false;
        }
    }
    private void CheckFloor()
    {
        float verticalOffset = enemyCollider.bounds.min.y - transform.position.y;
        float horizontalOffset = movementDirection * (1f + enemyCollider.bounds.max.x - transform.position.x);
        Vector2 checkStartPos = new Vector2(transform.position.x + horizontalOffset, transform.position.y + verticalOffset);

        Debug.DrawRay(checkStartPos, Vector2.down, Color.green, 0, false);
        if (onGround && !Physics2D.Raycast(checkStartPos, Vector3.down, 1f, unpassableMask))
        {
            atGroundEdge = true;
        }
    }

}
