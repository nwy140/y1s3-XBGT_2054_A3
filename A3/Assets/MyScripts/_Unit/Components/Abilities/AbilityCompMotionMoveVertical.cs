using System.Collections;
using UnityEngine;

public class AbilityCompMotionMoveVertical : AbilityBaseComp
{
    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.MoveVertical;
        base.Awake();
        desc = "Move Forward or Backwards";
        devComment = "Sync MoveVertical Axis with abilityCurrMoveDir.y float variable in _unitCharacterController, AI Requires both axis to be passed as parameters";
        
        if(eUnitPossesion == EUnitPossesionType.player)
        {
            willActivateAbility_OnUpdate = true;
        }
        rejectedAnimBoolParamStateNames.Add(nameof(EUnitAnimParamNames.isActionLocked));
    }
    public override void OnInit()
    {
        if (eUnitPossesion == EUnitPossesionType.ai)
        {
        }
        else if (eUnitPossesion == EUnitPossesionType.player)
        {
            willActivateAbility_OnUpdate = true;
        }
    }
    public override void OnUsageRequirementsNotMet()
    {
        base.OnUsageRequirementsNotMet();
        Axis = 0;
        _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.y = 0;
    }
    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();
    }

    public override void AbilityFunctionalityPlayer()
    {
        base.AbilityFunctionalityPlayer();
        _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.y = Axis;

    }

    public override void AbilityFunctionalityAI()
    {
        base.AbilityFunctionalityAI();
        if (buttonDown)
        {
            _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.y = Axis;
        }
        else
        {
            _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.y = 0;
        }
    }

    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        AbilityFunctionality();
    }
}
