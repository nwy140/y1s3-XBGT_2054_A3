using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EAbilityUsageState
{
    ready,
    active,
    cooldown
    //ready, // allow start // frame where ability is ready to be called
    //finish, // the final frame where the ability finishes without interruption
    //started, // the first frame where the ability starts
    //updating, // Ability Update each frame 
    //interrupted, // the frame where the ability gets interrupted
}
// public abstract class AbilityBaseComp : MonoBehaviour

public class AbilityBaseComp : MonoBehaviour
{
    #region Ability Attributes
    [Header("Ability Stats - General")]
    public EAbilityTechniques eAbilityTechniques;
    public float AbilityAtk;
    public float ComboMultiplier;

    //[Header("Ability Stats - Buffs // Temporary Stats Modifiers")]
    //public float AbiltiyDefBuff;
    [Header("Ability State")]
    public EAbilityUsageState usageState = EAbilityUsageState.ready;
    [Tooltip("AI or Player possesing this ability?")]
    public EUnitPossesionType eUnitPossesion = EUnitPossesionType.ai;

    [Header("Ability Usage Attributes - Requirements")]
    public bool isUsageRequirementsMet = true;
    public bool isSumOfAllRequirementsMet; // Checked On Update
    [Header("Ability Usage Attributes - Other Requirements")]
    [Tooltip("Movement Modes where the ability can be started")]
    public List<EUnitMovementMode> movementModesRequirement;
    [Tooltip("Do not activate this ability if any of these AnimBoolParamStates below are true")]
    public List<string> rejectedAnimBoolParamStateNames;
    //public List<bool> acceptedAnimParamStates_Bool;

    [Header("Ability Usage Attributes - Animator Params")]
    [Tooltip("Enable These Anim Bool Param State Names when calling this ability")]
    public List<string> enabledAnimBoolParamStateNames;
    [Tooltip("Disable These Anim Bool Param State Names when calling this ability")]
    public List<string> disabledAnimBoolParamStateNames;
    [Tooltip("Trigger These Anim Trigger Param State Names when calling this ability")]
    public List<string> triggeredAnimTriggerParamStateNames;

    // TODO: EquippedProp Requirement
    [Header("Ability Usage Attributes")]
    [Header("Ability Time")]
    public float cooldownDuration;
    public float curCooldownTimeElapsed;
    public float activeDuration;
    public float curActiveTimeElapsed;

    [Tooltip("Could be MP or AP Cost or SkillPoint Cost")]
    public float Cost;
    public bool hasUseQuantityLimit;
    public int useQuantityLimitCount; // If useQuantityLimit used up, ability canUse = false;

    /*
    //[Header("Unit Attributes")]
    //// Attribute Modifiers are values that will be added to the Unit's RPG Attribute Class
    //public float InstigatorUnitHP_OnAbilityStartOffsetModifier;//
    //public float InstigatorUnitHP_OnAbilityEndOffsetModifier;//
    //public float InstigatorUnitHP_OnHitAbilityOffsetModifier;//
    //public float ReceiverUnitHP_OnAbilityStartOffsetModifier;//
    //public float ReceiverUnitHP_OnAbilityEndOffsetModifier;
    //public float ReceiverUnitHP_OnHitAbilityOffsetModifier;
    */
    #region Getters And Setters
    //public EAbilityTechniques currEAbilityTechniques { get => eAbilityTechniques; set => eAbilityTechniques = value; }
    #endregion Getters And Setters
    [Header("Cosmetics-UI")]
    public Sprite icon;
    public Image cooldownProgressImg;
    public string title;
    public string desc;
    public string devComment;
    public string devNotes;
    public bool isDebugLog;

    /*
    //[Header("Cosmetics-InGame")]
    AnimationParam Name
    AnimationParam Type
    Particle Effect (Can Be handled in Animator or Animation Script Optionally)
    Disability Modifiers 
    (i.e When Receiver Stunned -> Disable Movement & Jumping Ability)
    EnAbility Modifiers (i.e Buff -> Enable Other Abilities, for example, Triple Jump or Fast Movement for a few seconds in a makeshift Super Saiyan Mode)
    Audio (We could use the Master AAA Audio package for this, it has behavior designer integration)
     */

