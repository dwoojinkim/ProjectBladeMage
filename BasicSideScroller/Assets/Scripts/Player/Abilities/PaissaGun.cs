using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Basic gun logic that will target the closest enemy to the player
public class PaissaGun : MonoBehaviour
{
    [SerializeField] int poolCount = 50;     // Number of bullets in the pool
    [SerializeField] GameObject bulletPrefab;
    private List<GameObject> bulletPool = new List<GameObject>();

    private float bulletDelay = 0.15f;
    private float bulletTimer = 0f;
    private ClosestEnemy closestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        CreateBulletPool();
        closestEnemy = this.transform.GetComponent<ClosestEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletTimer += Time.deltaTime;

        if  (bulletTimer >= bulletDelay)
        {
            bulletTimer = 0f;
            foreach(GameObject bullet in bulletPool)
            {
                if (!bullet.GetComponent<Bullet>().IsEnabled)
                {
                    FireBullet(bullet);
                    break;
                }
            }
        }
    }

    private void CreateBulletPool()
    {
        GameObject bullet;

        for (int i = 0; i < poolCount; i++)
        {
            bullet = Instantiate(bulletPrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
            bullet.GetComponent<Projectile>().Owner = this.tag;
            bulletPool.Add(bullet);
        }
    }

    // Fires bullet towards nearest enemy or just shoots forward
    private void FireBullet(GameObject bullet)
    {
        bullet.transform.position = this.transform.position;
        bullet.GetComponent<Bullet>().IsEnabled = true;
        bullet.GetComponent<Bullet>().FireBullet(closestEnemy.GetDirectionVector());
    }
}
