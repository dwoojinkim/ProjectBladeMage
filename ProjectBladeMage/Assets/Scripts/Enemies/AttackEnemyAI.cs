using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyAI : EnemyAI
{
    [SerializeField] protected Vector2 scoutRange;          // Range Squire will scout before going idle. Index 0 = min; Index 1 = max;
    [SerializeField] protected Vector2 idleTimeRange;       // Range of time Squire will be idle in seconds.
    [SerializeField] protected float attackDashDistance;
    [SerializeField] protected float attackDashSpeed;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackWindUpDuration;

    protected float setScoutRange;                          // Scout range set within range of scoutRange;
    protected float setIdleTime;                            // Idle time set within range of idleTimeRange.
    protected float idleTimer;                              // Actual timer keeping track of how long player is idle.
    protected float startPositionX;
    protected float attackCooldownTimer;
    protected float attackWindUpTimer;

    protected bool canAttack = true;



    protected void SetIdleTime()
    {
        setIdleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
    }

    protected void SetScoutRange()
    {
        setScoutRange = Random.Range(scoutRange.x, scoutRange.y);
    }
}
