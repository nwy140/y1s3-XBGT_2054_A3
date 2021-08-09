using System.Collections;
using UnityEngine;

public class AbilityCompCombatRegularAtk : AbilityBaseComp
{
    protected override void Awake()
    {
         base.Awake();
        desc = "Perform Regular Ground Attack Combo to inflict damage on Opponents";
        devComment = "Get Melee Handler Component and call Atk Method";
    }

    public int AttackType = 1; // use 1 for Regular Attack ,use 2 for Charged Attack, 

    public SupportCompCombatMeleeAtkHandler meleeAtkHandler;
    public SupportCompHitboxInflictorRigidHandler hitboxInflictorRigidHandler;
    // TODO: Atk Type Abilities, Range, AOE , Melee

    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        // same for both Ai and Player
        AbilityFunctionality();
    }

    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();
        meleeAtkHandler.OnAttack(AttackType);
    }


}
