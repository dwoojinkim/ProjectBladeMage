using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    private Collider2D[] enemies = new Collider2D[100];
    private float circleRadius = 5.0f;
    private LayerMask enemyLayer;
    private ContactFilter2D enemyFilter;

    private int numEnemies = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        enemyFilter.SetLayerMask(enemyLayer);
    }

    // Update is called once per frame
    void Update()
    {
        numEnemies = Physics2D.OverlapCircle((Vector2)this.transform.position, circleRadius, enemyFilter, enemies);

        Debug.Log("Number of enemies in circle: " + numEnemies);
    }
}
