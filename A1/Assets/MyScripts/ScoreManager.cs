using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;
    public string scorePlayerPrefsFloatName = "Score";
    public string highScorePlayerPrefsFloatName = "HighScore";

    void Awake()
    {
        if (ScoreManager.instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        PlayerPrefs.SetFloat(scorePlayerPrefsFloatName, 0);

        if (PlayerPrefs.HasKey(highScorePlayerPrefsFloatName) == false)
        {
            PlayerPrefs.SetFloat(highScorePlayerPrefsFloatName, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetFloat(scorePlayerPrefsFloatName) >= PlayerPrefs.GetFloat(highScorePlayerPrefsFloatName))
        {
            PlayerPrefs.SetFloat(highScorePlayerPrefsFloatName, PlayerPrefs.GetFloat(scorePlayerPrefsFloatName));
        }
    }

    public void SetScore(float newScore)
    {
        PlayerPrefs.SetFloat(scorePlayerPrefsFloatName, newScore);
    }
    public void IncrementScore(float newScore)
    {
        PlayerPrefs.SetFloat(scorePlayerPrefsFloatName, PlayerPrefs.GetFloat(scorePlayerPrefsFloatName) + newScore);
    }
    public void DecrementScore(float newScore)
    {
        PlayerPrefs.SetFloat(scorePlayerPrefsFloatName, PlayerPrefs.GetFloat(scorePlayerPrefsFloatName) - newScore);
    }
}
