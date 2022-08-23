using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Collider2D hurtbox;
    
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<BoxCollider2D>() != null)
        {
            hurtbox = this.GetComponent<BoxCollider2D>();
        }
        else
        {
            hurtbox = this.GetComponent<CircleCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
/*         if (obj.gameObject.tag == "SlashHitbox")
        {
            Debug.Log(this.name + " is kill");
            Kill();
        } */
    }

    public void Spawn()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        hurtbox.enabled = true;
    }

    public void Kill()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        hurtbox.enabled = false;
    }
}
