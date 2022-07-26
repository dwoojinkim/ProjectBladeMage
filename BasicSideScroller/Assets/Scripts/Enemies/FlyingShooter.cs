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
            if (IsPlayerCloseEnough(attackRange))
                Shoot();
        }

        cooldownTimer += Time.deltaTime;
    }

    // So anyway, I started blasting
    protected override void Shoot()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {
                Debug.Log("FlyingShooter is shooting from: " + this.transform.position);

                bullet.transform.position = this.transform.position;
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().IsEnabled = true;
                bullet.GetComponent<Bullet>().FireProjectile(playerPositionTracker.vector3Value.normalized);

                cooldownTimer = 0;
                return;
            }
        }
    }

}
