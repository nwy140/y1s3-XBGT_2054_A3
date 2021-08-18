using System.Collections.Generic;
using UnityEngine;
namespace experimental
{

    public class CharState : StateMachineBehaviour
    {
        public List<StateData> ListAbilityData = new List<StateData>();
        public void UpdateAll(CharState charState, Animator animator)
        {
            foreach(StateData d in ListAbilityData)
            {
                d.UpdateAbility(charState, animator);
            }
        }

        UnitRefs _ownerUnitRefs;
        bool hasOwnerUnitRefs;
        public UnitRefs GetOwnerUnitRefs(Animator animator)
        {
            if(hasOwnerUnitRefs == false)
            {
                hasOwnerUnitRefs = animator.TryGetComponent(out _ownerUnitRefs);
            }
            return _ownerUnitRefs;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            UpdateAll(this, animator);
        }
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}


        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}