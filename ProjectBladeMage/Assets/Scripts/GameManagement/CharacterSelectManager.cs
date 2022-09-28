using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] private GameObject _characterHoverIndicator;
    [SerializeField] private TextMeshProUGUI _characterNameText;
    [SerializeField] private List<GameObject> _characters;


    private RectTransform _hoverIndicator;
    private int _characterSelectIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _hoverIndicator = _characterHoverIndicator.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput();

        UpdateIndicatorPosition();

        UpdateCharacterNameText();
    }

    // Temporary function for player input in the character select screen. Going to change when I'm past the prototype stage
    private void CheckPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
            _characterSelectIndex++;
        else if (Input.GetKeyDown(KeyCode.A))
            _characterSelectIndex--;

        if (_characterSelectIndex >= _characters.Count)
            _characterSelectIndex = 0;
        else if (_characterSelectIndex < 0)
            _characterSelectIndex = _characters.Count - 1;

        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("KnightsNSquires");
    }

    private void UpdateIndicatorPosition()
    {
        _hoverIndicator.anchoredPosition = _characters[_characterSelectIndex].GetComponent<RectTransform>().anchoredPosition;
    }

    private void UpdateCharacterNameText()
    {
        if (_characterNameText != null)
            _characterNameText.text = _characters[_characterSelectIndex].GetComponent<CharacterSelectButton>().GetCharacterName();            
    }
}
