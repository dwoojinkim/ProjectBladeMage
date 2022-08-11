using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private FloatReference currentHP;
    [SerializeField] private FloatReference maxHP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = currentHP / maxHP;
    }
}
