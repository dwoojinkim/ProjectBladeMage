using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private EnemyAI enemyAIScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyAIScript = transform.parent.GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            enemyAIScript.Attack();
    }
}
