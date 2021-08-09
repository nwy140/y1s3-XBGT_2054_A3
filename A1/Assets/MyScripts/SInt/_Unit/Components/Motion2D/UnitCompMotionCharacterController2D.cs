using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCompMotionCharacterController2D : MonoBehaviour
{
    [Header("UnitComp - Motion")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveAcelSpeed = 2.0f;
    public float maxMoveAcelSpeed = 10f;
    public float curAcelSpeed = 0;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintAcelSpeed = 5.335f;
    public float maxSprintAcelSpeed = 20f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 1000)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    //public float _verticalVelocity;
    private float _terminalVelocity = 53.0f;



    [Header("Set in Inspector")]
    // CharRefs
    public UnitRefs _UnitRefs;
    bool _hasUnitRefs;

    [Header("Set in Awake")]
    private bool _hasAnimator;
    private Animator _animator;
    private bool hasRigidbody2D;
    public Rigidbody2D _rigidbody2D;


    [Header("Motion Custom Variables")]
    public EUnitMovementMode movementMode;

    [Header("Exposed to Ability Components")]
    public bool abilityIsGroundSprinting;
    public bool abilityIsGroundJumping;
    public float abilityCurrSprintAxis = 1f;
    public Vector2 abilityCurrMoveDir;

    private void Awake()
    {
        _hasUnitRefs = TryGetComponent(out _UnitRefs);
        _hasAnimator = TryGetComponent(out _animator);
        hasRigidbody2D = TryGetComponent(out _rigidbody2D);
    }
    private void FixedUpdate()
    {

        //JumpAndGravity();
        //GroundedCheck();
        Move();
        //Move_NotRelativeToCamera();
    }

    public void Move()
    {
        curAcelSpeed = Mathf.Clamp(abilityCurrMoveDir.y * MoveAcelSpeed * Time.deltaTime, -maxMoveAcelSpeed, maxMoveAcelSpeed);
        _rigidbody2D.AddRelativeForce(curAcelSpeed * Vector2.up);
        transform.eulerAngles += abilityCurrMoveDir.x * RotationSmoothTime * Time.deltaTime * -Vector3.forward;
    }


    private void AssignAnimationIDs()
    {
        //_animIDSpeed = Animator.StringToHash("Speed");
        //_animIDGrounded = Animator.StringToHash("Grounded");
        //_animIDJump = Animator.StringToHash("Jump");
        //_animIDFreeFall = Animator.StringToHash("FreeFall");
        //_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

}
