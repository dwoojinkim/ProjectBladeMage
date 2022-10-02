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

    private Menu _currentMenu;
    private GameObject _selectedCharacer;

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
            _currentMenu = Menu.PressAnyButton;
    }

    // Update is called once per frame
    void Update()
    {
        if (_selectedCharacer != null)
            Debug.Log("Character selected!");
    }

    public void SetSelectedCharacter(GameObject characterPrefab)
    {
        _selectedCharacer = characterPrefab;
    }
}
