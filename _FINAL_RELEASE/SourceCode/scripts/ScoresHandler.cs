using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class ScoresHandler
{
    public static bool CreateScoresFile()
    {
        try
        {
            StreamWriter sw = new StreamWriter(@"scores.txt");
            sw.Write("");
            sw.Close();
        }
        catch
        {
            return false;
        }
        return true;
    }

    public static bool SaveScores(Score[] scores)
    {
        try
        {
            StreamWriter sw = new StreamWriter(@"scores.txt");

            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i].Player.Length < 10)
                    scores[i].Player = scores[i].Player.PadRight(10);

                sw.Write(scores[i].Player + " ");

                sw.WriteLine(scores[i].Time);
            }

            sw.Close();
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static Score[] TopScores(Score[] scoresSoFar, int noOfTopScores = 5)
    {
        bool flag = true;
        Score temp;

        while (flag)
        {
            flag = false;

            for (int i = 0; i < scoresSoFar.Length - 1; i++)
            {
                if (scoresSoFar[i].Time > scoresSoFar[i + 1].Time)
                {
                    temp = scoresSoFar[i];

                    scoresSoFar[i] = scoresSoFar[i + 1];

                    scoresSoFar[i + 1] = temp;

                    flag = true;
                }
            }
        }

        if (scoresSoFar.Length > noOfTopScores)
        {
            Score[] topScores = new Score[noOfTopScores];

            for (int j = 0; j < topScores.Length; j++)
            {
                topScores[j] = scoresSoFar[j];
            }

            return topScores;
        }
        else
        {
            return scoresSoFar;
        }
    }

    public static Score[] ReadScores()
    {
        List<Score> scores = new List<Score>();

        try
        {
            StreamReader sr = new StreamReader(@"scores.txt");

            while (!sr.EndOfStream)
            {
                Score temp = new Score();
                char[] name = new char[10];

                sr.Read(name, 0, name.Length);
                temp.Player = new string(name);

                sr.Read(); // the space in between the name and the time

                temp.Time = System.Convert.ToSingle(sr.ReadLine());

                scores.Add(temp);
            }

            sr.Close();
        }
        catch
        {
            return null;
        }

        return scores.ToArray();
    }
}
