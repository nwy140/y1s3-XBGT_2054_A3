using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    public abstract class StateData : ScriptableObject
    {
        public virtual void UpdateAbility(CharState charState, Animator animator){
            EvaluateAbilityUsageState();
            if (willActivateAbility_CurFrame == false)
            {
                return;
            }

        }

        #region Adapted from Old AbilityBaseComp
        [Header("Ability State")]
        protected EAbilityUsageState usageState = EAbilityUsageState.ready;
        // SInt
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
        #endregion Simulate Input For Either AI And Player

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
            return true;
        }
        //public virtual void TryActivateAbility()
        //{
        //    if (isUsageRequirementsMet == false || EvaluateOtherRequirements() == false)
        //    {

        //        return;
        //    }
        //    // Evaluate Input
        //}


        // TODO: Interrupt Ability // Can also be done via Behavior Designer Nodes
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
                    //OnAbilityActiveExit();
                    willActivateAbility_CurFrame = false;
                }
            }
            if (Axis == 0 && willExitAbility_OnAxisZero == true)
            {
                if (willActivateAbility_CurFrame == true)
                {
                    //OnAbilityActiveExit();
                    willActivateAbility_CurFrame = false;
                }
            }
        }
        //Ref: https://youtu.be/ry4I6QyPw4E?list=PLuLJclBWmeWXforkt4wMaKvLAkbt8Yrsp&t=202
        public void EvaluateAbilityUsageState()
        {
            bool isOtherRequirementsMet = EvaluateOtherRequirements();

            EvaluateSimulatedInput();
            if (willActivateAbility_OnUpdate == true)
            {
                if (willActivateAbility_CurFrame == true)
                {
                    if (usageState == EAbilityUsageState.ready && isOtherRequirementsMet == true)
                    {
                        usageState = EAbilityUsageState.active;
                        curActiveTimeElapsed = activeDuration;
                        //AutoSetAnimatorParams();
                        //OnAbilityActiveEnter();
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
                            //AutoSetAnimatorParams();
                            //OnAbilityActiveEnter();
                        }
                    }
                    break;
                case EAbilityUsageState.active:
                    if (curActiveTimeElapsed > 0)
                    {
                        curActiveTimeElapsed -= Time.deltaTime;
                        //OnAbilityActiveStay();
                    }
                    else
                    {
                        //OnAbilityActiveExit();
                        usageState = EAbilityUsageState.cooldown;
                        curCooldownTimeElapsed = cooldownDuration;
                        if (cooldownDuration != 0)
                        {
                            //OnAbilityCooldownEnter();
                        }
                    }
                    break;
                case EAbilityUsageState.cooldown:
                    if (curCooldownTimeElapsed > 0)
                    {
                        curCooldownTimeElapsed -= Time.deltaTime;
                        //OnAbilityCooldownStay();
                    }
                    else
                    {
                        usageState = EAbilityUsageState.ready;
                        if (cooldownDuration != 0)
                        {
                            //OnAbilityCooldownExit();
                        }
                    }
                    break;
                default:
                    break;

            }

            //buttonDown = false;
            //buttonUp = false;
            //willActivateAbility_CurFrame = false;
        }
        #endregion Adapted from Old AbilityBaseComp
    }
}