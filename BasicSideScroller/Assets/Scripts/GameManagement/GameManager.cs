using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject GameTimerObj;
    public GameObject Player;
    public GameObject ObstacleSpawner;
    public GameObject bg1;
    public GameObject bg2;
    public GameObject bg3;
    public GameObject bg4;

    public bool pushback = false;

    private float gameTime = 0.0f;
    private float resetDistance = 100f;

    private float camHeight;
    private float camWidth;

    private Text GameTimerText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
 
            GameTimerText = GameTimerObj.GetComponent<Text>();
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
    }

    private void FixedUpdate()
    {
         if (Player.transform.position.x >= resetDistance)
        {
            ObstacleSpawner.GetComponent<ObstacleSpawner>().MoveObstaclesBack(resetDistance);
            Player.GetComponent<Player>().MovePlayerBack(resetDistance);
            bg1.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
            bg2.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
            bg3.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
            bg4.GetComponent<ParallaxBackground>().MoveBackground(resetDistance);
        } 
    }

    public void ResetGame()
    {

        gameTime = 0f;
    }
}
