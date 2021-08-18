using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    [CreateAssetMenu(fileName = "MotionMoveHorizontal2D", menuName = "AbilityData/MotionMoveHorizontal2D")]
    public class MotionMoveHorizontal2D : StateData
    {
        public override void UpdateAbility(CharState charState, Animator animator)
        {
            base.UpdateAbility(charState, animator);
            Debug.Log("MotionMoveHorizontal2D");
        }
    }
}
 