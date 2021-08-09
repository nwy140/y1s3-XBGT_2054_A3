using MyScripts.SInt.Character.CharComp;
using UnityEngine;
using Rewired;
using StandardAssets.Characters.Common;
using StarterAssets;

namespace MyScripts.SInt.Character
{
// Read: https://guavaman.com/projects/rewired/docs/HowTos.html Method 2 section
// https://guavaman.com/projects/rewired/docs/api-reference/html/T_Rewired_InputActionEventType.htm
// https://forum.unity.com/threads/rewired-advanced-input-for-unity.270693/page-19
// Rewired also supports gamepad vibration
// Read: https://guavaman.com/projects/rewired/docs/FAQ.html#force-feedback
// InputActionEventType is an enum with all sort of properties i.e ButtonJustDoublePressed	
// Read: https://guavaman.com/projects/rewired/docs/api-reference/html/T_Rewired_InputActionEventType.htm
    public class CharInputRewiredWrapper : MonoBehaviour // Rewired Wrapper For StandardAsset's Character
    {
        public int playerId;
        private Player player;

        private ICharMovementWrapper _charMovement;
        private ICharMovesets _charMovesets;


        // Comp Refs
        private void Awake()
        {
            _charMovement = GetComponent<ICharMovementWrapper>();
            _charMovesets = GetComponent<ICharMovesets>();
        }

        void OnDisable()
        {
            UnsubscribeInputEvents();
        }
        void UnsubscribeInputEvents()
        {
            // Movement
            player.RemoveInputEventDelegate(OnInputUpdate);
            player.RemoveInputEventDelegate(_charMovement.OnMoveX);
            player.RemoveInputEventDelegate(_charMovement.OnMoveY);
            player.RemoveInputEventDelegate(_charMovement.OnEvade);
            player.RemoveInputEventDelegate(_charMovement.OnJump);

            // Movesets
            player.RemoveInputEventDelegate(_charMovement.OnGroundSprint);
            player.RemoveInputEventDelegate(_charMovesets.OnGeneralLockOn);
        }
        private void OnEnable()
        {
            SubscribeInputEvents();
        }

        // Input Binding - Delegates Events Style
        void SubscribeInputEvents()
        {
            player = ReInput.players.GetPlayer(playerId);

            // SInt DONE:  // replaced context parameter from Unity New Input System with Rewired InputActionEventData
            // Rewired's Workflow is better than Unity's new input system.

            // Add delegates to receive input events from the Player

            // This event will be called every frame any input is updated
            player.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);

            #region Rewired Rewired Input Event Bindings

            #region 0 : Default

            // This event will be called every frame the "Move Horizontal" axis is non-zero and once more when it returns to zero.
            player.AddInputEventDelegate(_charMovement.OnMoveX, UpdateLoopType.Update,
                InputActionEventType.AxisActiveOrJustInactive,
                nameof(EBtns.MoveHorizontal));
            // MoveVertical,
            // This event will be called every frame the "Move Horizontal" axis is non-zero and once more when it returns to zero.
            player.AddInputEventDelegate(_charMovement.OnMoveY, UpdateLoopType.Update,
                InputActionEventType.AxisActiveOrJustInactive,
                nameof(EBtns.MoveVertical));
            // Empty,
            // This event will be called when the "Empty" button is held for at least 1 seconds and then released
            player.AddInputEventDelegate(EmptyMethodLog, UpdateLoopType.Update,
                InputActionEventType.ButtonPressedForTimeJustReleased,
                nameof(EBtns.Empty), new object[] {1.0f});

            #endregion 0 : Default

            #region 1 : Motion

