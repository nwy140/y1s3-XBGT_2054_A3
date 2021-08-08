using System.Collections;
using UnityEngine;

public class AbilityCompMotionMoveHorizontal : AbilityBaseComp
{
    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.MoveHorizontal;
        base.Awake();
        desc = "Move Right or Left";
        devComment = "Sync MoveHorizontal Axis with abilityCurrMoveDir.x float variable in _unitCharacterController, AI Requires both axis to be passed as parameters";
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
        _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.x = 0;
    }

    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();
    }

    public override void AbilityFunctionalityPlayer()
    {
        base.AbilityFunctionalityPlayer();
        _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.x = Axis;
    }

    public override void AbilityFunctionalityAI()
    {
        base.AbilityFunctionalityAI();
        if (buttonDown)
        {
            _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.x = Axis;
        }
        else
        {
            _ownerUnitRefs._unitCharacterController.abilityCurrMoveDir.x = 0;
        }
    }

    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        AbilityFunctionality();
    }
}