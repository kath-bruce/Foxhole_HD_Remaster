using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoresController : MonoBehaviour
{
    public List<Text> nameTexts;
    public List<Text> scoreTexts;
    public GameObject txtNoScoresYet;
    public Text txtHighScore;

    void Start()
    {
        Cursor.visible = true;

        Score[] scores = GlobalController.INSTANCE.GetScores();
        
        if (scores.Length > 0)
        {
            for (int i = 0; i < scores.Length; i++)
            {
                nameTexts[i].text = (i+1) + ". " + scores[i].Player;
                scoreTexts[i].text = scores[i].Time.ToString("0.000000") + "s";
            }

            for (int j = scores.Length; j < scoreTexts.Count; j++)
            {
                nameTexts[j].text = "";
                scoreTexts[j].text = "";
            }
        }
        else
        {
            for (int i = 0; i < scoreTexts.Count; i++)
            {
                if (i == 0)
                {
                    nameTexts[i].text = "";
                    scoreTexts[i].text = "";
                }
                else
                {
                    nameTexts[i].text = "";
                    scoreTexts[i].text = "";
                }
            }

            txtNoScoresYet.SetActive(true);
        }

        if (GlobalController.INSTANCE.CurrentPlayerHighScore)
        {
            txtHighScore.text = "High Score! - " + GlobalController.INSTANCE.CurrentPlayerName;

            GlobalController.INSTANCE.CurrentPlayerHighScore = false;
        }
        else if (GlobalController.INSTANCE.LastLevelName == "_first_level")
        {
            txtHighScore.text = "No high score this time! :(";
        }
    }
}
