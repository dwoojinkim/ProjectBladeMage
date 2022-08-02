using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script used by the DummyCamera to simulate camera moving to the side
public class CameraScroller : MonoBehaviour
{
    private const float MAX_SPEED_CAP = 10.0f;

    private const float INIT_CAMERA_MOVESPEED = 5.0f;

    [SerializeField] private FloatVariable stageSpeedData;
    [SerializeField] private GameObjectReference currentStageObject;
    
    private Level currentStage;

    private float timeToIncreaseSpeed = 10.0f;
    private float speedIncreaseTimer = 0.0f;
    private float increaseSpeedAmount = 1.0f;


    private Vector3 lastPlayerPosition;

    private float stageSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentStage = currentStageObject.gameObject.GetComponent<Level>();
        stageSpeed = INIT_CAMERA_MOVESPEED;
        currentStage.LevelComplete += StopStage;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCheck();
    }

    void FixedUpdate()
    {
        MoveStage();
    }

    private void MoveStage()
    {
        transform.position += transform.right * stageSpeed * Time.fixedDeltaTime;
    }

    private void SpeedCheck()
    {
        if (stageSpeed < MAX_SPEED_CAP)
        {
            speedIncreaseTimer += Time.deltaTime;
            if (speedIncreaseTimer >= timeToIncreaseSpeed)
            {
                speedIncreaseTimer = 0.0f;

                stageSpeed += increaseSpeedAmount;
                if (stageSpeed > MAX_SPEED_CAP)
                    stageSpeed = MAX_SPEED_CAP;

                //Debug.Log("Increasing Speed!!!. Speed is now: " + stageSpeed);
            }
        }

        stageSpeedData.SetValue(stageSpeed);
    }

    private void StopStage()
    {
        Debug.Log("Stopping Stage!");
        currentStage.LevelComplete -= StopStage;
    }
}
