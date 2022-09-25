using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvenRapier : MonoBehaviour, Weapon
{
    private enum ElvenRapierDirection
    {
        Right = 1,
        Left = -1
    }

    private enum ElvenRapierState
    {
        Idle,
        Thrown,
        Ricocheted
    }

    [SerializeField] private GameObjectReference playerObj;
    [SerializeField] private FloatVariable elvenRapierManaCost;
    
    private Ricochet ricochetAbility;   // Definitely a better way to go about this, but doing dumb way for now.
    private float rotationSpeed;
    private float movementSpeed;
    private float distanceDiff;     // Difference in distance between the player and elvenRapier on enemy hit
    private float initRicochetVelocity = 50f;
    private float ricochetVelocityX;
    private float ricochetVelocityY;
    private int baseDamage;
    private ElvenRapierDirection direction = ElvenRapierDirection.Right;
    private ElvenRapierState elvenRapierState = ElvenRapierState.Idle;
    private Vector3 throwDirection = Vector3.right;
    private Vector3 ricochetDirection = Vector3.up;
    private float gravity = 80f;
    private float manaRecoveryPercent = 0.5f;               // Percent of mana cost recovered when caught;
    private float maxRange = 15f;
    private float currentDistance = 0;                      // Distance elvenRapier has moved so far after being thrown.


    private bool hitEnemy = false;
    private bool hitApex = false;                           // When weapon hits apex of the arc, then it can collide with the ground.


    // Start is called before the first frame update
    void Start()
    {
        ricochetAbility = playerObj.gameObject.GetComponent<Ricochet>();
        rotationSpeed = -2500f;     // >0 = Spins Right ; <0 = Spins Left
        movementSpeed = 50f;
        baseDamage = 100;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (elvenRapierState == ElvenRapierState.Thrown)
        {
            MoveElvenRapier();
            //MoveelvenRapier2D(throwDirection);
        }
        else if (elvenRapierState == ElvenRapierState.Ricocheted)
        {
            RicochetMovement();
            RotateElvenRapier();

            if (ricochetVelocityY < 0)
                hitApex = true;
        }

    }

     void OnTriggerEnter2D(Collider2D col)
    {
        if (elvenRapierState == ElvenRapierState.Thrown)
        {
            if (col.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().DamageEnemy(baseDamage + playerObj.gameObject.GetComponent<Player>().BuffStacks);   // Using GetComponent for the sake of prototype. Need to eventually change it from just using baseDamage as well
                hitEnemy = true;
            }
        }
        else if (elvenRapierState == ElvenRapierState.Ricocheted)
        {
            if (col.tag == "Player")
            {
                col.GetComponent<Player>().CatchWeapon(elvenRapierManaCost.Value);
                Reset();
            }
            else if (col.tag == "Ground" && hitApex)
            {
                playerObj.gameObject.GetComponent<Player>().ResetBuffStack();
                Reset();
            }
        }

    }

    private void RotateElvenRapier()
    {
        transform.Rotate(Vector3.forward * (int)direction * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void MoveElvenRapier()
    {
        transform.position += Vector3.right * (int)direction * movementSpeed * Time.deltaTime;
        currentDistance += movementSpeed * Time.deltaTime;
        if (currentDistance >= maxRange)
        {
            if (hitEnemy)
                RicochetElvenRapier();
            else
                Reset();
        }
    }

    // After a bit of testing, it doesn't feel good because it's hard to aim with the camera moving and jumping around.
    private void MoveElvenRapier2D(Vector3 dir)
    {
        float throwAngle = Vector3.Angle(Vector3.right, dir);
        float movementSpeedX = Mathf.Cos(throwAngle * Mathf.Deg2Rad) * movementSpeed;
        float movementSpeedY = Mathf.Sin(throwAngle * Mathf.Deg2Rad) * movementSpeed;


        transform.position += Vector3.right  * movementSpeedX * Time.deltaTime;
        transform.position += Vector3.up * movementSpeedY * Time.deltaTime;
    }

    private void RicochetMovement()
    {
        ricochetVelocityY -= gravity * Time.deltaTime;

        transform.position += Vector3.up * ricochetVelocityY * Time.deltaTime;
        transform.position += Vector3.right * ricochetVelocityX * Time.deltaTime;
    }

    private void SetLeftRotation()
    {
        direction = ElvenRapierDirection.Left;
    }

    private void SetRightRotation()
    {
        direction = ElvenRapierDirection.Right;
    }

    private void SetRotation(float dir)
    {
        if (dir < 0)
            direction = ElvenRapierDirection.Left;
        else
            direction = ElvenRapierDirection.Right;
    }

    // Launches elvenRapier towards player
    private void RicochetElvenRapier()
    {
        float hangTime = 0.85f;

        distanceDiff = playerObj.gameObject.transform.position.x - transform.position.x;

        Vector2 initVel = CalculateVelocity(distanceDiff, hangTime);

        ricochetVelocityX = initVel.x;
        ricochetVelocityY = initVel.y;

        elvenRapierState = ElvenRapierState.Ricocheted;
        
        hitEnemy = false;

        SetRotation(distanceDiff);
    }

    private Vector2 CalculateVelocity(float range, float time)
    {
        float minHeightOffset = 0.5f;
        float maxHeightOffset = 10f;
        Vector2 Vo;
        float Vox = range / time;
        float Voy = (Random.Range(minHeightOffset, maxHeightOffset) / time) + 0.5f * gravity * time;

        Vo = new Vector2(Vox, Voy);

        return Vo;

    }

    public void ThrowWeapon(Vector3 directionVector)
    {
        SetRightRotation();
        elvenRapierState = ElvenRapierState.Thrown;
        throwDirection = directionVector;
    }

    public void ThrowWeapon(int dir)
    {
        direction = (ElvenRapierDirection)dir;
        elvenRapierState = ElvenRapierState.Thrown;

        if (direction > 0)
            SetRightRotation();
        else
            SetLeftRotation();
    }

    public void Reset()
    {
        transform.position = new Vector3(1000, 1000, 0);
        elvenRapierState = ElvenRapierState.Idle;
        SetRightRotation();
        currentDistance = 0;

        hitApex = false;

        if (ricochetAbility != null)
            ricochetAbility.ReturnWeapon(this.gameObject);
        else
            Debug.Log("Ricochet Ability is null!");

        this.gameObject.SetActive(false);
    }
}
