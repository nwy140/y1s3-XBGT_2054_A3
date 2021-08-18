using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.Events;

public class UnitStatHP : MonoBehaviour, ISupportComp
{
    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    public UnitRefs _ownerUnitRefs;

    public float maxHP = 5f;
    public float curHP = 5f;
    public Slider HPSlider;

    //    public TextMeshPro HPText; // need TmPro installed otherwise use Unity's built in Text
    public TextMeshProUGUI StatHPText;

    //Invulnerable means unable to receive damage
    public bool bIsInvulnerable;


    public UnityEvent OnKOEvent;
    public UnityEvent OnKOEvent_WaitForHPSliderLerp;

    void Awake()
    {
        curHP = maxHP;
        OnKOEvent.AddListener(DisableAllOwnerScripts);
        // Ref: https://stackoverflow.com/questions/45582261/how-to-add-unity-component-functions-in-unityevent-using-script/45583832
    }

 
    //[HideInInspector]
    public float lerpHP;
    public float hpSliderLerpSpeed = 0.95f;



    void OnGUI()
    {
        if (HPSlider != null)
        {
            if (maxHP != 0) // should not divide by zero
            {
                lerpHP = MathCommon.CalculateSliderValuePercentage(HPSlider, curHP, maxHP, hpSliderLerpSpeed * Time.deltaTime) * maxHP;
                // 2decimal place ToString("F2") Reference: https://answers.unity.com/questions/50391/how-to-round-a-float-to-2-dp.html
                if (StatHPText)
                    StatHPText.SetText("HP: " + Mathf.Round(lerpHP) + " / " + (int)maxHP);
                HPSlider.value = lerpHP / maxHP;
            }
            // If HP bar value == 0 i.e by 0 decimal place, then ResetToOriginal Shader Material
            if (Mathf.Round(lerpHP) == 0)
            {
                AudioManager.instance.PlaySFX("KO");
            }
        }
    }
    public void SetCurHP(float newCurrHP)  // Apply Damage method
    {
        curHP = Mathf.Clamp(newCurrHP, 0, maxHP);
        // Modify HP Cat Data

        if (isActiveAndEnabled)
        {
            StartCoroutine(OnDamageBlinking());
        }
        if (curHP <= 0)
        {
            //            anim.SetTrigger("death");
            OnKO();
            curHP = 0;
            _ownerUnitRefs._unitCharacterController._rigidbody2D.velocity = Vector2.zero;
        }
    }


    public void Heal(float HealValue, GameObject instiagor)
    {
        Heal(HealValue);
    }

    public void Heal(float HealValue)
    {
        if (curHP > 0)
        {
            curHP += HealValue;
        }
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }

    }

    public void ApplyDamage(float DamageValue, GameObject instigator)
    {
        // instigator
        ApplyDamage(DamageValue);
    }

    public void ApplyDamage(float DamageValue)
    {
        // instigator
        if (curHP > 0 && isBlinking == false)
        {
            curHP -= DamageValue;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(OnDamageBlinking());
            }
        }
        if (curHP <= 0)
        {
            //            anim.SetTrigger("death");
            OnKO();
            curHP = 0;
            _ownerUnitRefs._unitCharacterController._rigidbody2D.simulated = false;
            _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir = Vector2.zero;
        }
    }

    public void OnKO()
    {
        OnKOEvent.Invoke();
        Destroy(_ownerUnitRefs.gameObject);
    }

    public void DisableAllOwnerScripts()
    {
        MonoBehaviour[] components = _ownerUnitRefs.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour comp in components)
        {
            if (comp)
            {
                comp.enabled = false;
            }
        }
    }

    #region SInt mods

    [Header("Physics Filtering")] public List<string> onHitBoxDmgFilterTags;
    public List<string> onHitBoxDmgFilterIgnoreTags;
    public List<GameObject> onHitBoxDmgFilterIgnoreObj;

    public List<int> onHitBoxDmgValue;

    #endregion

    private void OnCollisionEnter(Collision other)
    {
        //Collision Damage
        OnHitBoxDMG(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHitBoxDMG(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitBoxDMG(other.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHitBoxDMG(collision.gameObject);
    }
    void OnHitBoxDMG(GameObject other)
    {
        //Debug.Log("HitBox Detected: " + other.name);
        if (onHitBoxDmgFilterTags.Contains(other.tag) == true
            && onHitBoxDmgFilterIgnoreTags.Contains(other.tag) == false
            && onHitBoxDmgFilterIgnoreObj.Contains(other) == false)
        {
            SetCurHP(curHP - onHitBoxDmgValue[onHitBoxDmgFilterTags.IndexOf(other.tag)]);
        }
        if (other.tag == "Potion")
        {
            AudioManager.instance.PlaySFX("heal");
            ScoreManager.instance.IncrementScore(200f);
            Destroy(other);
        }
    }

    public bool isBlinking;
    public int DamageBlinkCount = 5;
    public float DamageBlinkDelay = 0.1f;
    // Method Reused from: https://github.com/nwy140/TheWYGameDevelopmentFramework/blob/master/WyFramework/Assets/Scripts/MyLibrary/WyFramework/Char%20Scripts/Mech%20Scripts/MechCharStatHP.cs
    IEnumerator OnDamageBlinking()
    {
        isBlinking = true;
        if (HPSlider != null)
        {
            int count = 0;
            while (Mathf.Round(lerpHP) != curHP && Mathf.Round(lerpHP) > 0)
            {
                yield return new WaitForSecondsRealtime(DamageBlinkDelay);
                //disable
                // only blink if not healing
                if (Mathf.Round(lerpHP) > curHP && count < DamageBlinkCount)
                    _ownerUnitRefs.CosmeticObj.SetActive(false);
                yield return new WaitForSecondsRealtime(DamageBlinkDelay);
                // enable sprite
                if (_ownerUnitRefs.CosmeticObj.activeInHierarchy == false)
                    _ownerUnitRefs.CosmeticObj.SetActive(true);
                count++;
            }
        }
        isBlinking = false;
        if (Mathf.Round(lerpHP) == maxHP || Mathf.Round(lerpHP) == curHP || lerpHP == 0 || isBlinking == false)
        {
            if (curHP <= 0)
            {
                OnKOEvent_WaitForHPSliderLerp.Invoke();
            }
        }
    }
}