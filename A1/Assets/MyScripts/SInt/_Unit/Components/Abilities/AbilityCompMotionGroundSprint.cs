using System.Collections;
using UnityEngine;

public class AbilityCompMotionGroundSprint : AbilityBaseComp
{
    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.GroundSprint;
        base.Awake();
        desc = "Move at a higher speed while on the Ground";
        devComment = "Sync Sprint Axis and Sprint Button variables in _unitCharacterController";
        //willStartAbility_OnButton = true;
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
        _ownerUnitRefs._unitCharacterController.abilityCurrSprintAxis = 0;
        _ownerUnitRefs._unitCharacterController.abilityIsGroundSprinting = false;
    }
    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();
        //_unitRefs._unitCharacterController.AbilityCurrSprintAxis = Axis;
        //_unitRefs._unitCharacterController.AbilityIsGroundSprinting = button;
    }

    public override void AbilityFunctionalityPlayer()
    {
        base.AbilityFunctionalityPlayer();
        _ownerUnitRefs._unitCharacterController.abilityIsGroundSprinting = button;
        _ownerUnitRefs._unitCharacterController.abilityCurrSprintAxis = Axis;
    }

    public override void AbilityFunctionalityAI()
    {
        base.AbilityFunctionalityAI();
        if (buttonDown)
        {
            _ownerUnitRefs._unitCharacterController.abilityIsGroundSprinting = true;
            _ownerUnitRefs._unitCharacterController.abilityCurrSprintAxis = Axis;
        }
        else
        {
            _ownerUnitRefs._unitCharacterController.abilityIsGroundSprinting = false;
            _ownerUnitRefs._unitCharacterController.abilityCurrSprintAxis = 0;
        }
    }
    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        AbilityFunctionality();
    }


}