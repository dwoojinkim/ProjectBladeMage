using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This may be a redundant component as I can just move this function to the Enemy component. Think about deleting this.
public class EnemySpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;   // I have to change this, since obviously not all enemies will have circle colliders

        Debug.Log("Spawning enemy!");
    }
}
