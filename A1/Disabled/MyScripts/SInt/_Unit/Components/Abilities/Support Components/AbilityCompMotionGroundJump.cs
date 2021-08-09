using System.Collections;
using UnityEngine;

public class AbilityCompMotionGroundJump : AbilityBaseComp
{
    protected override void Awake()
    {
        eAbilityTechniques = EAbilityTechniques.GroundJump;
        base.Awake();
        desc = "Jump off the ground to reach greater heights";
        devComment = "Sync ButtonDown with abilityIsJumping bool variable in _unitCharacterController";
        rejectedAnimBoolParamStateNames.Add(nameof(EUnitAnimParamNames.isActionLocked));
    }



    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        // same for both Ai and Player
        _ownerUnitRefs._unitCharacterController.abilityIsGroundJumping = buttonDown;
    }

    //public override IEnumerator OnAbilityStarted()
    //{
    //    AbilityFunctionality();
    //    return base.OnAbilityStarted();
    //}
    //public override IEnumerator OnAbilityUpdating()
    //{
    //    AbilityFunctionality();

    //    return base.OnAbilityUpdating();
    //}
}
