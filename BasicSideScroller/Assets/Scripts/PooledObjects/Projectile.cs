using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic projectile functions and fields
public class Projectile : MonoBehaviour
{
    public ProjectileSO projectileData;

    public bool IsEnabled { get; set;}
    public string Owner { get; private set; }
    public int Damage { get; private set;}

    protected int baseDamage;
    protected int numHits;
    protected int maxNumHits;
    protected float speed;
    protected Vector3 direction = Vector3.right;
    protected Collider2D projectileCollider;

    void Awake()
    {
        InitializeProjectile();
    }

    void Update()
    {
        if (IsEnabled)
            MoveProjectile();

    }

    // Checking who shoots and gets hit can probably be done more optimally with Scriptable Object Sets
    void OnTriggerEnter2D(Collider2D obj)
    {
        // Now taking into consideration any piercing projectiles with numHits
        if (Owner == "Player")
        {
            if (IsEnabled)
            {
                if (obj.tag == "Enemy")
                {
                    ReduceProjectileHits();
                    obj.gameObject.GetComponent<Enemy>().DamageEnemy(baseDamage);   // Using GetComponent for the sake of prototype. Need to eventually change it from just using baseDamage as well
                }
                else if (obj.tag == "Barrier");     // For some reason the player is getting hit with the projectile first with "Barrier" tag...
                {
                    Debug.Log("Barrier hit! - " + obj.name);
                    Reset();
                }

            }
        }
        else if (Owner == "Enemy")
        {
            if (obj.tag == "Player")
                ReduceProjectileHits();
        }
    }

    protected void ReduceProjectileHits()
    {
        if (numHits > 0)
            numHits--;
        else
        {
            numHits = 0;
            Reset();
        }
    }

    protected void InitializeProjectile()
    {
        IsEnabled = false;

        baseDamage = projectileData.baseDamage;
        maxNumHits = projectileData.maxNumHits;
        speed = projectileData.projectileSpeed;
        projectileCollider = GetComponent<Collider2D>();
        projectileCollider.enabled = false;

        numHits = maxNumHits;
    }

    protected void Reset()
    {
        IsEnabled = false;
        transform.position = new Vector3(1000, 1000, 0);
        numHits = maxNumHits;
        projectileCollider.enabled = false;

        this.gameObject.SetActive(false);
    }

    public void CreateProjectile(string ownerTag, int dmg)
    {
        Owner = ownerTag;
        Damage = dmg;
    }

    public void FireProjectile(Vector3 dir)
    {
        direction = dir;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        transform.rotation *= Quaternion.Euler(0, 0, -90);
        projectileCollider.enabled = true;
    }

    // Updates position of bullet
    protected virtual void MoveProjectile()
    {
        transform.position += direction * speed * Time.deltaTime;

        // Placeholder reset condition. Will need to be relative to screen rather than hard coded values
        if (transform.position.x >=  50 || transform.position.x <= -50 || transform.position.y >= 50 || transform.position.x <= -50)
            Reset();

    }


}
