using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Contains basic enemy functions and fields common for all enemies
public class Enemy : MonoBehaviour
{

    private int maxHP = 10;
    private int currentHP = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        Debug.Log(obj.name + " has touched" + this.name);

        if (obj.gameObject.tag == "Projectile" && obj.GetComponent<Projectile>().Owner == "Player")
        {
            Debug.Log("Player has hit an enemy!");

            if (currentHP > 0)
            {
                currentHP -= obj.GetComponent<Projectile>().Damage;

                if (currentHP <= 0)
                {
                    Debug.Log(this.name + " has died!");
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
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
