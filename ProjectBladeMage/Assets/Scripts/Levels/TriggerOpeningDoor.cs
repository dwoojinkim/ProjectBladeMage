using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TriggerOpeningDoor : MonoBehaviour
{
    public enum DoorTriggerType
    {
        WaveCompletion,
        Key
    };

    public DoorTriggerType DoorTrigger;
    public GameObject Trigger;

    private Wave triggerWave;

    [SerializeField] private float moveDistance = 10f;      // Positive number = move up; Negative = move down;
    [SerializeField] private float moveDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();

        if (DoorTrigger == DoorTriggerType.WaveCompletion)
        {
            triggerWave = Trigger.GetComponent<Wave>();
            triggerWave.WaveComplete += OpenDoor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenDoor()
    {
        Debug.Log("Opening door...");
        transform.DOMoveY(transform.position.y + moveDistance, moveDuration);
        triggerWave.WaveComplete -= OpenDoor;
    }
}
