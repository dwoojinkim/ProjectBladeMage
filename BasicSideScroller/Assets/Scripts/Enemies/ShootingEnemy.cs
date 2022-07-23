using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generic shooting enemy with the method to detect when the player is close enough
public class ShootingEnemy : Enemy
{
    [SerializeField] private float checkDistance;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerCloseEnough(checkDistance))
            Debug.Log("Player is close enough to " + this.name);
    }

    // Returns true if player is within the check distance.
    protected bool IsPlayerCloseEnough(float checkDistance)
    {
        if (Vector3.Distance(this.transform.position, LevelManager.LMinstance.Player.transform.position) <= checkDistance)
            return true;

        return false;
    }
}
