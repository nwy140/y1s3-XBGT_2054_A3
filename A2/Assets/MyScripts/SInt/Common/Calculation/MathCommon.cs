using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Static Class for Math Related methods // Our Own Custom Mathf Like Class
public static class MathCommon
{
    #region OnGUI
    public static float CalculateSliderValuePercentage(Slider sliderUI, float currValue, float maxValue, float lerpTime)
    {
        float lerpValue = 0;
        if (maxValue > 0) // can't divide by 0
        {
            if (sliderUI != null)
            {
                lerpValue = Mathf.Clamp(Mathf.Lerp(sliderUI.value, currValue / maxValue, lerpTime), 0, 1); // Sliders cannot be negative, so this has to be clamped for visual purposes
                sliderUI.value = lerpValue;
            }
        }
        // return slider value as Percentage in decimal float format between 0.0 to 1.0
        return sliderUI.value;
        //return lerpValue;  // for debug.log return value  if needed
    }

    // Please Follow https://docs.unity3d.com/ScriptReference/Mathf.Lerp.html
    public static float CalculateLerpValuePercentage(float currValue, float maxValue, float lerpTime)
    {
        float lerpValue = 0.001f;
        if (currValue == 0)
        { // should not be zero, could cause lerping bugs, replace with small floating point instead
            currValue = 0.001f;
        }
        if (maxValue != 0) // can't divide by 0
        {
            float lerpTarget = currValue / maxValue;
            lerpValue = Mathf.Lerp(lerpValue, lerpTarget, lerpTime);
            Debug.Log("Lerp Value : " + lerpValue + " Lerp Target" + lerpTarget + "currValue: " + currValue + "maxValue: " + maxValue);
        }
        // return value as Percentage in decimal float format between -0.0 to 1.0
        return lerpValue;
        //lerpValue = Mathf.Lerp(lerpValue, currValue / maxValue, lerpTime);
    }
    #endregion OnGUI
    #region Improved Mathf

    // Ref: https://youtu.be/62IFyHUdH9U?list=PLuLJclBWmeWWe_n5PMPvObez_PATtcxvG and https://youtu.be/gqU1t1jpmDw?list=PLZpDYt0cyiusT185fsSTEU1ecr8CcTYMP

    public static Vector3 lerpLinearVector3(Vector3 start, Vector3 end, float timeStartedLerping, float lerpTime = 1) {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeStartedLerping / lerpTime;

        Vector3 result = Vector3.Lerp(start, end, percentageComplete);
        return result;
    }
    public static float lerpLinearFloat(float startVal, float endVal, float timeStartedLerping, float lerpTime = 1)
    {
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;
        start.x = startVal;
        end.x = endVal;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeStartedLerping / lerpTime;

        float result = Vector3.Lerp(start, end, percentageComplete).x;
        return result;
    }
    #endregion Improved Mathf

    #region Color Math

    // Wrapper for Color Utilty ParseHex via our own custom method
    public static Color ColorParseHex(string hexValue) {
        Color color = Color.white; // default color will be returned if Parsing Hex value fails
        ColorUtility.TryParseHtmlString(hexValue, out color); // out parameter
        return color;
    }

    // Ref: https://answers.unity.com/questions/128155/negative-color-scheme.html
    public static Color ColorInvert(Color color)
    {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }
    #endregion Color Math

}
