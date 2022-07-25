using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic shooting enemy with the method to detect when the player is close enough
public class ShootingEnemy : Enemy
{
    public int numberOfBullets;
    [SerializeField] protected GameObject bulletPrefab;


    protected float shootCooldown;
    protected float cooldownTimer;
    protected Transform player;

    protected List<GameObject> bullets = new List<GameObject>();     // Using generic bullet for testing. Will be different later.

    [SerializeField] protected float checkDistance;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeShootingEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerCloseEnough(checkDistance))
            Debug.Log("Player is close enough to " + this.name);
    }

    protected void InitializeShootingEnemy()
    {
        InitializeEnemy();
        InstantiateBullets(numberOfBullets);
        player = LevelManager.LMinstance.Player.transform;  // For some reason, this is coming up null. FIX IT!!
    }

    // Returns true if player is within the check distance.
    protected bool IsPlayerCloseEnough(float checkDistance)
    {
        if (Vector3.Distance(this.transform.position, player.position) <= checkDistance)
            return true;

        return false;
    }

    protected void InstantiateBullets(int numBullets)
    {
        GameObject bullet;

        for (int i = 0; i < numBullets; i++)
        {
            bullet = Instantiate(bulletPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
            bullet.GetComponent<Projectile>().CreateProjectile(this.tag, Damage);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }
}
