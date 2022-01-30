using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    private const float MAX_SPEED_CAP = 25.0f;

    private const float INIT_CAMERA_MOVESPEED = 5.0f;

    private float timeToIncreaseSpeed = 1.0f;
    private float speedIncreaseTimer = 0.0f;
    private float increaseSpeedAmount = 1.0f;


    [SerializeField] private Transform cameraTrackerTransform;
    private Vector3 lastPlayerPosition;

    private float stageSpeed;

    // Start is called before the first frame update
    void Start()
    {
        stageSpeed = INIT_CAMERA_MOVESPEED;
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

    }
}
