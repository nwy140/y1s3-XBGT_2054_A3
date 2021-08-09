using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPSliderColor : MonoBehaviour
{

    public enum UIColorTransition
    {
        //Reference: https://bulbapedia.bulbagarden.net/wiki/HP#:~:text=be%20healed%20automatically.-,HP%20bar,for%20each%20of%20their%20Pok%C3%A9mon
        High, // green
        Med, // orange/yellow
        Low // red
    }

    public Color high = Color.green;
    public Color med = Color.yellow;
    public Color low = new Color(1.0f, 0.64f, 0.0f);

    public UIColorTransition currColorTransition;
    public Slider slider;

    void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
            SetCurrColorTransitionBySliderValue();
        }
    }
    // TODO Optional: Lerp slider Colors 

    //Called on Value Changed
    public void SetCurrColorTransitionBySliderValue()
    {

        //Ref: https://answers.unity.com/questions/792008/how-to-change-normal-color-highlighted-color-etc-i.html
        if (slider.value > 0.51f) // High
        {
            currColorTransition = UIColorTransition.High;
            ColorBlock cb = slider.colors;
            cb.disabledColor = high;
            slider.colors = cb;
        }
        else if (slider.value <= 0.51f && slider.value > 0.251f) // Med
        {
            currColorTransition = UIColorTransition.Med;
            ColorBlock cb = slider.colors;
            cb.disabledColor = med;
            slider.colors = cb;
        }
        else if ((slider.value <= 0.251f && slider.value >= 0)) // Low
        {
            currColorTransition = UIColorTransition.Low;
            ColorBlock cb = slider.colors;
            cb.disabledColor = low;
            slider.colors = cb;
        }
        else
        {
            currColorTransition = UIColorTransition.Low;
            ColorBlock cb = slider.colors;
            cb.disabledColor = low;
            slider.colors = cb;
        }
    }



}
