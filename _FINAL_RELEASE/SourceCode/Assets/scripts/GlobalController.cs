using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController INSTANCE { get; protected set; }

    List<Score> scores;

    public string CurrentPlayerName { get; set; } //used for saving the player score

    public bool CurrentPlayerHighScore
    {
        get
        {
            return currentPlayerHighScore;
        }

        set
        {
            currentPlayerHighScore = value;
        }
    }

    private bool currentPlayerHighScore = false; //need to be able to set default value

    public string LastLevelName { get; set; }

    public AudioSource audioSrc; //need a component as using AudioSource.PlayClipAtPoint() 
                                    //creates GO that gets destroyed on loading new scenes

    void Awake()
    {
        if (INSTANCE == null)
        {
            DontDestroyOnLoad(gameObject);
            INSTANCE = this;
            ScoresHandler.CreateScoresFile(); //ensures scores file exists - will not create copies
            scores = ScoresHandler.ReadScores().ToList();
        }
        else if (INSTANCE != this)
        {
            Destroy(gameObject);
        }

    }

    public void AddToScores(Score score)
    {
        scores.Add(score);

        scores = ScoresHandler.TopScores(scores.ToArray()).ToList(); //find the top 5 scores

        ScoresHandler.SaveScores(scores.ToArray()); //save the top scores

        if (scores.Contains(score)) //if the current player's score is still in the top scores
        {
            //player achieved top score
            CurrentPlayerHighScore = true;
        }
    }

    public Score[] GetScores()
    {
        return scores == null ? null : scores.ToArray();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSrc.clip = clip;
        audioSrc.Play();
    }
}
