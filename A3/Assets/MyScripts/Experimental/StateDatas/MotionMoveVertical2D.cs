using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    [CreateAssetMenu(fileName = "MotionMoveVertical2D", menuName = "AbilityData/MotionMoveVertical2D")]
    public class MotionMoveVertical2D : StateData
    {
        public override void UpdateAbility(CharState charState, Animator animator)
        {
            base.UpdateAbility(charState, animator);
            Debug.Log("MotionMoveVertical2D");
        }
    }
}
 