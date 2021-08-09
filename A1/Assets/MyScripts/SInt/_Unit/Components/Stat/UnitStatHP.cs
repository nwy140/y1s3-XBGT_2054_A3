using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

//using TmPro;

//Class
///<summary> 
///     This class controls the HP Stat of the Character
///         
///     Explanation:
/// 		
///     Usage:
/// 		
///     Integration:
/// 
/// </summary>
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
    private bool bIsInvulnerable;
    private Animator animator;

    public List<GameObject> objsToDisableOnDeath;
    public List<GameObject> objsToEnableOnDeath;


    void Start()
    {
        animator = GetComponent<Animator>();
        // if anim still null, Get anim in children
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        curHP = maxHP;

    }
    void OnGUI()
    {
        if (HPSlider)
            HPSlider.value = curHP / maxHP;
        if (StatHPText)
            StatHPText.text = "HP: " + curHP + "/" + maxHP;
    }

    public bool Invulnerable
    {
        get { return bIsInvulnerable; }
        set { bIsInvulnerable = value; }
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
        if (curHP > 0 && Invulnerable == false)
        {
            curHP -= DamageValue;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(OnBlinkingsprite());
            }
        }

        if (curHP <= 0)
        {
            //            anim.SetTrigger("death");

            OnDeath();

            curHP = 0;
        }


    }

    public void OnDeath()
    {
        if (animator)
        {
            animator.SetBool("IsDead", true);

        }
        // default tag objects will not be shot
        //// death code for char
        // disable all components in char


        //for char using NavAgent only
        if (GetComponent<NavMeshAgent>())
        {
            GetComponent<NavMeshAgent>().isStopped = true;
        }

        // disable all components to prevent input or physics from occuring
        MonoBehaviour[] components = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour comp in components)
        {
            if (comp)
            {
                comp.enabled = false;
            }
        }

        foreach (GameObject obj in objsToDisableOnDeath)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objsToEnableOnDeath)
        {
            obj.SetActive(true);
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
        Debug.Log("HitBox Detected: " + other.name);
        if (onHitBoxDmgFilterTags.Contains(other.tag) == true
            && onHitBoxDmgFilterIgnoreTags.Contains(other.tag) == false
            && onHitBoxDmgFilterIgnoreObj.Contains(other) == false)
        {
            ApplyDamage(onHitBoxDmgValue[onHitBoxDmgFilterTags.IndexOf(other.tag)]);
            animator.SetTrigger("Trigger_Stun");
        }
    }
    public bool bisinvulnerable;
    IEnumerator OnBlinkingsprite()
    {
        bisinvulnerable = true;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            _ownerUnitRefs.CosmeticObj.SetActive(false);
            //disable sprite
            yield return new WaitForSeconds(0.1f);
            // enable sprite
            _ownerUnitRefs.CosmeticObj.SetActive(true);
        }
        bisinvulnerable = false;
    }
}