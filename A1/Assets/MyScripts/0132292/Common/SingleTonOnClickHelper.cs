using UnityEngine;
using UnityEngine.Events;

public class SingleTonOnClickHelper : MonoBehaviour
{
    // A Wrapper Class to SingleTon Classes such as AudioManager
    public void AudioManPlaySFX(string name)
    {
        if (AudioManager.instance)
            AudioManager.instance.PlaySFX(name);
    }
    public void AudioManPlayBGM(string name)
    {
        if (AudioManager.instance)
            AudioManager.instance.PlayBGM(name);
    }

    public void AudioManResetVolumeSettingsToDefault()
    {
        if (AudioManager.instance)
            AudioManager.instance.ResetVolumeSettingsToDefault();
    }
    public void AudioManSaveVolumeSettingsPlayerPrefs()
    {
        if (AudioManager.instance)
            AudioManager.instance.SaveVolumeSettingsPlayerPrefs();
    }


    // Common Onclick Methods
    public void CommonTurnOnOffObjVisibility(GameObject obj)
    {
        obj.SetActive(!obj.activeInHierarchy);
    }

    // Common Onclick Methods
    public void CommonSetSelfVisibilityByObjVisibility(GameObject obj)
    {
        gameObject.SetActive(!obj.activeInHierarchy);
    }
    // Common Onclick Methods
    public void CommonSetSelfVisibilityByObjVisibilityInverted(GameObject obj)
    {
        gameObject.SetActive(!!obj.activeInHierarchy);
    }

    #region Unused
    // https://forum.unity.com/threads/how-to-call-animator-setbool-using-unityevent.745361/
    //Common Onclick Methods
    // Needs a AnimatorUnityEventHandler component attached to obj passed
    //public void CommonTurnOnOffObjAnimatorBoolParameter(AnimationEventHelper obj)
    //{
    //    //obj.GetComponent<AnimatorUnityEventHandler>().TurnBoolOnOff;
    //}
    #endregion Unused
}