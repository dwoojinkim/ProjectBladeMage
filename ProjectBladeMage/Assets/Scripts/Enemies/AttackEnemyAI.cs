using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyAI : EnemyAI
{

    [SerializeField] protected float attackDashDistance;
    [SerializeField] protected float attackDashSpeed;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackWindUpDuration;
    [SerializeField] protected GameObject attackRange;

    protected float attackCooldownTimer;
    protected float attackWindUpTimer;

    protected bool canAttack = true;



    virtual public void Attack()
    {

    }

    virtual protected void FlipAttackEnemy()
    {
        FlipEnemy();

        attackRange.transform.localScale = new Vector3(movementDirection, attackRange.transform.localScale.y, 1);
    }
}
