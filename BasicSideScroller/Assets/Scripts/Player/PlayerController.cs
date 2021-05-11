using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player playerComponent;

    // Start is called before the first frame update
    void Start()
    {
        playerComponent = this.GetComponent<Player>();
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
        //Fast fall / crouch
        if (Input.GetButton("Down"))
        {
            playerComponent.SetDebugText("Downing");
            playerComponent.Down();
        }


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
        }
    }
}
