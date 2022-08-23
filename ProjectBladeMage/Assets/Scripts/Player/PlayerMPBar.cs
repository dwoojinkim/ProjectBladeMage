using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMPBar : MonoBehaviour
{

    [SerializeField] private Slider mpBar;
    [SerializeField] private FloatReference currentMP;
    [SerializeField] private FloatReference maxMP;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mpBar.value = currentMP / maxMP;
    }
}
