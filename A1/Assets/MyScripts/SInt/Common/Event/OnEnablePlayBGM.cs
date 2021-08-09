using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnablePlayBGM : MonoBehaviour
{
    public string BGMName;
    private void Start()
    {
        if (AudioManager.instance)
            AudioManager.instance.PlayBGM(BGMName);
    }
    private void OnEnable()
    {
        if(AudioManager.instance)
            AudioManager.instance.PlayBGM(BGMName);
    }
}
