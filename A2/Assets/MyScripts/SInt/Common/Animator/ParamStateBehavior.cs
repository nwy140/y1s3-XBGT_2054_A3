using UnityEngine;
 
// Ref: https://www.youtube.com/watch?v=BHs7IqQSuSc&list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU&index=2 (Script used for setting animator param values, based on their types)
public class ParamStateBehavior : StateMachineBehaviour
{

    [Header("OnStateEnter")]
    public SetParamStateData[] OnStateEnterParamDatas;
    //[Header("Layer Weights")]
    //public bool Enter_isAnimLayerWeightOverride;
    //public string Enter_AnimLayerName;
    //public float OnStateEnter_newAnimLayerWeight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Ref: https://youtu.be/EmJ9PXF61m0?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU&t=546
        // Use this script to fix animator parameters transition issues
        // i.e double click calling same animation state twice
        foreach (SetParamStateData data in OnStateEnterParamDatas)
        {
            SetParamStateData.SetAnimatorParamDatas(data, animator);
        }
        //if (Enter_AnimLayerName.Length > 0 && Enter_isAnimLayerWeightOverride == true)
        //{
        //    animator.SetLayerWeight(animator.GetLayerIndex(Enter_AnimLayerName), OnStateEnter_newAnimLayerWeight);
        //}
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (animator.GetComponent<CharacterController>() != null)
        //{
        //    CharacterController cc = animator.GetComponent<CharacterController>();
        //    if (scriptMotionForwardVectorSpeed != 0)
        //    {
        //        cc.Move(animator.transform.forward * scriptMotionForwardVectorSpeed
        //            * Time.deltaTime);
        //    }
        //    if (scriptMotionRightVectorSpeed != 0)
        //    {
        //        cc.Move(animator.transform.right * scriptMotionRightVectorSpeed
        //            * Time.deltaTime);
        //    }
        //}
    }

    /*
    [Header("OnStateExit")]
    public SetParamStateData[] OnStateExitParamDatas;
    public bool OnStateExit_isAnimLayerWeightOverride;
    public string OnStateExit_AnimLayerName;
    public float OnStateExit_newAnimLayerWeight;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (SetParamStateData data in OnStateExitParamDatas)
        {
            SetParamStateData.SetAnimatorParamDatas(data, animator);
        }
        if (OnStateExit_AnimLayerName.Length > 0 && OnStateExit_isAnimLayerWeightOverride == true)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(OnStateEnter_AnimLayerName), OnStateExit_newAnimLayerWeight);
        }
    }

    //IEnumerator SetParamStateDataWithDelay(SetParamStateData data, Animator animator, AnimatorStateInfo stateInfo,
    //    int layerIndex)
    //{
    //    while (stateInfo.normalizedTime < 1.0f)
    //    {
    //        //Wait every frame until animation has finished
    //        yield return null;
    //    }
    //    animator.SetBool(data.paramName, data.setDefaultState);
    //    yield return null;
    //}
    [Header("OnStateUpdate")]
    public SetParamStateData[] OnStateUpdateParamDatas;
    public bool OnStateUpdate_isAnimLayerWeightOverride;
    public string OnStateUpdate_AnimLayerName;
    public float OnStateUpdate_newAnimLayerWeight;
    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (SetParamStateData data in OnStateUpdateParamDatas)
        {
            SetParamStateData.SetAnimatorParamDatas(data, animator);
        }
        if (OnStateUpdate_AnimLayerName.Length > 0 && OnStateUpdate_isAnimLayerWeightOverride == true)
        {
            animator.SetLayerWeight(animator.GetLayerIndex(OnStateUpdate_AnimLayerName), OnStateUpdate_newAnimLayerWeight);
        }
    }
    */

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
public enum EAnimParamType
{
    BOOL_SetDefaultState,
    TRIGGER_Set,
    TRIGGER_Reset,
    INT,
    FLOAT,
}

[System.Serializable]
public class SetParamStateData
{
    public string paramName;
    // public Enumstate param Type // if statement on param type, trigger, bool, int
    public EAnimParamType animParamType;
    public bool setDefaultState; // if is bool param type
    public int ifIsIntParamType_NewInt; // if is int param type
    public float ifIsFloatParamType_NewFloat; // if is int param type

    // Overload Methods parameters
    public static void SetAnimatorParamDatas(SetParamStateData data, Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (data.animParamType == EAnimParamType.BOOL_SetDefaultState)
        {
            animator.SetBool(data.paramName, data.setDefaultState);
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Set)
        {
            animator.SetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Reset)
        {
            animator.ResetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.INT)
        {
            animator.SetInteger(data.paramName, data.ifIsIntParamType_NewInt);
        }
    }
    public static void SetAnimatorParamDatas(SetParamStateData data, Animator animator)
    {
        if (data.animParamType == EAnimParamType.BOOL_SetDefaultState)
        {
            animator.SetBool(data.paramName, data.setDefaultState);
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Set)
        {
            animator.SetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Reset)
        {
            animator.ResetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.INT)
        {
            animator.SetInteger(data.paramName, data.ifIsIntParamType_NewInt);
        }
    }
    public static void SetAnimatorParamDatas(SetParamStateData data, Animator animator, bool isInvertBOOL, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (data.animParamType == EAnimParamType.BOOL_SetDefaultState)
        {
            if (isInvertBOOL == false)
            {
                animator.SetBool(data.paramName, !data.setDefaultState);
            }
            else
            {
                animator.SetBool(data.paramName, data.setDefaultState);
            }
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Set)
        {
            animator.SetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Reset)
        {
            animator.ResetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.INT)
        {
            animator.SetInteger(data.paramName, data.ifIsIntParamType_NewInt);
        }
    }
    public static void SetAnimatorParamDatas(SetParamStateData data, Animator animator, bool isInvertBOOL)
    {
        if (data.animParamType == EAnimParamType.BOOL_SetDefaultState)
        {
            if (isInvertBOOL == true)
            {
                animator.SetBool(data.paramName, !data.setDefaultState);
            }
            else
            {
                animator.SetBool(data.paramName, data.setDefaultState);
            }
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Set)
        {
            animator.SetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.TRIGGER_Reset)
        {
            animator.ResetTrigger(data.paramName);
        }
        else if (data.animParamType == EAnimParamType.INT)
        {
            animator.SetInteger(data.paramName, data.ifIsIntParamType_NewInt);
        }
    }

}