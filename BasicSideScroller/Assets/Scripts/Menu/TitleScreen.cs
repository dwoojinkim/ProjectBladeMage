using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject OptionSelector;
    public Transform InitialMenu;

    private List<GameObject> InitialMenuButtons = new List<GameObject>();


    private int selectorPosition;

    // Start is called before the first frame update
    void Start()
    {
        InitialMenuButtons = AddMenuButtons(InitialMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<GameObject> AddMenuButtons(Transform menu)
    {
        List<GameObject> buttonList = new List<GameObject>();

        for (int i = 0; i < menu.childCount; i++)
            buttonList.Add(menu.GetChild(i).gameObject);

        return buttonList;
    }
}
