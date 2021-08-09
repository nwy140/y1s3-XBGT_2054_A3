using System;
using Cinemachine;
using Rewired;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Animations;

namespace MyScripts.SInt.Character
{
    public class CharMovesets : MonoBehaviour, ICharMovesets
    {
        // Please see link for the list of moves
        // https://docs.google.com/document/d/1HGl2VWe2WO3Wv1X3tET8ymVM_4GU11_Q-2J0vo-QfyY/edit#heading=h.1i8r60md8wmy
        private CharRefs _cr;

        private void Awake()
        {
            _cr = GetComponent<CharRefs>();
        }

        private void Update()
        {
            #region Regular use

            // Place all handling update methods here
            OnGeneralLockOn_SecondKeyDown_Handling();

            #endregion

            #region Automated Features

            AutomatedOnPossiblePriorityTargetFound_Handling();
            AutomatedLookAtIKLerpWeight_Handling();

            #endregion Automated Features
        }

        #region Rewired Input System Use

        public void OnRegularAttack(InputActionEventData data)
        {
            // IDEA TODO: Use Input Queque for button combinations 
            _cr.meleeHandler.OnAttack(1);
        }
        public void OnChargedAttack(InputActionEventData data)
        {
            // IDEA TODO: Use Input Queque for button combinations 
            _cr.meleeHandler.OnAttack(2);
        }

        public void OnGeneralLockOn(InputActionEventData data)
        {
            if (data.GetButtonDown())
            {
                OnGeneralLockOn();
            }
        }


        #endregion Rewired Input System Use

        #region Regular Use
 
        public void OnGeneralLockOn()
        {
            if (GeneralLockOn_GetIsLockedOn() == false)
            {
                OnGeneralLockOn_KeyDown();
            }
            else
            {
                OnGeneralLockOn_SecondKeyDown();
            }
        }

        #endregion Regular Use

        #region Key Helper methods

        #region OnGeneralLockOn

        [Header("General Locked ON")]
        public bool isGeneralLockedOn = false;

        public void OnGeneralLockOn_KeyDown()
        {
            if (isGeneralLockedOn == false && _cr._charCompAITargetingSystem_LineOfSightVision.HasTarget)
            {
                var cineLookAtTargetObj = _cr._charCompAITargetingSystem_LineOfSightVision.Target;

                if (cineLookAtTargetObj != null)
                {
                    if (cineLookAtTargetObj.activeInHierarchy &&
                        cineLookAtTargetObj.gameObject != _cr.gameObject)
                    {
                        cineLookAtTargetObj = OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(cineLookAtTargetObj);
                        // Add a source target to LockedOnIndicator's Parent Constraint to enable LockOn for that target Object
                        ConstraintCommon.SetFirstCSourceSnapTargetParent(_cr.HUD_LockOnChosenTargetPrnCstrain,
                            cineLookAtTargetObj.transform);

                        //? is a null checker shortcut // A widely unknown C# feature I think
                        GeneralLockOn_SetIsLockedOn(true);
                    }
                }
            }
        }
        public GameObject OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(GameObject cineLookAtTargetObj)
        {
            GameObject result = cineLookAtTargetObj;
            if (cineLookAtTargetObj.transform.childCount > 0)
            {
                //if first child of target has offset point obj, use the transform of that offset point obj
                if (cineLookAtTargetObj.transform.GetChild(0).CompareTag("TargetOffsetPoint"))
                {
                    result = cineLookAtTargetObj.transform.GetChild(0).gameObject;
                }
                // else reuse original AISensor detected target as cineLookAtTarget rather than its offsetpoint
            }
            return result;
        }
        // Check for the second time you press the same button
        public void OnGeneralLockOn_SecondKeyDown()
        {
            GeneralLockOn_SetIsLockedOn(false);
        }

        public void OnGeneralLockOn_SecondKeyDown_Handling()
        {
            if (_cr._charCompAITargetingSystem_360Vision_Far.HasTarget == false)
            {
                GeneralLockOn_SetIsLockedOn(false);
            }
            if (_cr.HUD_LockOnChosenTargetPrnCstrain.sourceCount > 0)
            {
                if (_cr.HUD_LockOnChosenTargetPrnCstrain.GetSource(0).sourceTransform == null)
                {
                    GeneralLockOn_SetIsLockedOn(false);
                }
                if (_cr.HUD_LockOnChosenTargetPrnCstrain.GetSource(0).sourceTransform.gameObject.activeInHierarchy == false)
                {
                    GeneralLockOn_SetIsLockedOn(false);
                }
            }
        }

        public void GeneralLockOn_SetIsLockedOn(bool newIsLockedOn)
        {
            isGeneralLockedOn = newIsLockedOn;
            //_cr.HUD_LockOnChosenTargetPrnCstrain.enabled = newIsLockedOn;
            GameObject cineLookAtTargetObj = _cr.HUD_LockOnChosenTargetIndicator;
            if (newIsLockedOn == true)
            {
                _cr.CM_LockOn.Priority = 12;
            }
            else
            {
                _cr.CM_LockOn.Priority = 0;
            }
            _cr.HUD_LockOnChosenTargetIndicator.gameObject.SetActive(newIsLockedOn);
            if (_cr._lookAtIK != null)
            {
                if (newIsLockedOn == true)
                {
                    cineLookAtTargetObj = OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(cineLookAtTargetObj);
                    nextLookAtIK_Target = cineLookAtTargetObj.transform;
                }
            }

            if (newIsLockedOn == false)
            {
                if (_cr.HUD_LockOnChosenTargetPrnCstrain.sourceCount > 0)
                {
                    _cr.HUD_LockOnChosenTargetPrnCstrain.RemoveSource(0);
                }
            }
        }

