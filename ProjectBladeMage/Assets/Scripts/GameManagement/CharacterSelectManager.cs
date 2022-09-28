using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public GameObject CharacterHoverIndicator;
    public List<GameObject> Characters;


    private RectTransform _hoverIndicator;
    private int _characterSelectIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _hoverIndicator = CharacterHoverIndicator.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();

        UpdateIndicatorPosition();
    }

    // Temporary function for player input in the character select screen. Going to change when I'm past the prototype stage
    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
            _characterSelectIndex++;
        else if (Input.GetKeyDown(KeyCode.A))
            _characterSelectIndex--;

        if (_characterSelectIndex >= Characters.Count)
            _characterSelectIndex = 0;
        else if (_characterSelectIndex < 0)
            _characterSelectIndex = Characters.Count - 1;


    }

    private void UpdateIndicatorPosition()
    {
        _hoverIndicator.anchoredPosition = new Vector2(-700 + (200 * _characterSelectIndex), _hoverIndicator.anchoredPosition.y);
    }
}
