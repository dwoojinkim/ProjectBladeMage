using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    private Wave waveScript;

    // Start is called before the first frame update
    void Start()
    {
        waveScript = transform.parent.GetComponent<Wave>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            waveScript.TriggerWaveSpawn();
    }
}
