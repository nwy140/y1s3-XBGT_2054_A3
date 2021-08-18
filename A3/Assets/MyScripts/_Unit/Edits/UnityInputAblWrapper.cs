using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInputAblWrapper : MonoBehaviour
{
    public UnitRefs _ur;
    public KeyCode key_GroundEvade1 = KeyCode.LeftControl;
    public KeyCode key_GroundEvade2 = KeyCode.C;
    public KeyCode key_RegularAtkRange2D = KeyCode.Mouse0;
    public KeyCode key_RegularAtkAim = KeyCode.Mouse1;

    private void FixedUpdate()
    {
        if (_ur.unitCompAbilityManager.eUnitPossesion == EUnitPossesionType.player)
        {
            //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal))
            //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveHorizontal2D).Axis = Input.GetAxis("Horizontal");
            //}
            //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical))
            //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.MoveVertical2D).Axis = Input.GetAxis("Vertical");
            //}
            //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint))
            //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint).button = Input.GetButton("Fire3");
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint).Axis = Input.GetAxis("Fire3");
            //}
            //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundEvade))
            //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundEvade2D).buttonDown = Input.GetKeyDown(key_GroundEvade1) || Input.GetKeyDown(key_GroundEvade2);
            //}
            //if (_ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.RegularAtkRange2D))
            //{
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.RegularAtkRange2D).button =
                        (Input.GetButton("Jump") || Input.GetKey(key_RegularAtkRange2D)) && Input.GetKey(key_RegularAtkAim) == false;
            //}
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.RegularAtkAim).button = Input.GetKey(key_RegularAtkAim);
        }
    }
}
