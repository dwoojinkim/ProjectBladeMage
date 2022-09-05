using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = transform.parent.GetComponent<Enemy>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int DoDamage()
    {
        return enemyScript.Damage;
    }
}
