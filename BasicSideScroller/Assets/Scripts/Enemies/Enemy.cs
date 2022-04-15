using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Contains basic enemy functions and fields common for all enemies
public class Enemy : MonoBehaviour
{

    private int maxHealth = 10;
    private int currentHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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

            if (currentHealth > 0)
            {
                currentHealth -= obj.GetComponent<Projectile>().Damage;

                if (currentHealth <= 0)
                    Debug.Log(this.name + " has died!");
            }
            
            
        }
    }
}
