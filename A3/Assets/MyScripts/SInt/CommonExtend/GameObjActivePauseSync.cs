using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjActivePauseSync : MonoBehaviour
{
    public bool isPaused;

    private void OnEnable()
    {
        SyncActivePauseVariables();
    }
    private void OnDisable()
    {
        SyncActivePauseVariables();
    }
    public void SyncActivePauseVariables()
    {
        isPaused = isActiveAndEnabled;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
