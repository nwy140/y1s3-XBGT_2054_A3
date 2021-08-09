using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRefs : MonoBehaviour
{

    #region Comp Refs
    [Header("Cosmetics")]
    // public ICharMovementWrapper _charMovement;
    //public ICharMovesets _charMovesets;
    public Animator _anim;
    public bool hasAnim;

    [Header("Ability Related")]
    //public SupportCompCombatMeleeAtkHandler meleeHandler;
    public UnitCompAbilityManager unitCompAbilityManager;

    //[Header("Character Components AI Sense")]
    //// Only LineOfSight should be blocked by walls
    //// 360Vision Far should not be blocked by walls, for Lock On System
    ////public CharCompAISensor _charCompAISensor_LineOfSightVision;

    ////public CharCompAISensor _charCompAISensor_360Vision_Far;
    ////public CharCompAISensor _charCompAISensor_360Vision_Close;

    ////public CharCompAITargetingSystem _charCompAITargetingSystem_LineOfSightVision;
    ////public CharCompAITargetingSystem _charCompAITargetingSystem_360Vision_Far;
    ////public CharCompAITargetingSystem _charCompAITargetingSystem_360Vision_Close;


    [Header("Ability Motion")]
    //public CharacterController _characterController; // built-in character controller
    public UnitCompMotionCharacterController2D _unitCharacterController;
    //public UnitCompMotionCharacterController _unitCharacterController;
    //public UnitCompMotionRigidBodyPush _basicRigidBodyPush;
    //public UnitCompMotionMovingPlatform _motionMovingPlatform;

    //[Header("Integration: Humanoid-Final IK")]
    //public FullBodyBipedIK _fullBodyBipedIK;
    //public GrounderFBBIK _grounderFbbik;
    //public LookAtIK _lookAtIK;

    //[Header("Integration: SAC Movement Effects (Standard Character Assets)")]
    //public CharCompMovementEffects _charCompMovementEffects;

    //[Header("Rig Transforms")]
    //public Transform rigSkeleton;

    //[Header("Camera Cinemachines")]
    //public Transform cameraTarget;
    //public CinemachineVirtualCameraBase CM_StateDriveFreeLook;
    //public CinemachineVirtualCameraBase CM_LockOn;

    //[Header("HUD Lock On")]
    //public ParentConstraint HUD_LockOnChosenTargetPrnCstrain;
    //public GameObject HUD_LockOnChosenTargetIndicator;
    //public ParentConstraint HUD_LockOnPossibleTargetPrnCstrain;
    //public GameObject HUD_LockOnPossibleTargetIndicator;

    //// [Header("Parent Constraint - Snap To rigHead")]
    //public ParentConstraint aiSensorParentConstraint;

    // [Header("Movesets")]

    private void Awake()
    {
        // Ref: TryGetComponent https://medium.com/chenjd-xyz/unity-tip-use-trygetcomponent-instead-of-getcomponent-to-avoid-memory-allocation-in-the-editor-fe0c3121daf6
        hasAnim = TryGetComponent(out _anim);
        //TryGetComponent(out _characterController);
        TryGetComponent(out _unitCharacterController);
        //TryGetComponent(out _basicRigidBodyPush);
        //TryGetComponent(out _motionMovingPlatform);

    }
    private void OnEnable()
    {
        //ResetRigRelatedRefs();
        // Setup Rig And IK
    }

    public void ResetRigRelatedRefs()
    {
        //if (cameraTarget)
        //{
        //    CM_StateDriveFreeLook.LookAt = cameraTarget;
        //    CM_LockOn.Follow = cameraTarget;
        //}
        //// Parent Constraints
        ///// Disabled///Snap AISensor to rigHead for more realistic sight sensing, but could break sight while performing evade/dive roll. Disabled
        //// ConstraintCommon.ResetCSrcToSingleTargetParent(aiSensorParentConstraint, _fullBodyBipedIK.references.head);
        //if (_fullBodyBipedIK.references.leftFoot.GetChild(0) != null)
        //{
        //    var footLTriggerPrCnstr = _charCompMovementEffects.m_MovementEffects.m_LeftFootDetection
        //        .GetComponent<ParentConstraint>();
        //    ConstraintCommon.ResetCSrcToSingleTargetParent(footLTriggerPrCnstr,
        //        _fullBodyBipedIK.references.leftFoot.GetChild(0));
        //}

        //if (_fullBodyBipedIK.references.rightFoot.GetChild(0) != null)
        //{
        //    var footRTriggerPrCnstr = _charCompMovementEffects.m_MovementEffects.m_RightFootDetection
        //        .GetComponent<ParentConstraint>();
        //    ConstraintCommon.ResetCSrcToSingleTargetParent(footRTriggerPrCnstr,
        //        _fullBodyBipedIK.references.rightFoot);
        //}

        //// _charCompMovementEffects.m_MovementEffects.m_LeftFootDetection.transform.position = cSource.sourceTransform.position;
    }


    #endregion Comp Refs

}

