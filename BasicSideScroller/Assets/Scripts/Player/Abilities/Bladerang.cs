using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Bladerang : MonoBehaviour
{
    private enum BladerangDirection
    {
        Right = 1,
        Left = -1
    }

    private enum BladerangState
    {
        Idle,
        Thrown,
        Ricocheted
    }

    [SerializeField] GameObjectReference playerObj;

    private float rotationSpeed;
    private float movementSpeed;
    private float distanceDiff;     // Difference in distance between the player and bladerang on enemy hit
    private float initRicochetVelocity = 50f;
    private float ricochetVelocityX;
    private float ricochetVelocityY;
    private int baseDamage;
    private BladerangDirection direction = BladerangDirection.Right;
    private BladerangState bladerangState = BladerangState.Idle;
    private Vector3 throwDirection = Vector3.right;
    private Vector3 ricochetDirection = Vector3.up;
    private float gravity = 80f;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = -2500f;     // >0 = Spins Right ; <0 = Spins Left
        movementSpeed = 50f;
        baseDamage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        RotateBladerang();

        if (bladerangState == BladerangState.Thrown)
        {
            MoveBladerang();
            //MoveBladerang2D(throwDirection);
        }
        else if (bladerangState == BladerangState.Ricocheted)
        {
            RicochetMovement();
        }

    }

     void OnTriggerEnter2D(Collider2D obj)
    {
        if (bladerangState == BladerangState.Thrown)
        {
            if (obj.tag == "Enemy")
            {
                obj.gameObject.GetComponent<Enemy>().DamageEnemy(baseDamage);   // Using GetComponent for the sake of prototype. Need to eventually change it from just using baseDamage as well
                RicochetBladerang();
                //Reset();
            }
        }
        else if (bladerangState == BladerangState.Ricocheted)
        {
            if (obj.tag == "Player")
            {
                Reset();
            }
            else if (obj.tag == "Ground")
            {
                //Reset();
            }
        }

    }

    private void RotateBladerang()
    {
        transform.Rotate(Vector3.forward * (int)direction * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void MoveBladerang()
    {
        transform.position += Vector3.right * (int)direction * movementSpeed * Time.deltaTime;
    }

    // After a bit of testing, it doesn't feel good because it's hard to aim with the camera moving and jumping around.
    private void MoveBladerang2D(Vector3 dir)
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
        direction = BladerangDirection.Left;
    }

    private void SetRightRotation()
    {
        direction = BladerangDirection.Right;
    }

    private void SetRotation(float dir)
    {
        if (dir < 0)
            direction = BladerangDirection.Left;
        else
            direction = BladerangDirection.Right;
    }

    // Launches Bladerang towards player
    private void RicochetBladerang()
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
        
        bladerangState = BladerangState.Ricocheted;
        

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

    public void ThrowBladerang(Vector3 directionVector)
    {
        SetRightRotation();
        bladerangState = BladerangState.Thrown;
        throwDirection = directionVector;
    }

    public void ThrowBladerang(int dir)
    {
        direction = (BladerangDirection)dir;
        bladerangState = BladerangState.Thrown;

        if (direction > 0)
            SetRightRotation();
        else
            SetLeftRotation();
    }

    public void Reset()
    {
        transform.position = new Vector3(1000, 1000, 0);
        bladerangState = BladerangState.Idle;
        SetRightRotation();
        this.gameObject.SetActive(false);
    }
}
