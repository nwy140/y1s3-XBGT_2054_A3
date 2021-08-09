using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ref: https://youtu.be/BHs7IqQSuSc?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU
public class TimedParamStateBehavior : StateMachineBehaviour
{
    public SetParamStateData data;
    public float Start, End; // Animation percentage

    private bool _waitForExit;
    private bool _onTransitionExitTriggered;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _waitForExit = false;
        _onTransitionExitTriggered = false;
        //animator.SetBool(data.paramName, !data.setDefaultState);
        SetParamStateData.SetAnimatorParamDatas(data, animator, true); // invert bool
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CheckOnTransitionExit(animator, layerIndex))
        {
            OnStateTransitionExit(animator);
        }
        if (_onTransitionExitTriggered == false && stateInfo.normalizedTime >= Start && stateInfo.normalizedTime <= End)
        {
            //animator.SetBool(data.paramName, data.setDefaultState);
            SetParamStateData.SetAnimatorParamDatas(data, animator, false); // invert bool
        }
    }

    private void OnStateTransitionExit(Animator animator)
    {
        SetParamStateData.SetAnimatorParamDatas(data, animator, true); // revert to original value // bool invert
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_onTransitionExitTriggered == false)
        {
            //animator.SetBool(paramName, !data.setDefaultState);
            SetParamStateData.SetAnimatorParamDatas(data, animator, true); // invert bool
        }
    }

    private bool CheckOnTransitionExit(Animator animator, int layerIndex)
    {
        if (_waitForExit == false && animator.GetAnimatorTransitionInfo(layerIndex).fullPathHash == 0)
        {
            _waitForExit = true;
        }
        if (_onTransitionExitTriggered == false && _waitForExit == true && animator.IsInTransition(layerIndex))
        {
            _onTransitionExitTriggered = true;
            return true;
        }
        return false;
    }

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
