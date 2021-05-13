using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private Obstacle obstacleScript;

    // Start is called before the first frame update
    void Start()
    {
        obstacleScript = this.gameObject.GetComponent<Obstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "SmashHitbox")
        {
            Debug.Log(this.name + " is kill");
            obstacleScript.Kill();
        }
    }
}
