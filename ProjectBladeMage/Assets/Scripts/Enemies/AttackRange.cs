﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private AttackEnemyAI enemyAIScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyAIScript = transform.parent.GetComponent<AttackEnemyAI>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            enemyAIScript.Attack();
    }
}
