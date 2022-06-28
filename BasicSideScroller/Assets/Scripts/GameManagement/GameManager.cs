using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager GMinstance = null;

    void Awake()
    {
        if (GMinstance == null)
        {
            GMinstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("Up"))
        {
            //move up on menu
        }
        else if (Input.GetButtonDown("Down"))
        {
            //move down on menu
        }*/
    }
}
