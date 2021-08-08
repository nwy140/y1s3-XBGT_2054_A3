using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVolumeSlider : MonoBehaviour
{
    public Slider slider;
    public bool isOverallVolume;
    public bool isSFXVolume;
    public bool isBGMVolume;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
    }
    private void Start()
    {
        SyncVolumes();
    }
    private void Update()
    {
        SyncVolumes();
    }
    void SyncVolumes()
    {
        if (AudioManager.instance != null)
        {
            if (isOverallVolume)
                slider.value = AudioManager.instance.overallVolume;
            else if (isSFXVolume)
                slider.value = AudioManager.instance.SFXMasterVolume;
            else if (isBGMVolume)
                slider.value = AudioManager.instance.BGMMasterVolume;
        }
    }

    public void OnOverallVolumeSliderValueChanged()
    {
        if (slider != null)
        {
            AudioManager.instance.overallVolume = slider.value;
            AudioManager.instance.SaveVolumeSettingsPlayerPrefs();
        }
    }
    public void OnSFXVolumeSliderValueChanged()
    {
        if (slider != null)
        {
            AudioManager.instance.SFXMasterVolume = slider.value;
            AudioManager.instance.SaveVolumeSettingsPlayerPrefs();
        }
    }
    public void OnBGMVolumeSliderValueChanged()
    {
        if (slider != null)
        {
            AudioManager.instance.BGMMasterVolume = slider.value;
            AudioManager.instance.SaveVolumeSettingsPlayerPrefs();
        }
    }
}
