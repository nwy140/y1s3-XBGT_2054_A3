using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnablePlaySFX : MonoBehaviour
{
    public string SFXName;
    private void Start()
    {
        if (AudioManager.instance)
            AudioManager.instance.PlaySFX(SFXName);
    }
    private void OnEnable()
    {
        if(AudioManager.instance)
            AudioManager.instance.PlaySFX(SFXName);
    }
}
