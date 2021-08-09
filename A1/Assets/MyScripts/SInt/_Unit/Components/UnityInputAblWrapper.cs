using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputAblWrapper : MonoBehaviour
{
    public UnitRefs _ur;

    private void Update()
    {
        if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal))
        {
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal).Axis = Input.GetAxis("Horizontal");
            print("OK");
        }
        if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical))
        {
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical).Axis = Input.GetAxis("Vertical");
        }   

    }
}
