using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop = false;
    // Allow Spawn Multiple Audio Source of same sound type
    public bool isAllowMultiAudioSrcInstance = false; 

    [Range(0, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    [HideInInspector]
    public float initialVolume;
    
    //[HideInInspector]
    public AudioSource source;
}