    #endregion Ability Attributes
    #region Monobehavior Methods
    protected virtual void Awake()
    {

        if (title == "")
        {
            // Use Enum Name as title if not set in inspector
            title = eAbilityTechniques.ToString();
        }
    }

    public virtual void OnInit() // Called from StartAbility BD Task Node, initialize values from BD Task Node inspector
    {

    }


    #endregion Monobehavior Methods
    #region Ability Basic Virtual Methods

    [HideInInspector]
    [Header("Unit Root Refs Component")]
    public UnitRefs _ownerUnitRefs;
    // Unit Refs Component
    // Sight Sensor (You may have Multiple Sensors)
    // Sound Sensor
    // Hitbox

    #region Simulate Input For Either AI And Player
    [Header("Simulate Input // Can Also Be Used By AI, in the BD Inspector")]
    public bool buttonDown;
    public bool button;
    public bool buttonUp;
    public float Axis;

    [Header("Simulate Input // Will Call Ability Method")]
    //[Tooltip("Will activate ability on this exact frame")]
    [HideInInspector]
    public bool willActivateAbility_CurFrame;
    public bool willActivateAbility_OnButtonDown = true;
    public bool willActivateAbility_OnButton = true;
    public bool willActivateAbility_OnButtonUp;
    public bool willActivateAbility_OnAxisNotZero = true;
    public bool willActivateAbility_OnUpdate;
    public bool willExitAbility_OnButtonUp;
    public bool willExitAbility_OnAxisZero;

