using UnityEngine;
using System.Collections.Generic;

public class UnitCompAbilityManager : MonoBehaviour
{
    public List<AbilityBaseComp> unitAblComponents; // Abl is short for abilities
    public List<AbilityBaseComp> activeAblComponent; // Abl is short for abilities

    public List<ISupportComp> supportComponents;
    public UnitRefs _ownerUnitRefs;
    void Awake()
    {
        unitAblComponents = new List<AbilityBaseComp>(GetComponentsInChildren<AbilityBaseComp>());
        activeAblComponent = new List<AbilityBaseComp>(System.Array.FindAll(unitAblComponents.ToArray(), a => a.isActiveAndEnabled && a.isUsageRequirementsMet));

        foreach (AbilityBaseComp ablComp in unitAblComponents)
        {
            ablComp._ownerUnitRefs = this._ownerUnitRefs;
        }

        supportComponents = new List<ISupportComp>(GetComponentsInChildren<ISupportComp>());
        foreach (var supportComp in supportComponents)
        {
            supportComp._OwnerUnitRefs = this._ownerUnitRefs;
        }
    }

    AbilityBaseComp GetActiveAbilityCompByEnum(EAbilityTechniques eAbilityTechniques)
    {
        foreach (AbilityBaseComp ablComp in activeAblComponent)
        {
            if(ablComp.eAbilityTechniques == eAbilityTechniques)
            {
                return ablComp;
            }
        }
        return null;
    }
}

