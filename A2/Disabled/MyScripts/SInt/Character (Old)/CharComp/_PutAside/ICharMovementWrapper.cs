using Rewired;
using UnityEngine;

namespace MyScripts.SInt.Character.CharComp
{
    public interface ICharMovementWrapper
    {


        #region Getters And Setters
        public Vector2 CurMoveDir { get; set; }
        public Vector2 CurMoveInputAxis2D { get; set; }
        public bool IsJumping { get; set; }
        public bool IsSprinting { get; set; }
        public float CurrSprintAxis { get; set; }

        #endregion Getters And Setters
        #region Rewired Input System

        public void OnMoveX(InputActionEventData data);


        /// <summary>
        /// Provides the input vector for the move control.
        /// </summary>
        /// <param name="data">data is required for GetButton</param>
        public void OnMoveY(InputActionEventData data);
        public void OnEvade(InputActionEventData data);
        public void OnJump(InputActionEventData data);
        public void OnGroundSprint(InputActionEventData data);
 
        #region Unused

        #endregion Unused

        #endregion Rewired Input System

        #region Regular Use
        public void MoveInput(Vector2 newMoveDirection);
        public void MoveInputX(float newMoveXDirection);
        public void MoveInputY(float newMoveYDirection);
        // public void LookInput(Vector2 newLookDirection);
        public void Jump(bool newJumpState);
        public void GroundJumpInput(bool newJumpState);
        public void GroundSprintInput(bool newSprintState);
        public void GroundSprintInput(float sprintAxisValue);

        public void AirJumpInput(bool newJumpState);
        public void AirEvadeInput();

        public void OnApplicationFocus(bool hasFocus);
        public void SetCursorState(bool newState);
        #endregion Regular Use

        #region Handling
        void HandleEMovementMode();
        #endregion
        #region Integrated Character Controller Specific

        #region StarterAssets Specific
        #region Redundant Stuff
        public bool IsAnalogMovement { get; set; }
        public bool IsGrounded { get; set; }
        #endregion Redundant Stuff

        #endregion StartAssets Specific

        #endregion Integrated Character Controller Specific
    }
}