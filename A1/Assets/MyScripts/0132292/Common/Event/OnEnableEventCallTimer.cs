using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnEnableEventCallTimer : MonoBehaviour
{
    public float timerInSeconds = 3f;
    public bool isAllowDisable = false;
    public bool isCountUpTimer = false;
    public bool isTimerHasLimit = true;
    public Text timerText;

    [Header("For Debug/Inspector View Only")]
    public float targetTime;

    private void OnEnable()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        targetTime = timerInSeconds;
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
    }

    public UnityEvent eventToCall;

    //public bool isPlayOnDelay = false; TODO: Optional for extending Functionality of this script
    public void PlayEventToCall()
    {
        //transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        eventToCall.Invoke();
    }
}