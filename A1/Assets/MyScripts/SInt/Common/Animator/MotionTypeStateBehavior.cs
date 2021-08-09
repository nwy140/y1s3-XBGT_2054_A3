using UnityEngine;

// Ref: https://youtu.be/BHs7IqQSuSc?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU
public class MotionTypeStateBehavior : StateMachineBehaviour, IStateBehavior
{
    [Header("Root Motion Type Settings")]
    public bool isAllowOverwriteRootMotion; // allow modify animator to apply RootMotion
    public bool isRootMotion; // OnStateEnter
    public bool isUseTempMovementMode; // Temporary Movement Mode
    public EUnitMovementMode tempMovementMode;

    [Header("X : Right Vector, Y : Forward Vector")]
    public Vector2 scriptMotionVelocity2D; // Relative to forward and right vector
    //public bool isFreezeScriptMotionXAxis;
    //public bool isUseTempGravityOverride;
    //public float tempGravityOverride;
    //private float originalGravity;
    public bool isUseCharControlTempVerticalVelocityOverride;
    public float tempCharControlVerticalVelocity;
    private float originalCharControlVerticalVelocity;

    [Header("If Is Timed Settings")]
    public bool isIsTimed;
    public float Start, End; // Animation percentage

    private bool _waitForExit;
    private bool _onTransitionExitTriggered;

    #region Interface Related
    private UnitRefs _ownerUnitRefs;
    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    private bool hasOwnerUnitRefs;
    #endregion Interface Related 
    //TODO: Optional, Timed Motion Curve for Script Motion

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasOwnerUnitRefs == false)
        {
            hasOwnerUnitRefs = animator.TryGetComponent(out _ownerUnitRefs);
            //originalGravity = _ownerUnitRefs._unitCharacterController.Gravity;
            //originalCharControlVerticalVelocity = _ownerUnitRefs._unitCharacterController._verticalVelocity;
        }
        if (isIsTimed == true)
        {
            _waitForExit = false;
            _onTransitionExitTriggered = false;
            //animator.SetBool(data.paramName, !data.setDefaultState);

            //// Set all false or invert first
            //if (isAllowOverwriteRootMotion)
            //{
            //    animator.applyRootMotion = !isRootMotion;
            //}
            RevertToOriginalValuesTimed(animator);
        }
        else
        {
            if (isAllowOverwriteRootMotion)
            {
                animator.applyRootMotion = isRootMotion;
            }
            ApplyMotionTypeSettings(animator);

        }
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isIsTimed == true)
        {
            if (CheckOnTransitionExit(animator, layerIndex))
            {
                OnStateTransitionExit(animator);
            }
            if (_onTransitionExitTriggered == false && stateInfo.normalizedTime >= Start && stateInfo.normalizedTime <= End)
            {
                //animator.SetBool(data.paramName, data.setDefaultState);
                if (isAllowOverwriteRootMotion)
                {
                    animator.applyRootMotion = isRootMotion;
                }
                ApplyMotionTypeSettings(animator);
            }
        }
        else
        {
            ApplyMotionTypeSettings(animator);
        }
    }

    private void OnStateTransitionExit(Animator animator)
    {
        if (isIsTimed == true)
        {
            // revert to original value  
            RevertToOriginalValuesTimed(animator);
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isIsTimed == true)
        {
            if (_onTransitionExitTriggered == false)
            {
                //animator.SetBool(paramName, !data.setDefaultState);
                RevertToOriginalValuesTimed(animator);
            }
        }
        else
        {
            RevertToOriginalValues();
        }
    }

    private bool CheckOnTransitionExit(Animator animator, int layerIndex)
    {
        if (_waitForExit == false && animator.GetAnimatorTransitionInfo(layerIndex).fullPathHash == 0)
        {
            _waitForExit = true;
        }
        if (_onTransitionExitTriggered == false && _waitForExit == true && animator.IsInTransition(layerIndex))
        {
            _onTransitionExitTriggered = true;
            return true;
        }
        return false;
    }

    public void RevertToOriginalValuesTimed(Animator animator)
    {
        //animator.SetBool(paramName, !data.setDefaultState);
        if (isAllowOverwriteRootMotion)
        {
            animator.applyRootMotion = !isRootMotion;
        }
        RevertToOriginalValues();
    }
    public void RevertToOriginalValues()
    {
        if (hasOwnerUnitRefs)
        {
            //if (isUseTempGravityOverride)
            //{
            //    _ownerUnitRefs._unitCharacterController.Gravity = originalGravity;
            //}
            if (isUseCharControlTempVerticalVelocityOverride)
            {
                //_ownerUnitRefs._unitCharacterController._verticalVelocity = originalCharControlVerticalVelocity;
            }
        }
    }
    void ApplyMotionTypeSettings(Animator animator)
    {
        if (hasOwnerUnitRefs)
        {
            if (isUseTempMovementMode)
            {
                _ownerUnitRefs._unitCharacterController.movementMode = tempMovementMode;
            }
            //if (isUseTempGravityOverride)
            //{
            //    _ownerUnitRefs._unitCharacterController.Gravity = tempGravityOverride;
            //}
            if (isUseCharControlTempVerticalVelocityOverride)
            {
                //_ownerUnitRefs._unitCharacterController._verticalVelocity = tempCharControlVerticalVelocity;
            }
        }
        if (scriptMotionVelocity2D != Vector2.zero)
        {
            if (animator.GetComponent<CharacterController>() != null)
            {
                CharacterController cc = animator.GetComponent<CharacterController>();
                var forwardTransform = animator.transform.forward;

                //if (isFreezeScriptMotionXAxis)
                //{
                //    forwardTransform.x = 0; 
                //}
                if (scriptMotionVelocity2D.y != 0)
                {
                    cc.Move(forwardTransform * scriptMotionVelocity2D.y
                        * Time.deltaTime);
                }
                if (scriptMotionVelocity2D.x != 0)
                {
                    cc.Move(forwardTransform * scriptMotionVelocity2D.x
                        * Time.deltaTime);
                }
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}


}
