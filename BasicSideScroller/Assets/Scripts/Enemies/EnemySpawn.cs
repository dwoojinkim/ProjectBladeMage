using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This may be a redundant component as I can just move this function to the Enemy component. Think about deleting this.
public class EnemySpawn : MonoBehaviour
{
    private SpriteRenderer enemySprite {get; set;}
    private Collider2D enemyCollider {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = this.transform.GetComponent<Enemy>().EnemySprite;
        enemyCollider = this.transform.GetComponent<Enemy>().EnemyCollider;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        //Spawn logic
        // - Basically just enable the enemy so other scriptes are running and sprite is visible
        // = Can add more logic for spawning effects if necessary
        // - Script will be attached to all enemies.

        if (enemySprite != null)
            enemySprite.enabled = true;
        if (enemyCollider != null)
            enemyCollider.enabled = true;   // I have to change this, since obviously not all enemies will have circle colliders

        Debug.Log("Spawning enemy!");
    }
}
