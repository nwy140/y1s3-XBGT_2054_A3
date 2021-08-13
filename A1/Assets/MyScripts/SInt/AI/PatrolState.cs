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

        targetTime = timerInSeconds;
    }
    public float timerInSeconds = 3f;
    public float targetTime;

    public float randomMoveRadius = 5f;
    public Vector2 randomSelectedPosInCircle;
    public float targetAngle;
    public Vector3 vectorInput = Vector2.up;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0)
        {
            randomSelectedPosInCircle = Random.insideUnitCircle * randomMoveRadius;

            targetTime = timerInSeconds;
        }

        targetAngle = Vector2Common.GetRotBetween2Pos(_ur.transform.position, randomSelectedPosInCircle);
        if (targetAngle >= 3f)
        {
            vectorInput.x = targetAngle / (360f*2);
        }
        else
        {
            vectorInput.x = 0;
        }
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal).Axis = vectorInput.x;
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal).buttonDown = true;
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical).Axis = 1;
        _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical).buttonDown = true;

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