        public bool GeneralLockOn_GetIsLockedOn()
        {
            return isGeneralLockedOn;
        }

        #endregion

        #endregion Key Helper methods

        #region Automated

        public bool isPossibleTargetFound;


        #region OnPossiblePriorityTargetFound

        public bool isAllow_LookAtIK_OnPossibleTargetFound = true;

        public void AutomatedOnPossiblePriorityTargetFound_Handling()
        {
            if (GeneralLockOn_GetIsLockedOn() == true)
            {
                _cr.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
                isPossibleTargetFound = false;
                return;
            }

            if (_cr.HUD_LockOnPossibleTargetPrnCstrain != null
                && _cr._charCompAITargetingSystem_LineOfSightVision.HasTarget)
            {
                var cineLookAtTargetObj = _cr._charCompAITargetingSystem_LineOfSightVision.Target;
                if (cineLookAtTargetObj != null)
                {
                    if (cineLookAtTargetObj.activeInHierarchy &&
                        cineLookAtTargetObj.gameObject != _cr.gameObject)
                    {

                        cineLookAtTargetObj = OnGeneralLockOn_GetPossibleTargetChildOffsetPoint(cineLookAtTargetObj);
                        ConstraintCommon.SetFirstCSourceSnapTargetParent(_cr.HUD_LockOnPossibleTargetPrnCstrain,
                            cineLookAtTargetObj.transform);

                        _cr.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(true);
                        isPossibleTargetFound = true;
                        if (_cr._lookAtIK != null)
                        {
                            if (nextLookAtIK_Target == null)
                            {
                                nextLookAtIK_Target = cineLookAtTargetObj.transform;
                            }
                            if (Vector3.Distance(cineLookAtTargetObj.transform.position, nextLookAtIK_Target.position) > 1.08) // targets not too close to each other
                            {
                                nextLookAtIK_Target = cineLookAtTargetObj.transform;
                            }
                        }
                    }
                    else
                    {
                        isPossibleTargetFound = false;
                        _cr.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
                    }
                }
                else
                {
                    isPossibleTargetFound = false;
                    _cr.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
                }
            }
            else
            {
                isPossibleTargetFound = false;
                _cr.HUD_LockOnPossibleTargetPrnCstrain.gameObject.SetActive(false);
            }
        }

        public bool Automated_GetIsPossibleTargetFound()
        {
            return isPossibleTargetFound;
        }

        #region LookAtIKLerpWeight

        public float LookAtIk_WeightLerpSpeed = 0.5f;
        public float LookAtIK_isLockedWeightLerpMultiplier = 1.5f;
        public Transform nextLookAtIK_Target;
        public bool isLookAtIKCurrentlySwitchingTargets;

        void AutomatedLookAtIKLerpWeight_Handling()
        {
            // lerp lookatik weight
            if (Automated_GetIsPossibleTargetFound() || GeneralLockOn_GetIsLockedOn())
            {
                if (((isAllow_LookAtIK_OnPossibleTargetFound && Automated_GetIsPossibleTargetFound() &&
                      !GeneralLockOn_GetIsLockedOn())
                     || GeneralLockOn_GetIsLockedOn()) &&
                     (_cr._lookAtIK.solver.target != null &&
                     _cr._lookAtIK.solver.target == nextLookAtIK_Target))
                {
                    if (_cr._lookAtIK != null)
                    {
                        AutomatedLookAtIKLerpWeight_IncrementWeight();
                    }
                }
                else
                {
                    AutomatedLookAtIKLerpWeight_DecrementWeight();
                }
            }
            else
            {
                AutomatedLookAtIKLerpWeight_DecrementWeight();
            }

            if (nextLookAtIK_Target != null)
            {
                if (_cr._lookAtIK.solver.target != nextLookAtIK_Target)
                {
                    isLookAtIKCurrentlySwitchingTargets = true;
                }
                else
                {
                    isLookAtIKCurrentlySwitchingTargets = false;
                }
            }

            if (_cr._lookAtIK.solver.IKPositionWeight == 0)
            {
                _cr._lookAtIK.solver.target = nextLookAtIK_Target;
            }
        }


        void AutomatedLookAtIKLerpWeight_IncrementWeight()
        {
            if (_cr._lookAtIK != null)
            {
                float multiplier = 1f;
                if (GeneralLockOn_GetIsLockedOn() == true)
                {
                    multiplier = LookAtIK_isLockedWeightLerpMultiplier;
                }
                _cr._lookAtIK.solver.SetLookAtWeight(_cr._lookAtIK.solver.IKPositionWeight +=
                    Time.deltaTime * LookAtIk_WeightLerpSpeed * multiplier);
            }
        }

        void AutomatedLookAtIKLerpWeight_DecrementWeight()
        {
            if (_cr._lookAtIK != null)
            {
                float multiplier = 1f;
                if(GeneralLockOn_GetIsLockedOn() == true)
                {
                    multiplier = LookAtIK_isLockedWeightLerpMultiplier;
                }
                _cr._lookAtIK.solver.SetLookAtWeight(_cr._lookAtIK.solver.IKPositionWeight -=
                    Time.deltaTime * LookAtIk_WeightLerpSpeed * multiplier);
            }
        }

        #endregion LookAtIKLerpWeight

        #endregion OnPossiblePriorityTargetFound

        #endregion Automated
    }
}