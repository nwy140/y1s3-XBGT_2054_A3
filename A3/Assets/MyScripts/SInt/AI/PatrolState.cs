using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : StateMachineBehaviour
{

    ISupportComp getOwnerUnitRefComp;
    bool hasGetOwnerUnitRefComp;
    UnitRefs _ur;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasGetOwnerUnitRefComp == false)
        {
            hasGetOwnerUnitRefComp = animator.TryGetComponent<ISupportComp>(out getOwnerUnitRefComp);
            _ur = getOwnerUnitRefComp._OwnerUnitRefs;
        }

    }
    public float timerInSeconds = 5f;
    public float targetTime;

    public float randomMoveRadius = 5f;
    public Vector2 randomSelectedPosInCircle;
    public float targetAngle = 2f;
    public Vector3 vectorInput = Vector2.up;
    public float angle;


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0)
        {
            randomSelectedPosInCircle = Vector2.one * _ur.transform.position + Random.insideUnitCircle * randomMoveRadius;

            targetTime = timerInSeconds;
            Vector2 dir = (Vector2.one * _ur.transform.position - randomSelectedPosInCircle).normalized;
            Debug.DrawLine(randomSelectedPosInCircle, randomSelectedPosInCircle + dir * 10, Color.red, Mathf.Infinity);
            vectorInput = dir;
        }
        angle = Vector2.Angle(_ur.transform.up, randomSelectedPosInCircle - _ur.transform.position * Vector2.one);
        if (angle < targetAngle)
        {
            vectorInput = Vector2.zero;
            angle = 0;
        }
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal).Axis = vectorInput.x;
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal).buttonDown = true;
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical).Axis = 1.5f;
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical).buttonDown = true;

        var lockOnSightPerceptionAblComp = (AbilityCompAssistCameraLockOn)(_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.LockOn));
        foreach (var obj in lockOnSightPerceptionAblComp.m_CandidateTargets)
        {
            if(obj.tag == "Player")
            {
                _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.RegularAtkAim).buttonDown = true;
            }
        }
        

    }

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
