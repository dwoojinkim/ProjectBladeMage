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

    public void CreateBullet(string ownerTag, int dmg)
    {
        Owner = ownerTag;
        Damage = dmg;
    }
}
