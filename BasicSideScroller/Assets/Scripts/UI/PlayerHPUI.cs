using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPUI : MonoBehaviour
{
    public FloatReference PlayerHP;
    public TextMeshProUGUI HPText;

    // Update is called once per frame
    void Update()
    {
        HPText.text = "Player HP: " + PlayerHP.Value.ToString();
    }
}
