using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShooter : ShootingEnemy
{
    // Start is called before the first frame update
    void Awake()
    {
        InitializeShootingEnemy();
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
        hpBar.value = (float)currentHP / maxHP;
    }

    // So anyway, I started blasting
    override protected void Shoot()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeSelf)
            {
                bullet.transform.position = this.transform.position;

                Vector3 playerDirection = (playerPositionTracker.vector3Value - bullet.transform.position).normalized;// Vector3 variable to get direction from bullet to player. maybe something like (bullet.transform.position - playerPositionTracker.vector3Value)

                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().IsEnabled = true;
                bullet.GetComponent<Bullet>().FireProjectile(playerDirection);

                cooldownTimer = 0;
                return;
            }
        }
    }

}
