using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnEnableEventCallTimer : MonoBehaviour
{
    [Header("Default TimerInSeconds - if isUseRandomRangeTimerInSeconds is false")]
    public float timerInSeconds = 3f;
    public bool isUseIntervalTimer; //random

    public bool isAllowDisable = false;
    public bool isCountUpTimer = false;
    public bool isTimerHasLimit = true;
    public Text timerText;

    public bool isLoopTimer;

    public bool isUseRandomRangeTimerInSeconds;
    [Header("Random Range MinMax TimerInSeconds - if isUseRandomRangeTimerInSeconds is true")]
    public float minTimerInSeconds;
    public float maxTimerInSeconds;

    [Header("For Debug/Inspector View Only")]
    public float targetTime;

    private void OnEnable()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        if (isUseRandomRangeTimerInSeconds == false)
        {
            targetTime = timerInSeconds;
        }
        else
        {
            targetTime = Random.Range(minTimerInSeconds,maxTimerInSeconds);
        }
        if (timerText != null)
        {
            timerText.text = (targetTime).ToString("F0");
        }
    }

    void Update()
    {
        //https://answers.unity.com/questions/351420/simple-timer-1.html
        if (isCountUpTimer == true)
        {
            targetTime += Time.deltaTime;
        }
        else
        {
            targetTime -= Time.deltaTime;
        }

        if (isTimerHasLimit)
        {
            if (isCountUpTimer == true)
            {
                if (targetTime >= timerInSeconds * 2f)
                {
                    timerEnded();
                }
            }
            else
            {
                if (targetTime <= 0.0f)
                {
                    timerEnded();
                }
            }
        }

        if (timerText != null)
        {
            if (timerText.gameObject.activeInHierarchy)
            {
                timerText.text = (targetTime).ToString("F0");
            }
        }
    }

    void timerEnded()
    {
        //do your stuff here.
        if (isAllowDisable)
        {
            gameObject.SetActive(false);
        }

        PlayEventToCall();

        if (isLoopTimer)
        {
            ResetTimer();
        }
    }

    public UnityEvent eventToCall;

    //public bool isPlayOnDelay = false; TODO: Optional for extending Functionality of this script
    public void PlayEventToCall()
    {
        //transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        eventToCall.Invoke();
    }
}