using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Remove this once I no longer need the debug text

public class Player : MonoBehaviour
{
    public GameObject DebugTextObj;
    public float jumpVelocity = 5f;
    public float jumpGravity = 1f;
    public float normalGravity = 10f;

    private Rigidbody2D playerRigidbody;

    private Text debugText;
    private bool jumping = false;


    // Start is called before the first frame update
    void Start()
    {
        if (DebugTextObj != null)
        {
            debugText = DebugTextObj.GetComponent<Text>();
        }
        
        playerRigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Obstacle")
        {
            Debug.Log("Game Over");
            //Reset game logic here
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("ONCollision2D");
        if (col.gameObject.tag == "Ground")
        {
            Debug.Log("Touched ground");
            jumping = false;
        }
    }
    

    public void SetDebugText(string text)
    {
        debugText.text = text;
    }

    public void Jump()
    {
        SetDebugText("Jumping");

        //If not already in a jumping state, then do below
        if (!jumping)
        {
            playerRigidbody.velocity = transform.up * jumpVelocity;
            playerRigidbody.gravityScale = jumpGravity;
            jumping = true;
        }

    }

    public void ReleaseJump()
    {
        playerRigidbody.gravityScale = normalGravity;
    }
}
