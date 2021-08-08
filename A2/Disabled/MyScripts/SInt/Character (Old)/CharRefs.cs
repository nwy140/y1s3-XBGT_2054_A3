using System;
using System.Collections.Generic;
using Cinemachine;
using MyScripts.SInt.Character.CharComp;
using MyScripts.SInt.Character.CharComp.AI;
using RootMotion.FinalIK;
using StandardAssets.Characters.Common;
using StandardAssets.Characters.Physics;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations;
using MyScripts.SInt.Character;


public class CharRefs : MonoBehaviour
{
    // Get the Comp Refs that are already in the root obj
    private void Awake()
    {
        _charInputRewiredWrapper = GetComponent<CharInputRewiredWrapper>();
        _charMovement = GetComponent<ICharMovementWrapper>();
        _charMovesets = GetComponent<ICharMovesets>();

        if (GetComponent<Animator>() != null)
            _anim = GetComponent<Animator>();
        if (GetComponent<CharacterController>() != null)
            _characterController = GetComponent<CharacterController>();
        if (GetComponent<StarterAssetsThirdPersonController>() != null)
            starterAssetsThirdPersonController = GetComponent<StarterAssetsThirdPersonController>();
        if (GetComponent<BasicRigidBodyPush>() != null)
            _basicRigidBodyPush = GetComponent<UnitCompMotionRigidBodyPush>();
    }
    #region Exposed Variables
    [Header("Moveset")]
    public EUnitMovementMode currMovementMode;
    public ECharMovesetState currMovesetPerformingStateType;
    public MoveSetData currMovesetData; // Test Evade Move
    public bool isMovesetOnGoing; // for isolated motion or isolated attackso only
    #endregion

    #region Comp Refs
    [Header("Character Setup")]
    public CharInputRewiredWrapper _charInputRewiredWrapper;
    public ICharMovementWrapper _charMovement;
    public ICharMovesets _charMovesets;
    public Animator _anim;

    [Header("Character Components Ability Actions")]
    public OldMeleeHandler meleeHandler;

    [Header("Character Components AI Sense")]
    // Only LineOfSight should be blocked by walls
    // 360Vision Far should not be blocked by walls, for Lock On System
    public CharCompAISensor _charCompAISensor_LineOfSightVision;

    public CharCompAISensor _charCompAISensor_360Vision_Far;
    public CharCompAISensor _charCompAISensor_360Vision_Close;

    public CharCompAITargetingSystem _charCompAITargetingSystem_LineOfSightVision;
    public CharCompAITargetingSystem _charCompAITargetingSystem_360Vision_Far;
    public CharCompAITargetingSystem _charCompAITargetingSystem_360Vision_Close;


    [Header("Integration: Starter Assets Controller")]
    public CharacterController _characterController; // built-in character controller
    public StarterAssetsThirdPersonController starterAssetsThirdPersonController;

    public UnitCompMotionRigidBodyPush _basicRigidBodyPush;

    [Header("Integration: Final IK")]
    public FullBodyBipedIK _fullBodyBipedIK;
    public GrounderFBBIK _grounderFbbik;
    public LookAtIK _lookAtIK;

    [Header("Integration: SAC Movement Effects (Standard Character Assets)")]
    public CharCompMovementEffects _charCompMovementEffects;

    [Header("Rig Transforms")]
    public Transform rigSkeleton;

    [Header("Camera Cinemachines")]
    public Transform cameraTarget;
    public CinemachineVirtualCameraBase CM_StateDriveFreeLook;
    public CinemachineVirtualCameraBase CM_LockOn;

    [Header("HUD Lock On")]
    public ParentConstraint HUD_LockOnChosenTargetPrnCstrain;
    public GameObject HUD_LockOnChosenTargetIndicator;
    public ParentConstraint HUD_LockOnPossibleTargetPrnCstrain;
    public GameObject HUD_LockOnPossibleTargetIndicator;

    // [Header("Parent Constraint - Snap To rigHead")]
    public ParentConstraint aiSensorParentConstraint;

    // [Header("Movesets")]

    private void OnEnable()
    {
        ResetRigRelatedRefs();
        // Setup Rig And IK
    }

    public void ResetRigRelatedRefs()
    {
        if (cameraTarget)
        {
            CM_StateDriveFreeLook.LookAt = cameraTarget;
            CM_LockOn.Follow = cameraTarget;
        }

        // Parent Constraints
        /// Disabled///Snap AISensor to rigHead for more realistic sight sensing, but could break sight while performing evade/dive roll. Disabled
        // ConstraintCommon.ResetCSrcToSingleTargetParent(aiSensorParentConstraint, _fullBodyBipedIK.references.head);

        if (_fullBodyBipedIK.references.leftFoot.GetChild(0) != null)
        {
            var footLTriggerPrCnstr = _charCompMovementEffects.m_MovementEffects.m_LeftFootDetection
                .GetComponent<ParentConstraint>();
            ConstraintCommon.ResetCSrcToSingleTargetParent(footLTriggerPrCnstr,
                _fullBodyBipedIK.references.leftFoot.GetChild(0));
        }

        if (_fullBodyBipedIK.references.rightFoot.GetChild(0) != null)
        {
            var footRTriggerPrCnstr = _charCompMovementEffects.m_MovementEffects.m_RightFootDetection
                .GetComponent<ParentConstraint>();
            ConstraintCommon.ResetCSrcToSingleTargetParent(footRTriggerPrCnstr,
                _fullBodyBipedIK.references.rightFoot);
        }

        // _charCompMovementEffects.m_MovementEffects.m_LeftFootDetection.transform.position = cSource.sourceTransform.position;
    }


    #endregion Comp Refs


}