            // Evade,
            player.AddInputEventDelegate(_charMovement.OnEvade, UpdateLoopType.Update,
                InputActionEventType.ButtonJustPressed,
                nameof(EBtns.Evade));
            // Jump,
            // This event will be called every frame the "Jump" action is updated
            player.AddInputEventDelegate(_charMovement.OnJump, UpdateLoopType.Update,
                InputActionEventType.ButtonJustPressed,
                nameof(EBtns.Jump));
            // player.AddInputEventDelegate(_charInput.OnGroundJump, UpdateLoopType.Update,
            //     InputActionEventType.ButtonJustReleased,
            //     nameof(EBtns.Jump));
            // Sprint,
            player.AddInputEventDelegate(_charMovement.OnGroundSprint, UpdateLoopType.Update,
                InputActionEventType.AxisActiveOrJustInactive,
                nameof(EBtns.Sprint));
            //player.AddInputEventDelegate(_charMovement.OnGroundSprint, UpdateLoopType.Update,
            //    InputActionEventType.ButtonJustPressed,
            //    nameof(EBtns.Sprint));
            //player.AddInputEventDelegate(_charMovement.OnGroundSprint, UpdateLoopType.Update,
            //    InputActionEventType.ButtonJustReleased,
            //    nameof(EBtns.Sprint));

            #endregion 1 : Motion

            // Interact,

            #region 2 : Activity
            // Attack,
            player.AddInputEventDelegate(_charMovesets.OnRegularAttack, UpdateLoopType.Update,
                InputActionEventType.ButtonJustPressed,
                nameof(EBtns.Attack));
            // Attack, Press for time 
            player.AddInputEventDelegate(_charMovesets.OnChargedAttack, UpdateLoopType.Update, 
                InputActionEventType.ButtonPressedForTime, 
                nameof(EBtns.Attack), new object[] {0.14f});
 
            #endregion 2 : Activity

            #region 3 : Techniques

            #endregion 3 : Techniques

            #region 2 : Assist

            // Pause,

            #endregion 2 : Assist

            #region 5 : Camera

            // LockOn,
            player.AddInputEventDelegate(_charMovesets.OnGeneralLockOn, UpdateLoopType.Update,
                InputActionEventType.ButtonJustPressed,
                nameof(EBtns.LockOn));
            // player.AddInputEventDelegate(_charMovesets.OnGeneralLockOn, UpdateLoopType.Update,
            //     InputActionEventType.ButtonJustReleased,
            //     nameof(EBtns.LockOn));
            // LockOnCycle,
            // Strafe
            // player.AddInputEventDelegate(_charInput.OnStrafe, UpdateLoopType.Update,
            //     InputActionEventType.ButtonJustPressed,
            //     nameof(EBtns.Strafe));
            // player.AddInputEventDelegate(_charInput.OnStrafe, UpdateLoopType.Update,
            //     InputActionEventType.ButtonJustReleased,
            //     nameof(EBtns.Strafe));
            // mouseLookX,
            // mouseLookY,
            // gamepadLookX,
            // gamepadLookY,
            // Recentre
            // player.AddInputEventDelegate(_charInput.OnRecentre, UpdateLoopType.Update,
            //     InputActionEventType.ButtonJustPressed,
            //     nameof(EBtns.Recentre));

            #endregion 5 : Camera

            #endregion Rewired Input Event Bindings

            // The update loop you choose for the event matters. Make sure your chosen update loop is enabled in
            // the Settings page of the Rewired editor or you won't receive any events.
        }

        #region Rewired Input Update Bindings
        // Input Binding - Update GetKey Style
        void OnInputUpdate(InputActionEventData data)
        {
            //combo

        }

        public void EmptyMethodStub(InputActionEventData data)
        {
            // do nothing
        }

        public void EmptyMethodLog(InputActionEventData data)
        {
            Debug.Log("Action :" + data.actionName + "Held Time: " + data.GetButtonPrev());
        }

        #endregion Rewired Input Update Bindings

        #region Rewired Events Bindings

        // Empty for now

        #endregion Rewired Events Bindings


    }
}

#region Rewired Example Events Template

