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
    void Awake()
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

        GenerateLevelTree();

    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        GameTimerText.text = ((int)gameTime).ToString();
        //PlayerHPText.text = "Player HP: " + Player.GetComponent<Player>().HP.ToString();
    }

    public void ResetGame()
    {
        gameTime = 0f;

        CameraTracker.transform.position = cameraResetPos;
        Player.transform.position = playerResetPos;
        ObstacleSpawner.GetComponent<ObstacleSpawner>().ResetSpawner();
    }
    
    private void GenerateLevelTree()
    {
        // Temp variable to test level generation.
        // Will be using Scriptable Objects later.
        int numLevels = 5;  // Number of levels per Act
        int numActs = 3;
        Random.seed = 18;
        int choice1 = -1;
        int choice2 = -1;

        Debug.Log("Possible Level Choices:");

        for (int i = 1; i <= numActs; i++)
        {
            for (int j = 1; j <= numLevels; j++)
            {
                choice1 = Random.Range(1,6);
                choice2 = Random.Range(1,6);

                Debug.Log("Level " + i + "-" + j + " will be: " + choice1 + " or " + choice2);
            }
        }
    }
}