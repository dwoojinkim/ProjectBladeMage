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
    private float initRicochetVelocity;
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
        movementSpeed = 20f;
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
                Reset();
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
        ricochetVelocityY = 45f;
        bladerangState = BladerangState.Ricocheted;

        SetRotation(distanceDiff);
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