    public virtual void ResetInitialButtonAndAxisValues()
    {
        Axis = 0;
        button = false;
    }
    public virtual void ResetInitialButtonDownUpValues()
    {
        // Redundant If Statements // Remove Later
        buttonDown = false;
        buttonUp = false;
    }
    #endregion Simulate Input For Either AI And Player
    public bool EvaluateOtherRequirements()
    {
        if (hasUseQuantityLimit == true)
        { // i.e Some Abilities can only be used once or twice in a Battle. 
            useQuantityLimitCount -= 1; // Quantity Limit is treated like Ammo Count
            if (useQuantityLimitCount <= 0)
            {
                return false;
            }
        }

        // Check Movement Mode
        if (movementModesRequirement.Count == 0 || movementModesRequirement.Contains(_ownerUnitRefs._unitCharacterController.movementMode)) // Correct Movement Mode
        {
            if (_ownerUnitRefs.hasAnim)
            {
                // Check Anim Param Name
                foreach (string animParamName in rejectedAnimBoolParamStateNames)
                {
                    if (_ownerUnitRefs._anim.GetBool(animParamName) == true)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
    //public virtual void TryActivateAbility()
    //{
    //    if (isUsageRequirementsMet == false || EvaluateOtherRequirements() == false)
    //    {

    //        return;
    //    }
    //    // Evaluate Input
    //}


    public virtual void Update()
    {
        UpdateAbilityUsageState();
    }

    // TODO: Interrupt Ability // Can also be done via Behavior Designer Nodes
    #region CoolDown Timer

    //Ref: https://youtu.be/ry4I6QyPw4E?list=PLuLJclBWmeWXforkt4wMaKvLAkbt8Yrsp&t=202
    public void UpdateAbilityUsageState()
    {
        bool isOtherRequirementsMet = EvaluateOtherRequirements();
        if (isUsageRequirementsMet == true && isOtherRequirementsMet)
        {
            isSumOfAllRequirementsMet = true;
        }
        else
        {
            OnUsageRequirementsNotMet();
        }

        EvaluateSimulatedInput();
        if (willActivateAbility_OnUpdate == true)
        {
            if (willActivateAbility_CurFrame == true)
            {
                if (usageState == EAbilityUsageState.ready && isOtherRequirementsMet == true)
                {
                    usageState = EAbilityUsageState.active;
                    curActiveTimeElapsed = activeDuration;
                    AutoSetAnimatorParams();
                    OnAbilityActiveEnter();
                }
            }
        }
        switch (usageState)
        {
            case EAbilityUsageState.ready:
                if (willActivateAbility_CurFrame == true && willActivateAbility_OnUpdate == false)
                {
                    if (isOtherRequirementsMet == true)
                    {
                        usageState = EAbilityUsageState.active;
                        curActiveTimeElapsed = activeDuration;
                        AutoSetAnimatorParams();
                        OnAbilityActiveEnter();
                    }
                }
                break;
            case EAbilityUsageState.active:
                if (curActiveTimeElapsed > 0)
                {
                    curActiveTimeElapsed -= Time.deltaTime;
                    OnAbilityActiveStay();
                }
                else
                {
                    OnAbilityActiveExit();
                    usageState = EAbilityUsageState.cooldown;
                    curCooldownTimeElapsed = cooldownDuration;
                    if (cooldownDuration != 0)
                    {
                        OnAbilityCooldownEnter();
                    }
                }
                break;
            case EAbilityUsageState.cooldown:
                if (curCooldownTimeElapsed > 0)
                {
                    curCooldownTimeElapsed -= Time.deltaTime;
                    OnAbilityCooldownStay();
                }
                else
                {
                    usageState = EAbilityUsageState.ready;
                    if (cooldownDuration != 0)
                    {
                        OnAbilityCooldownExit();
                    }
                }
                break;
            default:
                break;

        }

        buttonDown = false;
        buttonUp = false;
        willActivateAbility_CurFrame = false;
    }

    #endregion CoolDown Timer
    public virtual void EvaluateSimulatedInput()
    {
        // Evaluate Input Then Call StartAbility
        if (buttonDown == true && willActivateAbility_OnButtonDown == true)
        {
            willActivateAbility_CurFrame = true;
        }
        if (button == true && willActivateAbility_OnButton == true)
        {
            willActivateAbility_CurFrame = true;
        }
        if (buttonUp == true && willActivateAbility_OnButtonUp == true)
        {
            willActivateAbility_CurFrame = true;
        }
        if (willActivateAbility_OnUpdate)
        {
            willActivateAbility_CurFrame = true;
        }
        else if (Axis != 0 && willActivateAbility_OnAxisNotZero == true)
        {
            willActivateAbility_CurFrame = true;
        }
        if (buttonUp && willExitAbility_OnButtonUp == true)
        {
            if (willActivateAbility_CurFrame == true)
            {
                OnAbilityActiveExit();
                willActivateAbility_CurFrame = false;
            }
        }
        if (Axis == 0 && willExitAbility_OnAxisZero == true)
        {
            if (willActivateAbility_CurFrame == true)
            {
                OnAbilityActiveExit();
                willActivateAbility_CurFrame = false;
            }
        }
    }

    public virtual void AutoSetAnimatorParams()
    {

        if (_ownerUnitRefs.hasAnim)
        {
            foreach (string animParamName in enabledAnimBoolParamStateNames)
            {
                _ownerUnitRefs._anim.SetBool(animParamName, true);
            }
            foreach (string animParamName in disabledAnimBoolParamStateNames)
            {
                _ownerUnitRefs._anim.SetBool(animParamName, false);
            }
            foreach (string animParamName in triggeredAnimTriggerParamStateNames)
            {
                _ownerUnitRefs._anim.SetTrigger(animParamName);
                Debug.Log(animParamName);
            }
        }
    }
    #endregion Ability Basic Virtual Methods

    #region Ability Inspector Events (For Overriding in child classes)
    [Header("Ability Inspector Events")]
    public UnityEvent OnAbilityActiveEnterEvent;
    public UnityEvent OnAbilityActiveStayEvent;
    public UnityEvent OnAbilityActiveExitEvent;
    public UnityEvent OnAbilityCooldownEnterEvent;
    public UnityEvent OnAbilityCooldownStayEvent;
    public UnityEvent OnAbilityCooldownExitEvent;
    public UnityEvent OnUsageRequirementsNotMetEvent;


    #endregion Ability Inpsector Events (For Overriding in child classes)
    // Override your ability functionality here, then override the other classes to call this method to perform your ability's functionalities, or bind it to one of the UnityEvents
    public virtual void AbilityFunctionality()
    {
        // You may override this method and calls this method in OnAbilityActive Enter, Stay or Exit
        // Place the Logic Of How Your Ability Works here
        if (isDebugLog)
        {
            Debug.Log("AbilityFunctionality : " + title);
        }
        if (eUnitPossesion == EUnitPossesionType.player)
        {
            AbilityFunctionalityPlayer();
        }
        else if (eUnitPossesion == EUnitPossesionType.ai)
        {
            AbilityFunctionalityAI();
        }
    }
    public virtual void AbilityFunctionalityPlayer()
    {
        if (isDebugLog)
        {
            Debug.Log("AbilityFunctionalityPlayer : " + title);
        }
    }
    public virtual void AbilityFunctionalityAI()
    {
        if (isDebugLog)
        {
            Debug.Log("AbilityFunctionalityAI : " + title);
        }
    }
    public virtual void OnAbilityActiveEnter()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityActiveEnter() : " + title);
        }
        OnAbilityActiveEnterEvent.Invoke();

        // Call OnAbiltiyStay at least once
        OnAbilityActiveStay();
        // if active is 0, OnAbilityStay will only be called once
        // OnAbilityStay will keep calling until activeTime is 0
    }
    public virtual void OnAbilityActiveStay()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityActiveStay() : " + title);
        }
        OnAbilityActiveStayEvent.Invoke();
    }
    public virtual void OnAbilityActiveExit()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityActiveExit() : " + title);
        }
        OnAbilityActiveExitEvent.Invoke();
    }
    public virtual void OnAbilityCooldownEnter()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityCooldownEnter() : " + title);
        }
        OnAbilityCooldownEnterEvent.Invoke();
    }
    public virtual void OnAbilityCooldownStay()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityCooldownStay() : " + title);
        }
        OnAbilityCooldownStayEvent.Invoke();
    }
    public virtual void OnAbilityCooldownExit()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityCooldownExit() : " + title);
        }
        OnAbilityCooldownExitEvent.Invoke();
    }
    public virtual void OnUsageRequirementsNotMet()
    {
        if (isDebugLog)
        {
            Debug.Log("OnUsageRequirementsNotMet() : " + title);
        }
        OnUsageRequirementsNotMetEvent.Invoke();
    }
}


