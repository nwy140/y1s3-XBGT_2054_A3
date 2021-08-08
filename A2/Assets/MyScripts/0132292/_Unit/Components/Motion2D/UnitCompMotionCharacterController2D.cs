using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCompMotionCharacterController2D : MonoBehaviour
{
    [Header("UnitComp - Motion")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]


    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    public float _verticalVelocity;
    private float _terminalVelocity = 53.0f;



    [Header("Set in Inspector")]
    // CharRefs
    public UnitRefs _UnitRefs;
    bool _hasUnitRefs;

    [Header("Set in Awake")]
    private bool _hasAnimator;
    private Animator _animator;
    private bool hasRigidbody2D;
    private Rigidbody2D _rigidbody2D;


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
    private void Update()
    {

        //JumpAndGravity();
        //GroundedCheck();
        Move();
        //Move_NotRelativeToCamera();
    }


    // Converted my own Move method from 3D to 2D // Commented 3D related code
    private void Move()
    {
        float inputMagnitude = abilityCurrMoveDir.magnitude;
        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = abilityIsGroundSprinting ? SprintSpeed /** abilityCurrSprintAxis*/ : MoveSpeed;
        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon
        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (abilityCurrMoveDir == Vector2.zero)
        {
            targetSpeed = 0.0f;
            inputMagnitude = 0.0f;
        }

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y).magnitude;

        float speedOffset = 0.1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

        // normalise input direction
        Vector2 inputDirection = new Vector2(abilityCurrMoveDir.x,  abilityCurrMoveDir.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (abilityCurrMoveDir != Vector2.zero)
        {
            //_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg ;
            //float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            //transform.rotation = Quaternion.Euler(0.0f, 0, rotation);
            transform.rotation = Quaternion.Euler(90 * inputDirection.x, 90 * inputDirection.y,0);
        }

        Vector2 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector2.up;
        if (abilityCurrMoveDir == Vector2.zero) targetDirection = Vector2.zero;

        // move the player
        _rigidbody2D.velocity += ((targetDirection.normalized * (_speed * Time.deltaTime) ));
            //new Vector2(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));
        //_rigidbody2D.AddForce((targetDirection.normalized * (_speed * Time.deltaTime) +
        //    new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime));

        // update animator if using character
        if (_hasAnimator)
        {
            //_animator.SetFloat(_animIDSpeed, _animationBlend);
            //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
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