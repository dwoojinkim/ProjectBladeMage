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

    private float rotationSpeed;
    private float movementSpeed;
    private int baseDamage;
    private BladerangDirection direction = BladerangDirection.Right;
    private BladerangState bladerangState = BladerangState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = -2500f;     // >0 = Spins Right ; <0 = Spins Left
        movementSpeed = 10f;
        baseDamage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        RotateBladerang();
        MoveBladerang();
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

    private void SetLeftRotation()
    {
        direction = BladerangDirection.Left;
    }

    private void SetRightRotation()
    {
        direction = BladerangDirection.Right;
    }

    // Launches Bladerang towards player
    private void RicochetBladerang()
    {
        bladerangState = BladerangState.Ricocheted;
        SetLeftRotation();
    }

    public void ThrowBladerang()
    {
        SetRightRotation();
        bladerangState = BladerangState.Thrown;
    }

    public void Reset()
    {
        transform.position = new Vector3(1000, 1000, 0);
        bladerangState = BladerangState.Idle;
        SetRightRotation();
        this.gameObject.SetActive(false);
    }
}
