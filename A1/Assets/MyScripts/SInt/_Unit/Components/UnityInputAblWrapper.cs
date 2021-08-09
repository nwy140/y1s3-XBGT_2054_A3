using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputAblWrapper : MonoBehaviour
{
    public UnitRefs _ur;
    public KeyCode key_GroundEvade1 = KeyCode.LeftControl;
    public KeyCode key_GroundEvade2 = KeyCode.C;

    private void FixedUpdate()
    {
        //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal))
        //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal).Axis = Input.GetAxis("Horizontal");
        //}
        //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical))
        //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical).Axis = Input.GetAxis("Vertical");
        //}
        //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint))
        //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint).button = Input.GetButton("Fire3");
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint).Axis = Input.GetAxis("Fire3");
        //}
        //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundEvade))
        //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundEvade).buttonDown = Input.GetKeyDown(key_GroundEvade1) || Input.GetKeyDown(key_GroundEvade2);
        //}
    }
}
