using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private int spikeDamage = 10;

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().PlayerKnockback(transform.position);
            collision.gameObject.GetComponent<Player>().DamagePlayer(spikeDamage);
        }
    }
}
