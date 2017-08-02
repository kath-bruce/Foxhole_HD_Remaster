using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score
{
    public string Player { get; set; }

    public float Time { get; set; }

    public Score()
    {
        Player = "";
        Time = 0f;
    }

    public Score(string playerName, float playerTime)
    {
        Player = playerName;
        Time = playerTime;
    }

    public override string ToString()
    {
        return Player + " " + Time.ToString("0.000000") + "s";
    }
}
