using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic projectile functions and fields
public class Projectile : MonoBehaviour
{
    public string Owner { get; private set; }

    public int Damage { get; private set;}

    private int numHits;
    private int maxNumHits = 0; // Hard-coded for now. This will be dynamic for piercing projectiles

    // Start is called before the first frame update
    void Start()
    {
        numHits = maxNumHits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        // Now taking into consideration anyu piercing projectiles.
        if (obj.tag == "Enemy")
        {
            if (numHits > 0)
                numHits--;
            else
            {
                numHits = 0;
                Reset();
            }
        }
    }

    public void CreateProjectile(string ownerTag, int dmg)
    {
        Owner = ownerTag;
        Damage = dmg;
    }

    private void Reset()
    {
        if (this.transform.GetComponent<Bullet>() != null)
            this.transform.GetComponent<Bullet>().LandBullet();

        numHits = maxNumHits;
    }
}
