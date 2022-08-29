using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadsword : MonoBehaviour, Weapon
{
    private enum BroadswordDirection
    {
        Right = 1,
        Left = -1
    }

    private enum BroadswordState
    {
        Idle,
        Thrown,
        Ricocheted
    }

    [SerializeField] private GameObjectReference playerObj;
    [SerializeField] private FloatVariable broadswordManaCost;
    
    private Ricochet ricochetAbility;   // Definitely a better way to go about this, but doing dumb way for now.
    private float rotationSpeed;
    private float movementSpeed;
    private float distanceDiff;     // Difference in distance between the player and broadsword on enemy hit
    private float initRicochetVelocity = 50f;
    private float ricochetVelocityX;
    private float ricochetVelocityY;
    private int baseDamage;
    private BroadswordDirection direction = BroadswordDirection.Right;
    private BroadswordState broadswordState = BroadswordState.Idle;
    private Vector3 throwDirection = Vector3.right;
    private Vector3 ricochetDirection = Vector3.up;
    private float gravity = 80f;
    private float manaRecoveryPercent = 0.5f;               // Percent of mana cost recovered when caught;
    private float maxRange = 25f;
    private float currentDistance = 0;                      // Distance broadsword has moved so far after being thrown.


    // Start is called before the first frame update
    void Start()
    {
        ricochetAbility = playerObj.gameObject.GetComponent<Ricochet>();
        rotationSpeed = -2500f;     // >0 = Spins Right ; <0 = Spins Left
        movementSpeed = 50f;
        baseDamage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (broadswordState == BroadswordState.Thrown)
        {
            Movebroadsword();
            //Movebroadsword2D(throwDirection);
        }
        else if (broadswordState == BroadswordState.Ricocheted)
        {
            RicochetMovement();
            Rotatebroadsword();
        }

    }

     void OnTriggerEnter2D(Collider2D obj)
    {
        if (broadswordState == BroadswordState.Thrown)
        {
            if (obj.tag == "Enemy")
            {
                obj.gameObject.GetComponent<Enemy>().DamageEnemy(baseDamage + playerObj.gameObject.GetComponent<Player>().BuffStacks);   // Using GetComponent for the sake of prototype. Need to eventually change it from just using baseDamage as well
                Ricochetbroadsword();
                //Reset();
            }
        }
        else if (broadswordState == BroadswordState.Ricocheted)
        {
            if (obj.tag == "Player")
            {
                obj.GetComponent<Player>().CatchWeapon(broadswordManaCost.Value);
                Reset();
            }
            else if (obj.tag == "Ground")
            {
                playerObj.gameObject.GetComponent<Player>().ResetBuffStack();
                Reset();
            }
        }

    }

    private void Rotatebroadsword()
    {
        transform.Rotate(Vector3.forward * (int)direction * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void Movebroadsword()
    {
        transform.position += Vector3.right * (int)direction * movementSpeed * Time.deltaTime;
        currentDistance += movementSpeed * Time.deltaTime;
        if (currentDistance >= maxRange)
            Reset();
    }

    // After a bit of testing, it doesn't feel good because it's hard to aim with the camera moving and jumping around.
    private void Movebroadsword2D(Vector3 dir)
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
        direction = BroadswordDirection.Left;
    }

    private void SetRightRotation()
    {
        direction = BroadswordDirection.Right;
    }

    private void SetRotation(float dir)
    {
        if (dir < 0)
            direction = BroadswordDirection.Left;
        else
            direction = BroadswordDirection.Right;
    }

    // Launches broadsword towards player
    private void Ricochetbroadsword()
    {
        distanceDiff = playerObj.gameObject.transform.position.x - transform.position.x;

        //ricochetVelocityX = Mathf.Cos(CalculateAngle(distanceDiff)) * initRicochetVelocity * (distanceDiff / Mathf.Abs(distanceDiff));
        //ricochetVelocityY = Mathf.Sin(CalculateAngle(distanceDiff)) * initRicochetVelocity;

        //ricochetVelocityX = Mathf.Cos(CalculateFakeAngle(distanceDiff)) * initRicochetVelocity * (distanceDiff / Mathf.Abs(distanceDiff));
        //ricochetVelocityY = Mathf.Sin(CalculateFakeAngle(distanceDiff)) * initRicochetVelocity;

        Vector2 initVel = CalculateVelocity(distanceDiff, 0.85f);

        ricochetVelocityX = initVel.x;
        ricochetVelocityY = initVel.y;

        //Debug.Log("Angle = " + CalculateAngle(distanceDiff));
        
        broadswordState = BroadswordState.Ricocheted;
        

        SetRotation(distanceDiff);
    }

    private float CalculateAngle(float range)
    {
        float time = 2f;
        float insideASin = gravity * Mathf.Abs(range) / Mathf.Pow(initRicochetVelocity, 2);
        float offset = 150;
        float angleInDegrees = insideASin / 2f * offset;

        //Debug.Log("Inside Arc Sin = " + insideASin);

        return angleInDegrees * Mathf.Deg2Rad;
        //return Mathf.Asin((time * gravity) / (2f * initRicochetVelocity));
    }

    private float CalculateFakeAngle(float range)
    {
        int minAngle = 45;
        int maxAngle = 90;

        Debug.Log("Range: " + range);

        return 45f;
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
        broadswordState = BroadswordState.Thrown;
        throwDirection = directionVector;
    }

    public void ThrowWeapon(int dir)
    {
        direction = (BroadswordDirection)dir;
        broadswordState = BroadswordState.Thrown;

        if (direction > 0)
            SetRightRotation();
        else
            SetLeftRotation();
    }

    public void Reset()
    {
        transform.position = new Vector3(1000, 1000, 0);
        broadswordState = BroadswordState.Idle;
        SetRightRotation();
        currentDistance = 0;

        if (ricochetAbility != null)
            ricochetAbility.ReturnWeapon(this.gameObject);
        else
            Debug.Log("Ricochet Ability is null!");

        this.gameObject.SetActive(false);
    }
}
