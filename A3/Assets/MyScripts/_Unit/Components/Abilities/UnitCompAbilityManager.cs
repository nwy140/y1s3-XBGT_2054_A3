using UnityEngine;
using System.Collections.Generic;

public class UnitCompAbilityManager : MonoBehaviour
{
    public List<AbilityBaseComp> unitAblComponents; // Abl is short for abilities
    public List<AbilityBaseComp> activeAblComponent; // Abl is short for abilities

    public List<ISupportComp> supportComponents;
    public UnitRefs _ownerUnitRefs;

    public EUnitPossesionType eUnitPossesion;

    [Header("Ability Slots")]
    public AbilityBaseComp BasicTaskAblSlot_A1; //
    public AbilityBaseComp BasicTaskAblSlot_A2; // 

    public AbilityBaseComp BasicTaskAblSlot_B1; // 
    public AbilityBaseComp BasicTaskAblSlot_B2;
    public AbilityBaseComp BasicTaskAblSlot_B3;
    public AbilityBaseComp BasicTaskAblSlot_B4; // 

    public AbilityBaseComp BasicTaskAblSlot_C1; // 
    public AbilityBaseComp BasicTaskAblSlot_C2; // 

    public AbilityBaseComp BasicTaskAblSlot_D1;
    public AbilityBaseComp BasicTaskAblSlot_D2;
    void Awake()
    {
        unitAblComponents = new List<AbilityBaseComp>(GetComponentsInChildren<AbilityBaseComp>());

        foreach (AbilityBaseComp ablComp in unitAblComponents)
        {
            ablComp._ownerUnitRefs = this._ownerUnitRefs;
            ablComp.eUnitPossesion = eUnitPossesion;
        }

        supportComponents = new List<ISupportComp>(GetComponentsInChildren<ISupportComp>());
        foreach (var supportComp in supportComponents)
        {
            supportComp._OwnerUnitRefs = this._ownerUnitRefs;
        }
    }
    private void Start()
    {
        activeAblComponent = new List<AbilityBaseComp>(System.Array.FindAll(unitAblComponents.ToArray(), a => a.isActiveAndEnabled && a.isUsageRequirementsMet));
    }



    public AbilityBaseComp GetActiveAbilityCompByEnum(EAbilityTechniques eAbilityTechniques)
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

