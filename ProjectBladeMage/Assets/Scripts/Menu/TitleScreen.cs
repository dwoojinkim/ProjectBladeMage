using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
   enum Menu
    {
        PressAnyButton,
        InitialMenu
    }

    public GameObject OptionSelector;
    public GameObject PressAnyButtonText;
    public Transform InitialMenu;

    private Menu currentMenu;
    private Transform currentMenuTransform;
    private List<GameObject> InitialMenuButtons = new List<GameObject>();


    private int selectorPosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = Menu.PressAnyButton;
        InitialMenuButtons = AddMenuButtons(InitialMenu);
        currentMenuTransform = PressAnyButtonText.transform;
        selectorPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    private List<GameObject> AddMenuButtons(Transform menu)
    {
        List<GameObject> buttonList = new List<GameObject>();

        for (int i = 0; i < menu.childCount; i++)
            buttonList.Add(menu.GetChild(i).gameObject);

        return buttonList;
    }

    private void PlayerInput()
    {
        if (currentMenu == Menu.PressAnyButton)
        {
            if (Input.anyKeyDown)
            {
                currentMenu = Menu.InitialMenu;
                PressAnyButtonText.SetActive(false);
                InitialMenu.gameObject.SetActive(true);
                //OptionSelector.SetActive(true);
                currentMenuTransform = InitialMenu;
            }
        }
        else 
        {
            if (Input.GetButtonDown("Up"))
                selectorPosition--;
            else if (Input.GetButtonDown("Down"))
                selectorPosition++;
            else if (Input.GetButtonDown("Submit"))
            {
                // Code for activating button functionality.
                Debug.Log("Submit button has been pressed.");
            }


            // When the cursor is on the bottom option and presses down, the cursor will loop to the top
            // and vice versa
            if (selectorPosition >= currentMenuTransform.childCount)
                selectorPosition -= currentMenuTransform.childCount;
            else if (selectorPosition < 0)
                selectorPosition += currentMenuTransform.childCount;


            //Debug.Log(selectorPosition);

            //MoveSelector(currentMenuTransform.GetChild(selectorPosition));
        }

    }

    private void MoveSelector(Transform menuOption)
    {
        float selectorX = OptionSelector.GetComponent<RectTransform>().anchoredPosition.x;

        OptionSelector.GetComponent<RectTransform>().anchoredPosition = new Vector2(selectorX, menuOption.GetComponent<RectTransform>().anchoredPosition.y);
    }


    // Function to move to the character select screen after choosing "Start Game"
    // This will change eventually, but for now will go straight into character select.
    public void CharacterSelectScreen()
    {
        // Move to character select. 
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}
