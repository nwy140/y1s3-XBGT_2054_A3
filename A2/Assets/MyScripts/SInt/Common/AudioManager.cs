using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// top update to other project

// Brackey's Audio Manager // Modified
// https://youtu.be/6OT43pvUyfY?list=PLuLJclBWmeWWe_n5PMPvObez_PATtcxvG
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Settings Default Volume")] [Range(0, 1f)]
    public float defaultSFXMasterVolume = 0.75f;

    [Range(0, 1f)] public float defaultBGMMasterVolume = 0.8f;
    [Range(0, 1f)] public float defaultOverallVolume = 0.3f;
    public bool isVolumeSettingSaved;

    [Header("Audio Settings Current Volume")] [Range(0, 1f)]
    public float SFXMasterVolume = 1f;

    [Range(0, 1f)] public float BGMMasterVolume = 1f;
    [Range(0, 1f)] public float overallVolume = 1f;
    public Sound currSFX;
    public Sound currBGM;
    public GameObject multiAudioSourceObj;
    public List<Sound> soundsSFX;
    public List<Sound> soundsBGM;

    Transform sfxChild;
    Transform bgmChild;

    string currSceneName = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // cant have multiple singletons in one scene
        }

        //GenerateSoundListByEnumType();

        foreach (Sound s in soundsSFX)
        {
            sfxChild = transform.GetChild(0);
            s.source = sfxChild.gameObject.AddComponent<AudioSource>();
            s.initialVolume = s.volume;
            InitNewAudioSFXSource(s);
            s.source.Stop();
        }

        foreach (Sound s in soundsBGM)
        {
            bgmChild = transform.GetChild(1);
            s.source = bgmChild.gameObject.AddComponent<AudioSource>();
            s.initialVolume = s.volume;
            InitNewAudioBGMSource(s);
            s.source.Stop();
        }

        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        ResetVolumeSettingsToDefault();
        isVolumeSettingSaved = PlayerPrefs.GetInt("isVolumeSettingSaved") == 1;
        if (isVolumeSettingSaved == true)
        {
            LoadVolumeSettingsPlayerPrefs();
        }
    }

    public void ResetVolumeSettingsToDefault()
    {
        overallVolume = defaultOverallVolume;
        BGMMasterVolume = defaultBGMMasterVolume;
        SFXMasterVolume = defaultSFXMasterVolume;
    }

    public void LoadVolumeSettingsPlayerPrefs()
    {
        overallVolume = PlayerPrefs.GetFloat("OverallVolumeMaster");
        BGMMasterVolume = PlayerPrefs.GetFloat("BGMVolumeMaster");
        SFXMasterVolume = PlayerPrefs.GetFloat("SFXVolumeMaster");
        isVolumeSettingSaved = PlayerPrefs.GetInt("isVolumeSettingSaved") == 1; // 0 for false, 1 true
        //Debug.Log("Load"+PlayerPrefs.GetFloat("OverallVolumeMaster") + "  " + PlayerPrefs.GetInt("isVolumeSettingSaved"));
    }

    public void SaveVolumeSettingsPlayerPrefs()
    {
        isVolumeSettingSaved = true;
        PlayerPrefs.SetFloat("OverallVolumeMaster", overallVolume);
        PlayerPrefs.SetFloat("BGMVolumeMaster", BGMMasterVolume);
        PlayerPrefs.SetFloat("SFXVolumeMaster", SFXMasterVolume);
        PlayerPrefs.SetInt("isVolumeSettingSaved", isVolumeSettingSaved ? 1 : 0); // 0 for false, 1 true
        //Debug.Log("Saved"+ PlayerPrefs.GetFloat("OverallVolumeMaster") + "  " + PlayerPrefs.GetInt("isVolumeSettingSaved"));
    }

    private void Update()
    {
        // Update General Volume
        if (AudioListener.volume != overallVolume)
        {
            AudioListener.volume = overallVolume;
        }

        foreach (Sound s in soundsSFX)
        {
            s.source.volume = s.volume * SFXMasterVolume;
            // Quick find and replace
            //s.name = s.name.Replace("", "Stun");
        }

        foreach (Sound s in soundsBGM)
        {
            s.source.volume = s.volume * BGMMasterVolume;
        }

        OnSceneChanged();
    }

    void InitNewAudioSFXSource(Sound s)
    {
        s.source.loop = s.loop;
        s.source.volume = s.volume * SFXMasterVolume;
        s.source.pitch = s.pitch;
        s.source.clip = s.clip;
    }

    void InitNewAudioBGMSource(Sound s)
    {
        s.source.loop = s.loop;
        s.source.volume = s.volume * BGMMasterVolume;
        s.source.pitch = s.pitch;
        s.source.clip = s.clip;
    }

    void OnSceneChanged()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene.isLoaded && activeScene.name != currSceneName)
        {
            // Your Code Here
            currSceneName = activeScene.name;
            PlayBGM("Scene_" + currSceneName);
        }
    }

    public void PlaySFX(string sfxName)
    {
        Sound s = Array.Find(soundsSFX.ToArray(), sound => sound.name == sfxName);
        if (s != null)
        {
            currSFX = s;
            if (s.isAllowMultiAudioSrcInstance)
            {
                GameObject newObj = Instantiate(multiAudioSourceObj, sfxChild.transform);
                AudioSource audioSrc = newObj.GetComponent<AudioSource>();
                AudioSource originalSrc = s.source;
                s.source = audioSrc;
                InitNewAudioSFXSource(s);
                audioSrc.Play();
                Destroy(newObj, audioSrc.clip.length + 1);
                s.source = originalSrc;
            }
            else
            {
                s.source.Play();
            }

            Debug.Log(sfxName + " SFX sound being played. " + sfxName + " found!");
        }
        else
        {
            Debug.Log(sfxName + " SFX sound could not be played. " + sfxName + " not found!");
        }
    }

    public void PlayBGM(string bgmName)
    {
        Sound s = Array.Find(soundsBGM.ToArray(), sound => sound.name == bgmName);
        AudioSource sourceToPlay;
        if (s != null)
        {
            currBGM = s;
            foreach (Sound bgm in soundsBGM)
            {
                if (bgm.name != bgmName)
                {
                    if (bgm.source != null)
                    {

                        bgm.source.Stop();
                    }
                }
            }
            if (s.isAllowMultiAudioSrcInstance)
            {
                GameObject newObj = Instantiate(multiAudioSourceObj, bgmChild.transform);
                AudioSource audioSrc = newObj.GetComponent<AudioSource>();
                AudioSource originalSrc = s.source;
                s.source = audioSrc;

                InitNewAudioBGMSource(s);
                sourceToPlay = audioSrc;
                Destroy(newObj, audioSrc.clip.length + 1);
                s.source = originalSrc;
            }
            else
            {
                sourceToPlay = s.source;
            }

            // if (sourceToPlay.isPlaying == false)
            // {
                sourceToPlay.Play();
                Debug.Log(bgmName + " BGM sound being played");
            // }
        }
        else
        {
            Debug.Log(bgmName + " BGM sound could not be played. Sound not found!");
        }
    }

    // FadeOut BGM Stop
    public void StopBGM(string name)
    {
        Sound s = Array.Find(soundsBGM.ToArray(), sound => sound.name == name);
        if (s != null)
        {
            // Play BGM FadeOut
            s.source.Stop();
        }
    }
}

// AudioFade class From https://stackoverflow.com/questions/57527257/audio-fade-in-out-with-c-sharp-in-unity#comment101622084_57529081
public class AudioFade
{
    public static IEnumerator FadeOut(Sound sound, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        float startVolume = sound.source.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.source.volume = Interpolate(startVolume, 0, t);
            //sound.volume = sound.source.volume;
            yield return null;
        }

        sound.source.volume = 0;
        sound.source.Stop();
    }

    public static IEnumerator FadeIn(Sound sound, float fadingTime, Func<float, float, float, float> Interpolate)
    {
        sound.source.Play();
        sound.source.volume = 0;

        float resultVolume = sound.volume;
        float frameCount = fadingTime / Time.deltaTime;
        float framesPassed = 0;

        while (framesPassed <= frameCount)
        {
            var t = framesPassed++ / frameCount;
            sound.source.volume = Interpolate(0, resultVolume, t);
            //sound.volume = sound.source.volume;

            yield return null;
        }

        sound.source.volume = resultVolume;
        //sound.volume = sound.source.volume;
    }
}