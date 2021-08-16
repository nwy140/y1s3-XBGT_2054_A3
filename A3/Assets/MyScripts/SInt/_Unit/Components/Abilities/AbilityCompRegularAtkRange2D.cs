using System.Collections;
using System.Collections.Generic;
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
    public List<Transform> muzzleSockets;

    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveStay();

        foreach (Transform muzzle in muzzleSockets)
        {
            bool hasImpactEffectComp;
            ImpactEffect impactEffect;
            Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            Instantiate(muzzleFX, muzzle.position, muzzle.rotation);
            hasImpactEffectComp = projectilePrefab.TryGetComponent(out impactEffect);

            if (hasImpactEffectComp)
            {
                impactEffect.instigator = _ownerUnitRefs.gameObject;
            }
        }

    }
}
