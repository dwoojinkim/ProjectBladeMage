using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private CharacterSO _characterInfo;
    [SerializeField] private GameObject _characterPrefab;

    private string _characterName = "Unavailable";

    // Start is called before the first frame update
    void Start()
    {
        if (_characterInfo != null)
        {
            _characterName = _characterInfo.characterName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetCharacterName()
    {
        return _characterName;
    }

    public GameObject GetCharacterPrefab()
    {
        return _characterPrefab;
    }
}
