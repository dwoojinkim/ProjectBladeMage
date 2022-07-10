using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager LMinstance = null;

    public GameObject Player;
    public GameObject GameTimerObj;
    public GameObject PlayerHPObj;
    public GameObject CameraTracker;
    public GameObject ObstacleSpawner;


    private TextMeshProUGUI GameTimerText;
    private TextMeshProUGUI PlayerHPText;

    private float gameTime = 0.0f;
    private int highScore = 0;
    private float resetDistance = 25;
    private Vector3 playerResetPos;
    private Vector3 cameraResetPos;


    // Start is called before the first frame update
    void Start()
    {
        if (LMinstance == null)
        {
            LMinstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


            GameTimerText = GameTimerObj.GetComponent<TextMeshProUGUI>();
            PlayerHPText = PlayerHPObj.GetComponent<TextMeshProUGUI>();

            playerResetPos = Player.transform.position;
            cameraResetPos = CameraTracker.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        GameTimerText.text = ((int)gameTime).ToString();
        PlayerHPText.text = "Player HP: " + Player.GetComponent<Player>().HP.ToString();
    }

    public void ResetGame()
    {
        gameTime = 0f;

        CameraTracker.transform.position = cameraResetPos;
        Player.transform.position = playerResetPos;
        ObstacleSpawner.GetComponent<ObstacleSpawner>().ResetSpawner();
    }
}
