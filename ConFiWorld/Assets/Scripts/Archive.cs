using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archive : Singleton<Archive>
{
    public void SetTotalScore(int total)
    {
        PlayerPrefs.SetInt("TotalScore", total);
        PlayerPrefs.Save();
    }
    public int GetTotalScore()
    {
        return PlayerPrefs.GetInt("TotalScore", 0);
    }

    public void SetMaxScore(int score)
    {
        PlayerPrefs.SetInt("MaxScore", score);
        PlayerPrefs.Save();
    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("MaxScore", 0);
    }
}
