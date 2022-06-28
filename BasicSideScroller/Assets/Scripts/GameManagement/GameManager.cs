using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    enum Menu
    {
        PressAnyButton,
        InitialMenu
    }

    public static GameManager GMinstance = null;

    private Menu currentMenu;

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
            currentMenu = Menu.PressAnyButton;
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
