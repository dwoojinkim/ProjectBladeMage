using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic projectile functions and fields
public class Projectile : MonoBehaviour
{
    public string Owner { get; private set; }

    public int Damage { get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "Enemy")
            Reset();

    }

    public void CreateProjectile(string ownerTag, int dmg)
    {
        Owner = ownerTag;
        Damage = dmg;
    }

    // This method can allow the bullet to hit multiple enemies before being reset. Right now,
    // it can only hit one so it'll reset right away.
    public void LandProjectile()
    {
        //Reset();
    }

    private void Reset()
    {
        if (this.transform.GetComponent<Bullet>() != null)
            this.transform.GetComponent<Bullet>().LandBullet();
    }
}
