using System.Collections;
using UnityEngine;

public class AbilityCompRegularAtkRange2D : AbilityBaseComp
{
    protected override void Awake()
    {
         base.Awake();
        eAbilityTechniques = EAbilityTechniques.RegularAtkRange2D;

        //desc = "Perform Regular Ground Attack Combo to inflict damage on Opponents";
        //devComment = "Get Melee Handler Component and call Atk Method";
    }
    public GameObject projectilePrefab;
    public GameObject muzzleFX;
    public Transform muzzleSocket;

    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveStay();
        bool hasImpactEffectComp;
        ImpactEffect impactEffect;
        Instantiate(projectilePrefab, muzzleSocket.position, muzzleSocket.rotation);
        Instantiate(muzzleFX, muzzleSocket.position, muzzleSocket.rotation);

        hasImpactEffectComp = projectilePrefab.TryGetComponent(out impactEffect);

        if (hasImpactEffectComp)
        {
            impactEffect.instigator = _ownerUnitRefs.gameObject;
        }
    }
}
