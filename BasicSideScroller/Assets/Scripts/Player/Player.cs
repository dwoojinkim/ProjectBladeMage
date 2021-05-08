using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //Remove this once I no longer need the debug text

public class Player : MonoBehaviour
{
    public GameObject DebugTextObj;

    private Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        if (DebugTextObj != null)
        {
            debugText = DebugTextObj.GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Obstacle");
        {
            Debug.Log("Game Over");
            //Reset game logic here
        }
    }
}
