using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Toggles the time scale between 1 and 0.7
    // whenever the user hits the Fire1 button.
    private float fixedDeltaTime;


    private Player playerComponent;

    // Start is called before the first frame update
    void Start()
    {
        playerComponent = this.GetComponent<Player>();
    }
    
    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            playerComponent.Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            playerComponent.ReleaseJump();
        }
        //Fast fall 
        if (Input.GetButtonDown("Down"))
        {
            playerComponent.SetDebugText("Downing");
            playerComponent.Down();
        }
        // Crouch
        if (Input.GetButton("Down"))
        {
            //playerComponent.SetDebugText("Crouching");
            playerComponent.Crouch();
        }

        //Move Left/Right
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
            playerComponent.Move(Input.GetAxisRaw("Horizontal"));

        //Slash
        if (Input.GetButtonDown("Slash"))
        {
            playerComponent.SetDebugText("Slashing");
            playerComponent.Slash();
        }
        //Smash
        if (Input.GetButtonDown("Smash"))
        {
            playerComponent.SetDebugText("SMASHING");
            playerComponent.Smash();
        }

        //Time Scale (for debugging purposes)
        if (Input.GetButtonDown("TimeScaleTest"))
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.25f;
            else
                Time.timeScale = 1.0f;
            // Adjust fixed delta time according to timescale
            // The fixed delta time will now be 0.02 real-time seconds per frame
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }
}
