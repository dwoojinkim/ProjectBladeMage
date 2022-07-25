using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


// Contains basic enemy functions and fields common for all enemies
public class Enemy : MonoBehaviour
{
    public EnemySO enemyData;
    public EnemyRuntimeSet ActiveEnemiesSet;

    public bool IsAlive { get; private set;}
    public int Damage { get; private set;}

    public SpriteRenderer EnemySprite {get; private set;}
    public Collider2D EnemyCollider {get; private set;}
    public AIPath AIPathScript {get; private set;}

    protected AIDestinationSetter AIDestinationSetterScript;
    protected EnemyAI enemyAIScript;

    protected int maxHP;
    protected int currentHP;

    protected Vector3 startingPos;
 

    // Start is called before the first frame update
    void Awake()
    {
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void InitializeEnemy()
    {
        EnemySprite = this.transform.GetComponent<SpriteRenderer>();
        EnemyCollider = this.transform.GetComponent<Collider2D>();
        //AIPathScript = this.transform.GetComponent<AIPath>();
        //AIDestinationSetterScript = this.transform.GetComponent<AIDestinationSetter>();
        if (this.transform.GetComponent<EnemyAI>() != null)
            enemyAIScript = this.transform.GetComponent<EnemyAI>();

        startingPos = transform.position;   // This will need to change to be starting at a general area outside of screen pos instead of a specific position.

        maxHP = enemyData.maxHP;
        currentHP = maxHP;
        Damage = enemyData.damage;
        IsAlive = false;

        ActiveEnemiesSet.Add(this);
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        //Debug.Log(obj.name + " has touched" + this.name);

        if (obj.gameObject.tag == "Projectile" && obj.GetComponent<Projectile>().Owner == "Player")
        {
            //Debug.Log("Player has hit an enemy!");

            if (currentHP > 0)
            {
                currentHP -= obj.GetComponent<Projectile>().Damage;
                if (currentHP <= 0)
                {
                    //Debug.Log(this.name + " has died!");
                    KillEnemy();
                }
            }
        }
    }

    // Currently just passing in each stat that I need set for different enemies as a parameter, but I may change this 
    // since I'm expecting more stats to be set in here in the future.
    // Probably need to change this to extracting the stats from an XML file. I'll need to read up on that though.
    public void CreateEnemy(int maxHealth)
    {
        maxHP = maxHealth;
        currentHP = maxHealth;
    }

    public void KillEnemy()
    {
        IsAlive = false;

        EnemySprite.enabled = false;
        EnemyCollider.enabled = false;
        enemyAIScript.enabled = false;
        //AIPathScript.enabled = false;
        ActiveEnemiesSet.Remove(this);

        this.gameObject.SetActive(false);
    }

    public void SpawnEnemy()
    {
        SpawnEnemy(startingPos);
    }

    public void SpawnEnemy(Vector2 spawnPos)
    {
        //Spawn logic
        // - Basically just enable the enemy so other scriptes are running and sprite is visible
        // = Can add more logic for spawning effects if necessary
        // - Script will be attached to all enemies.

        transform.position = spawnPos;

        IsAlive = true;
        EnemySprite.enabled = true;
        EnemyCollider.enabled = true;   // This will work for ALL 2d Colliders since the variable is a generic Collider2D type
        //AIPathScript.enabled = true;
        //AIDestinationSetterScript.target = LevelManager.LMinstance.Player.transform;
        if (this.transform.GetComponent<EnemyAI>() != null)
            enemyAIScript.target = LevelManager.LMinstance.Player.transform;
        currentHP = maxHP;
        IsAlive = true;

        Debug.Log("Spawning enemy!");
    }
}
