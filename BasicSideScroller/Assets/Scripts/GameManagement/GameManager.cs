using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject GameTimerObj;
    public GameObject PlayerHPObj;
    public GameObject Player;
    public GameObject CameraTracker;
    public GameObject ObstacleSpawner;
    public GameObject bg1;
    public GameObject bg2;
    public GameObject bg3;
    public GameObject bg4;

    public bool pushback = false;

    private float gameTime = 0.0f;
    private int highScore = 0;
    private float resetDistance = 25;
    private Vector3 playerResetPos;
    private Vector3 cameraResetPos;

    private float camHeight;
    private float camWidth;

    private Text GameTimerText;
    private Text PlayerHPText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
 
            GameTimerText = GameTimerObj.GetComponent<Text>();
            PlayerHPText = PlayerHPObj.GetComponent<Text>();
            playerResetPos = Player.transform.position;
            cameraResetPos = CameraTracker.transform.position;

            Debug.Log(cameraResetPos.y);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        GameTimerText.text = ((int)gameTime).ToString();
        PlayerHPText.text = "Player HP: " + Player.GetComponent<Player>().Health.ToString();
    }

    private void FixedUpdate()
    {
         if (CameraTracker.transform.position.x >= resetDistance)
        {
            //AdjustBG();
        } 
    }

    public void ResetGame()
    {
        gameTime = 0f;

        CameraTracker.transform.position = cameraResetPos;
        Player.transform.position = playerResetPos;
        ObstacleSpawner.GetComponent<ObstacleSpawner>().ResetSpawner();
    }

    // Move the background so it loops seamlessly as it is moving
    private void AdjustBG()
    {
        ObstacleSpawner.GetComponent<ObstacleSpawner>().MoveObstaclesBack(resetDistance);
        CameraTracker.transform.position -= transform.right * resetDistance;
        bg1.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
        bg2.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
        bg3.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
        bg4.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
    }

}
