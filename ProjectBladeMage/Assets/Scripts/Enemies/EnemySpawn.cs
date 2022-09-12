using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


// This may be a redundant component as I can just move this function to the Enemy component. Think about deleting this.
public class EnemySpawn : MonoBehaviour
{
    private SpriteRenderer enemySprite;
    private Collider2D enemyCollider;
    private AIPath aiPathScript;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = this.transform.GetComponent<Enemy>().enemyGFX;
        enemyCollider = this.transform.GetComponent<Enemy>().enemyCollider;
        aiPathScript = this.transform.GetComponent<Enemy>().AIPathScript;

        startingPos = transform.position;   // This will need to change to be starting at a general area outside of screen pos instead of a specific position.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        //Spawn logic
        // - Basically just enable the enemy so other scripts are running and sprite is visible
        // = Can add more logic for spawning effects if necessary
        // - Script will be attached to all enemies.

        transform.position = startingPos;

        if (enemySprite != null)
            enemySprite.enabled = true;
        if (enemyCollider != null)
            enemyCollider.enabled = true;
        if (aiPathScript != null)
            aiPathScript.enabled = true;

        Debug.Log("Spawning enemy!");
    }
}
