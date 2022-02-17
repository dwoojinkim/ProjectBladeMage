using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemy : MonoBehaviour
{
    public float circleRadius = 5.0f;
    private Collider2D[] enemies = new Collider2D[100];
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

        DrawEllipse(this.transform.position, Vector3.forward, Vector3.up, circleRadius, circleRadius, 16, Color.red);
        Debug.Log("Number of enemies in circle: " + numEnemies);
    }

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
}
