using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    // Start is called before the first frame update
    void Awake()
    {
        InitializeProjectile();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnabled)
            MoveProjectile();
    }
}
