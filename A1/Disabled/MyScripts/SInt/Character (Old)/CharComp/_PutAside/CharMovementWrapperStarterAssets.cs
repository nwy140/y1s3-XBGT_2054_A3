using System;
using MyScripts.SInt.Character.CharComp;
using Rewired;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
#endif

namespace MyScripts.SInt.Character
{
    public class CharMovementWrapperStarterAssets : MonoBehaviour, ICharMovementWrapper
    {
        private CharRefs _cr;

        private void Awake()
        {
            _cr = GetComponent<CharRefs>();
        }

        [Header("Character Input Values")] 
        public Vector2 moveDir; // only for character movement

        public Vector2 moveInputAxis2D; // for other scripts to access // 
        public bool jump;
        public bool sprint;
        public float sprintAxis; // for other scripts to access // 

        #region  Redundant Stuff
        [Header("Movement Settings")] 
        public bool analogMovement; 
        public bool IsAnalogMovement { get=>analogMovement; set=>analogMovement = value;}
        #endregion Redundant Stuff

#if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")] public bool cursorLocked = true;
        public bool cursorInputForLook = true;
        private ICharMovementWrapper _charMovement;
#endif
        #region Getters And Setters
        public Vector2 CurMoveDir {get => moveDir; set => moveDir = value; }
        public Vector2 CurMoveInputAxis2D {get => moveDir; set => moveDir = value; }
        public bool IsJumping {get => jump; set => jump = value; }
        public bool IsSprinting {get => sprint; set => sprint = value; }
        public float CurrSprintAxis { get => sprintAxis; set => sprintAxis = value; }
        public bool IsGrounded {get => _cr.starterAssetsThirdPersonController.Grounded; set => _cr.starterAssetsThirdPersonController.Grounded = value; }
        #endregion Getters And Setters
        #region Rewired Input System

        public void OnMoveX(InputActionEventData data)
        {
            MoveInputX(data.GetAxis());
        }


        /// <summary>
        /// Provides the input vector for the move control.
        /// </summary>
        /// <param name="data">data is required for GetButton</param>
        public void OnMoveY(InputActionEventData data)
        {
            MoveInputY(data.GetAxis());
        }

        public void OnEvade(InputActionEventData data)
        {
            // Validate movement mode, then decide whether to call by air or ground
            if (_cr.currMovementMode == EUnitMovementMode.movingOnGround)
            {
                GroundEvadeInput();
            } else if (_cr.currMovementMode == EUnitMovementMode.fallingOnAir)
            {
                AirEvadeInput();
            }
        }

        public void OnJump(InputActionEventData data)
        {
            Jump(data.GetButton());
        }

        public void OnGroundSprint(InputActionEventData data)
        {
            // Use this method if you want the sprint speed to not be relative to how hard the sprint Button is pressed on the joystick 
            GroundSprintInput(data.GetButton()); 

            // Use this method if you want the sprint speed to be relative to how hard the sprint Button is pressed on the joystick
            // This is useful for accurate vehicle acceleration to simulate a car pedal
            //GroundSprintInput(data.GetAxis()); 
        }
 

        #region Unused

        // Unused
        // public void OnLook(InputActionEventData data)
        // {
        // 	if(cursorInputForLook)
        // 	{
        // 		LookInput(value.Get<Vector2>());
        // 	}
        // }

        #endregion Unused

        #endregion Rewired Input System


// #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
//         public void OnMove(InputValue value)
//         {
//         	MoveInput(value.Get<Vector2>());
//         }
//         
//         public void OnLook(InputValue value)
//         {
//         	if(cursorInputForLook)
//         	{
//         		LookInput(value.Get<Vector2>());
//         	}
//         }
//         
//         public void OnJump(InputValue value)
//         {
//         	JumpInput(value.isPressed);
//         }
//         
//         public void OnSprint(InputValue value)
//         {
//         	SprintInput(value.isPressed);
//         }
// #else
// 	// old input sys if we do decide to have it (most likely wont)...
// #endif

