using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump is pressed.");
        }
        //Fast fall / crouch
        if (Input.GetButton("Down"))
        {
            Debug.Log("Down is pressed.");
        }

        
        //Slash
        if (Input.GetButtonDown("Slash"))
        {
            Debug.Log("Slash is pressed.");
        }
        //Smash
        if (Input.GetButtonDown("Smash"))
        {
            Debug.Log("Smash is pressed.");
        }
    }
}