#region Redundant
/*
    public virtual IEnumerator OnAbilityReady()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityReady() : " + title);
        }
        OnAbilityReadyEvent.Invoke();
        yield return null;
    }
    public virtual IEnumerator OnAbilityStarted()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityStarted() : " + title);
        }
        OnAbilityStartedEvent.Invoke();
        yield return null;
            //ResetInitialButtonDownUpValues();
        }
    public virtual IEnumerator OnAbilityUpdating()
    {
        if (isDebugLog)
        {
            Debug.Log("OnAbilityUpdating() : " + title);
        }
        OnAbilityUpdatingEvent.Invoke();
        yield return null;
    }
    //public virtual IEnumerator OnAbilityInterrupted()
    //{
    //    yield return null;
    //}
    public virtual IEnumerator OnAbilityFinish()
    {
        OnAbilityFinishEvent.Invoke();
        yield return null;
    }


    #region Monobehaviour Inspector Events (For Overriding in child classes)
    [Header("Monobehavior Inspector Events")]
    public bool isAllowAwakeInspectorEvent;
    public UnityEvent AwakeEvent;
    public bool isAllowOnEnableInspectorEvent;
    public UnityEvent OnEnableEvent;
    public bool isAllowStartInspectorEvent;
    public UnityEvent StartEvent;
    public bool isAllowUpdateInspectorEvent;
    public UnityEvent UpdateEvent;
    #endregion Monobehaviour Inpsector Events (For Overriding in child classes)
 */
#endregion Redundant