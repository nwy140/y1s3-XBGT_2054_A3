using System.Collections;
using UnityEngine;

namespace MyScripts.SInt.Character
{
    [System.Serializable]
    public class MoveSetData
    {
        // Conditions
        // public bool isUnlocked;
        // public bool isEnabled;
        // public bool isMovesetOnGoing;

        public ECharMovesetName movesetName;
        public ECharMovesetState MovesetPerformState;

        public EUnitMovementMode MovementMode;

        // Footing
        public bool isAnimated = true;
        public bool isForceScriptMotion = false;
        public float scriptMotionPushMultiplier;
        public float hitVictimPushMultiplier;
        public float cooldownPerCombo;
        public int curComboIndex;

        public int comboLength;
        public float comboTimeDelay;

        // Range Type
        // Number of Opponents // whether it's aoe or a single target move
        public float comboAtkMultiplier; // CM // for our damage formula // see the google docs technical documentation

        // private float ClassTypeMultiplier;      //         [Taken Externally From Character Stats]

        // Extras
        // SFX
        // VFX

        // Instigator is the character that is the performing the move
        public IEnumerator PerformMoveset(CharRefs instigator)
        {
            Debug.Log("Try Perform Moveset: " + (movesetName).ToString() + " Started");
            if (instigator.currMovementMode ==
                MovementMode) // same movement mode, i.e falling in air, walking in ground
            {
                // if prev moveset is not isolated
                if ((instigator.currMovesetData.MovesetPerformState == ECharMovesetState.isolatedMotion
                    || instigator.currMovesetData.MovesetPerformState == ECharMovesetState.isolatedAttack)
                    && instigator.isMovesetOnGoing == false) // if is isolated
                {
                    instigator.currMovesetPerformingStateType = MovesetPerformState;
                    if (MovesetPerformState == ECharMovesetState.isolatedMotion) // if is motion
                    {
                        instigator.isMovesetOnGoing = true;
                        // Apply Instant Rotate Parent Root of Char Controller By Movement Axis, i.e usage in moveset direction, i.e Evade 
                        // Ref: https://answers.unity.com/questions/1259992/rotate-object-towards-joystick-input-using-c.html
                        // var tempEulerAngle =new Vector3( 0, Mathf.Atan2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 180 / Mathf.PI, 0 );
                        // if (tempEulerAngle != Vector3.zero)
                        // {
                        //     instigator.transform.parent.eulerAngles = tempEulerAngle;
                        // }
                        // instigator.transform.LookAt(instigator.transform.position+
                        //     Vector3.right *  instigator._charMovement.CurMoveInputAxis2D.x +
                        //     Vector3.up *  instigator._charMovement.CurMoveInputAxis2D.y
                        //     );
                        // TODO: LockedOn/PossibleTarget direction
                        if (isAnimated)
                        {


                            // perform animation by enum name, if is root motion, perform transform modification if is root motion
                            instigator._anim.SetTrigger((movesetName).ToString());
                            instigator._anim.SetBool(nameof(instigator.isMovesetOnGoing), instigator.isMovesetOnGoing);
                        }
                        else
                        {
                            instigator._anim.applyRootMotion = false;
                        }

                        if ( isForceScriptMotion == true)
                        {
                            instigator._characterController.SimpleMove(instigator.transform.forward *
                                                                       scriptMotionPushMultiplier);
                            // OPTIONAL: Rot and Scale
                        }
                        Debug.Log("Perform Moveset: " + (movesetName).ToString() + " Success");

                        yield return new WaitForSeconds(cooldownPerCombo);
                        instigator.isMovesetOnGoing = false;
                        instigator.currMovesetPerformingStateType = ECharMovesetState.None;
                        if (isAnimated)
                        {
                            instigator._anim.SetBool(nameof(instigator.isMovesetOnGoing), instigator.isMovesetOnGoing);
                        }
                    }
                }
            }

            yield return null;
        }
    }
}