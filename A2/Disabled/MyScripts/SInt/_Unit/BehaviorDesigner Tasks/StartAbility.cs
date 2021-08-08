using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

// Read BD Documentation: https://opsive.com/support/documentation/behavior-designer/tasks/


[TaskCategory("Unit/")]
[TaskDescription("Start An Ability, Pick the posession type for this task node i.e AI or Player. If you pick AI, leave buttonDown variable as true, you may pass in a variable as an axis for some abilities as tweaks. " +
    "If you pick Player, pass in your own variables for the button and axis")]
public class StartAbility : Action
{
    public UnitCompAbilityManager compAbilityManager;
    [TooltipAttribute("AI or Player possesing this ability?")]
    public SharedEUnitPossesionType eUnitPossesion = EUnitPossesionType.ai;
    
    [TooltipAttribute("Starts Ability Based on EAbilityTechniques Enum")]
    public SharedEAbilityTechniques eAbilityTechniques;
    public AbilityBaseComp CurrentAbilityComp;

    [TooltipAttribute("Simulate Input")]
    public SharedBool buttonDown = true;
    public SharedBool button;
    public SharedBool buttonUp;
    public SharedFloat Axis;

    private UnitRefs ownerUnitRefs;
    private bool hasownerUnitRefs;

    #region Behavior Designer's Methods
    public override void OnAwake()
    {
        hasownerUnitRefs = gameObject.TryGetComponent(out ownerUnitRefs);
        if (hasownerUnitRefs)
        {
            compAbilityManager = ownerUnitRefs.unitCompAbilityManager;
            CurrentAbilityComp = System.Array.Find(compAbilityManager.unitAblComponents.ToArray(), a => a.eAbilityTechniques == (eAbilityTechniques).Value);
            if (CurrentAbilityComp)
            {
                CurrentAbilityComp.eUnitPossesion = eUnitPossesion.Value;

                BD_HandleVariables();
                CurrentAbilityComp.OnInit();
            }
        }
    }

    public override TaskStatus OnUpdate()
    {
        BD_HandleVariables();
        if (CurrentAbilityComp == null)
        {
            return TaskStatus.Failure;
        }

        //CurrentAbilityComp.TryActivateAbility(); // Throw Null if not found

        return TaskStatus.Success;
        //if (CurrentAbilityComp.state == EAbilityState.started)
        //{
        //}
        //else if ((CurrentAbilityComp.isDoneCoolingDown == false && CurrentAbilityComp.state == EAbilityState.ready)) // CurrentAbilityComp.state == EAbilityState.interrupted || 
        //{
        //    // if interrupted or (Ready and Not Done Cooling Down)
        //    return TaskStatus.Failure;
        //}
        //return TaskStatus.Running;
    }

    public override void OnStart()
    {

    }
 

    public override void OnEnd()
    {

    }

    public override void OnReset()
    {

    }
    #endregion  Behavior Designer's Methods
    #region Handle Variables between Class and BD
    //[Header("Attributes-Set-BD")]
    //public SharedFloat Axis;

    public virtual void BD_HandleVariables()
    {
        if (CurrentAbilityComp)
        {
            if (Axis.IsNone == false)
            {
                CurrentAbilityComp.Axis = this.Axis.Value;
            }
            if (buttonDown.IsNone == false)
            {
                CurrentAbilityComp.buttonDown = this.buttonDown.Value;
            }
            if (button.IsNone == false)
            {
                CurrentAbilityComp.button = this.button.Value;
            }
            if (buttonUp.IsNone == false)
            {
                CurrentAbilityComp.buttonUp = this.buttonUp.Value;
            }
        }
    }
    #endregion  Handle Variables between Class and BD
}
//[TaskCategory("Unit/Abilities/Motion")]
//public class StartAbilityMotionMoveVertical : StartAbility
//{    //[Header("Attributes-Set-BD")]
//    public override void BD_HandleVariables()
//    {
//        base.BD_HandleVariables();
//    }
//}