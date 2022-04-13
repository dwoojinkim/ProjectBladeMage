using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    public float circleRadius = 5.0f;
    private Collider2D[] enemies = new Collider2D[100];
    private Collider2D closestEnemy;
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
        //closestEnemy = FindClosestEnemy(enemies);

        DrawEllipse(this.transform.position, Vector3.forward, Vector3.up, circleRadius, circleRadius, 16, Color.red);
        Debug.Log("Number of enemies in circle: " + numEnemies);

        if (closestEnemy != null)
            Debug.Log("Closest enemy: " + closestEnemy.name);
    }

    // This is just used for visualising the range of the closest enemy check
    private static void DrawEllipse(Vector3 pos, Vector3 forward, Vector3 up, float radiusX, float radiusY, int segments, Color color, float duration = 0)
    {
        float angle = 0f;
        Quaternion rot = Quaternion.LookRotation(forward, up);
        Vector3 lastPoint = Vector3.zero;
        Vector3 thisPoint = Vector3.zero;
 
        for (int i = 0; i < segments + 1; i++)
        {
            thisPoint.x = Mathf.Sin(Mathf.Deg2Rad * angle) * radiusX;
            thisPoint.y = Mathf.Cos(Mathf.Deg2Rad * angle) * radiusY;
 
            if (i > 0)
            {
                Debug.DrawLine(rot * lastPoint + pos, rot * thisPoint + pos, color, duration);
            }
 
            lastPoint = thisPoint;
            angle += 360f / segments;
        }
    }

    private Collider2D FindClosestEnemy(Collider2D[] enemyList)
    {
        Collider2D closestEnemy = null;
        float closestDistance = float.MaxValue;

        if (enemyList != null)
        {
            foreach(Collider2D enemy in enemyList)
            {
                if (enemy != null && Vector3.Distance(this.transform.position, enemy.transform.position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(this.transform.position, enemy.transform.position);
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    public Vector3 GetDirectionVector()
    {
        closestEnemy = FindClosestEnemy(enemies);

        var vector = closestEnemy.transform.position - this.transform.position;

        //vector = vector / vector.magnitude;     // Normalized direction

        return vector.normalized;
    }
}
