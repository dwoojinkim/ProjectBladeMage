using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShooter : ShootingEnemy
{
    // Start is called before the first frame update
    void Awake()
    {
        InitializeShootingEnemy();
        shootCooldown = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer >= shootCooldown)
        {
            if (IsPlayerCloseEnough(checkDistance))
                Shoot();
        }

        cooldownTimer += Time.deltaTime;
    }

    // So anyway, I started blasting
    private void Shoot()
    {
        Debug.Log("FlyingShooter is shooting!");

        cooldownTimer = 0;
    }

}
