using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreSingletonUnityEventWrapper : MonoBehaviour
{

    public void SetScore(float newScore)
    {
        ScoreManager.instance.SetScore(newScore);
    }
    public void IncrementScore(float newScore)
    {
        ScoreManager.instance.IncrementScore(newScore);
    }
    public void DecrementScore(float newScore)
    {
        ScoreManager.instance.DecrementScore(newScore);
    }
    public void DestroyObj(GameObject obj)
    {
        Destroy(obj, 3f);
    }
}