        #region  Regular Use
        public void MoveInput(Vector2 newMoveDirection)
        {
            moveInputAxis2D = newMoveDirection;
            if (_cr._anim.GetBool(nameof(ECharAnimParam.isActionLocked)) == true || _cr._anim.hasRootMotion) // root motion enabled, disable script motion
            {
                newMoveDirection = Vector2.one;
            }
            moveDir = newMoveDirection;
        }

        public void MoveInputX(float newMoveXDirection)
        {
            moveInputAxis2D = moveDir = Vector2.right * newMoveXDirection
                                      + Vector2.up * moveInputAxis2D.y;
            if (_cr._anim.GetBool(nameof(ECharAnimParam.isActionLocked)) == true || _cr._anim.hasRootMotion) // root motion enabled, disable script motion
            {
                newMoveXDirection = 0;
            }

            moveDir = moveDir = Vector2.right * newMoveXDirection
                                + Vector2.up * moveDir.y;
        }

        public void MoveInputY(float newMoveYDirection)
        {
 
            moveInputAxis2D = Vector2.right * moveInputAxis2D.x
                            + Vector2.up * newMoveYDirection;
            if (_cr._anim.GetBool(nameof(ECharAnimParam.isActionLocked)) == true || _cr._anim.hasRootMotion) // root motion enabled, disable script motion
            {
                newMoveYDirection = 0;
            }
            moveDir = Vector2.right * moveDir.x
                   + Vector2.up * newMoveYDirection;
        }

        // public void LookInput(Vector2 newLookDirection)
        // {
        //     look = newLookDirection;
        // }

        public void Jump(bool newJumpState)
        {
            // Validate movement mode, then decide whether to call by air or ground
            if (_cr.currMovementMode == EUnitMovementMode.movingOnGround)
            {
                GroundJumpInput(newJumpState);
            } else if (_cr.currMovementMode == EUnitMovementMode.fallingOnAir)
            {
                AirJumpInput(newJumpState);
            }            
        }

        public float groundEvadePushForce = 10f;

        public void GroundEvadeInput()
        {
            if (_cr._anim.GetBool(nameof(ECharAnimParam.isActionLocked)) == true)
            {
                return;
            }
            StartCoroutine( _cr.currMovesetData.PerformMoveset(_cr));
        }
        public void GroundJumpInput(bool newJumpState)
        {
            if (_cr._anim.GetBool(nameof(ECharAnimParam.isActionLocked)) == true)
            {
                return;
            }
            jump = newJumpState;
        }

        public void GroundSprintInput(bool newSprintState)
        {
            //if (_cr._anim.GetBool(nameof(ECharAnimParam.isActionLocked)) == true)
            //{
            //    newSprintState = false;
            //}
            sprint = newSprintState;
            sprintAxis = 1f;
        }
        public void GroundSprintInput(float newSprintAxisValue)
        {
            sprintAxis = newSprintAxisValue;
            if (newSprintAxisValue != 0)
            {
                sprint = true;
            }
            else
            {
                sprint = false;
            }
        }

        public void AirJumpInput(bool newJumpState)
        {
            // throw new NotImplementedException();
        }

        public void AirEvadeInput()
        {
            // throw new NotImplementedException();
        }

        #endregion Regular Use
        #region Handling

        private void Update()
        {
            HandleEMovementMode();
        }

        public void HandleEMovementMode()
        {
            if (IsGrounded == true)
            {
                _cr.currMovementMode = EUnitMovementMode.movingOnGround;
            }
            else // && not in swimableLayer or flying Layer , 
            {
                _cr.currMovementMode = EUnitMovementMode.fallingOnAir;
            }
        }
        #endregion


#if !UNITY_IOS || !UNITY_ANDROID

        public void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        public void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

#endif
    }
}