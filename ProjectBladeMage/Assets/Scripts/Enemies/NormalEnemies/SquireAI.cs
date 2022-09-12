﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquireAI : AttackEnemyAI
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
    //private float attackWindUpTimer = 0;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            Debug.Log("Collision on Wall");

            if (currentState == EnemyState.Scout || currentState == EnemyState.Attack)
            {
                currentState = EnemyState.Idle;

                idleTimer = 0;
                SetIdleTime();

                
            }
        }
    }

    override public void DetectPlayer()
    {
        if (currentState !=EnemyState.Attack)
        {
            currentState = EnemyState.Aggro;
            idleTimer = 0;

            SetIdleTime();
            SetScoutRange();

            Debug.Log("Player Dectected!");
        }
    }

    override public void Attack()
    {
        if (currentState != EnemyState.Attack && canAttack)
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

    private void AttackDash()
    {
        transform.position += Vector3.right * attackDashSpeed * movementDirection * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - startPositionX) >= attackDashDistance)
        {
            currentState = EnemyState.Idle;

            idleTimer = 0;
            SetIdleTime();
        }
    }

}