// void OnInputUpdate(InputActionEventData data)
// {
//     // You can't use ToString() in switch case so we'll have to use nameof instead. Read: https://stackoverflow.com/questions/1273228/how-can-i-use-the-string-value-of-a-c-sharp-enum-value-in-a-case-statement
//     // nameof doesn't produce garbage Read: https://stackoverflow.com/questions/35523172/what-is-the-difference-between-myenum-item-tostring-and-nameofmyenum-item
//     switch (data.actionName)
//     {
//         // determine which action this is
//         // case nameof(EBtns.MoveHorizontal):
//         //     if (data.GetAxis() != 0.0f) Debug.Log("Move Horizontal!" + data.GetAxis());
//         //     break;
//         // case nameof(EBtns.MoveVertical):
//         //     if (data.GetAxis() != 0.0f) Debug.Log("Move Vertical!" + data.GetAxis());
//         //     break;
//
//         // case nameof(EBtns.Jump):
//         //     if (data.GetButtonDown()) Debug.Log("Jumpa!");
//         //     break;
//     }
// }
// void SubscribeInputEvents()
// {
//     player = ReInput.players.GetPlayer(playerId);
//
//     // SInt DONE:  // replaced context parameter from Unity New Input System with Rewired InputActionEventData
//     // Rewired's Workflow is better than Unity's new input system.
//         
//     // Add delegates to receive input events from the Player
//
//     // This event will be called every frame any input is updated
//     player.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);
//
//     // // This event will be called every frame the "Fire" action is updated
//     // player.AddInputEventDelegate(OnAttackUpdate, UpdateLoopType.Update, "Attack");
//     //
//     // // This event will be called when the "Fire" button is first pressed
//     // player.AddInputEventDelegate(OnAttackButtonDown, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed,
//     //     "Attack");
//     //
//     // // This event will be called when the "Fire" button is first released
//     // player.AddInputEventDelegate(OnAttackButtonUp, UpdateLoopType.Update, InputActionEventType.ButtonJustReleased,
//     //     "Attack");
//     //
//     // // This event will be called every frame the "Move Horizontal" axis is non-zero and once more when it returns to zero.
//     // player.AddInputEventDelegate(OnMoveHorizontal, UpdateLoopType.Update,
//     //     InputActionEventType.AxisActiveOrJustInactive, "Move Horizontal");
//
//     // This event will be called when the "Jump" button is held for at least 2 seconds and then released
//     player.AddInputEventDelegate(_charInput.OnJump, UpdateLoopType.Update,
//         InputActionEventType.ButtonPressedForTimeJustReleased, "Jump", new object[] {2.0f});
//
//     // The update loop you choose for the event matters. Make sure your chosen update loop is enabled in
//     // the Settings page of the Rewired editor or you won't receive any events.
// }
// void OnAttackUpdate(InputActionEventData data)
// {
//     if (data.GetButtonDown()) Debug.Log("Attack Action Data Updated!");
// }
//
// void OnAttackButtonDown(InputActionEventData data)
// {
//     Debug.Log("Attack Pressed");
// }
//
// void OnAttackButtonUp(InputActionEventData data)
// {
//     Debug.Log("Attack Released!");
// }
//
// void OnJumpButtonUp(InputActionEventData data)
// {
//     Debug.Log("Jump Held for 2 seconds and released!");
// }
//
// void OnMoveHorizontal(InputActionEventData data)
// {
//     Debug.Log("Move Horizontal: " + data.GetAxis());
// }
// void UnsubscribeInputEvents()
// {
//     // Unsubscribe from events when object is destroyed
//     player.RemoveInputEventDelegate(OnInputUpdate);
//     player.RemoveInputEventDelegate(OnAttackUpdate);
//     player.RemoveInputEventDelegate(OnAttackButtonDown);
//     player.RemoveInputEventDelegate(OnAttackButtonUp);
//     player.RemoveInputEventDelegate(OnMoveHorizontal);
//     player.RemoveInputEventDelegate(OnJumpButtonUp);
// }

#endregion Rewired Example Events Template