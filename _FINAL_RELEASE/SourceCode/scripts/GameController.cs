using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE { get; protected set; }

    public GameObject player;

    Vector3 respawnPointVec;

    public GameObject respawnText;
    const float respawn_text_display_reset = 2.0f;
    float respawn_text_display = respawn_text_display_reset;

    public Text countdownText;
    bool inCountdown = true;
    float countdownTimer = 3f;

    public Text timerText;
    float timer = 0f;

    string playerName;

    bool paused = false;

    public GameObject pausedText;

    public GameObject btnBack;

    float delta_time;

    public AudioClip splash;

    // Use this for initialization
    void Start()
    {
        if (INSTANCE != null)
        {
            Debug.LogError("MORE THAN ONE GAMECONTROLLER!!!!");
        }
        else
        {
            INSTANCE = this;
        }

        timerText.text = timer.ToString("0.000000") + "s";

        respawnPointVec = player.transform.position;

        playerName = GlobalController.INSTANCE.CurrentPlayerName; //not strictly necessary
    }

    public void WaterCollision()
    {
        AudioSource.PlayClipAtPoint(splash, respawnPointVec);
        RespawnText(true);
        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        player.transform.position = respawnPointVec;
    }

    void RespawnText(bool activateText)
    {
        respawn_text_display = respawn_text_display_reset;
        respawnText.SetActive(activateText);
    }

    public void LevelComplete()
    {
        GlobalController.INSTANCE.AddToScores(new Score(playerName, timer));
        LevelLoader.INSTANCE.LoadLevel("_scores");
    }

    public bool GamePaused()
    {
        return paused || inCountdown;
    }

    void TimerText()
    {
        timer += delta_time;
        timerText.text = timer.ToString("0.000000") + "s";
    }

    void CountdownTimerText()
    {
        countdownTimer -= delta_time;

        if (countdownTimer > 2)
            countdownText.text = "3";
        else if (countdownTimer > 1)
            countdownText.text = "2";
        else if (countdownTimer > 0)
            countdownText.text = "1";
        else if (countdownTimer < 0)
        {
            inCountdown = false;
            countdownText.text = "GO!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        delta_time = Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Tab)) //paused
        {
            paused = !paused;
            CameraController.INSTANCE.Pause(paused);
            pausedText.SetActive(paused);
            btnBack.SetActive(paused);

            AudioSource audioSrc = GetComponent<AudioSource>(); //needed to pause and unpause environment sounds

            if (paused)
                audioSrc.Pause();
            else
                audioSrc.Play();
        }

        #region countdown

        if (inCountdown && !paused)
        {
            CountdownTimerText();
        }
        else if (timer > 1)
        {
            countdownText.text = "";
        }

        #endregion

        #region timer text display

        if (!paused && !inCountdown)
        {
            TimerText();
        }

        #endregion

        #region respawn text display
        if (respawnText.activeInHierarchy && respawn_text_display > 0)
        {
            respawn_text_display -= delta_time;
        }
        else
        {
            RespawnText(false);
        }
        #endregion
        
    }
}
