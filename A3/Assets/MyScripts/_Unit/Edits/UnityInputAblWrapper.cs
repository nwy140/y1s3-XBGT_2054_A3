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

    public SupportCompSysInventory sysInventory;

    private void FixedUpdate()
    {
        if (_ur.unitCompAbilityManager.eUnitPossesion == EUnitPossesionType.player)
        {
            var ablMan = _ur.unitCompAbilityManager;
            /*
            #region Old
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

            #endregion old
            */

            #region New
            if (ablMan.BasicTaskAblSlot_A1 != null)
                ablMan.BasicTaskAblSlot_A1.Axis = Input.GetAxis("Horizontal");
            if (ablMan.BasicTaskAblSlot_A2 != null)
                ablMan.BasicTaskAblSlot_A2.Axis = Input.GetAxis("Vertical");

            ablMan.BasicTaskAblSlot_B4.button = Input.GetButton("Fire3");
            _ur.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.GroundSprint).Axis = Input.GetAxis("Fire3");

            ablMan.BasicTaskAblSlot_B1.buttonDown = Input.GetKeyDown(key_GroundEvade1) || Input.GetKeyDown(key_GroundEvade2);

            ablMan.BasicTaskAblSlot_C1.button =
                        (Input.GetButton("Jump") || Input.GetKey(key_RegularAtkRange2D)) && Input.GetKey(key_RegularAtkAim) == false;
            ablMan.BasicTaskAblSlot_C2.button = Input.GetKey(key_RegularAtkAim);
            #endregion New

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                sysInventory.uiInventory.UseItemByUIItemIndex(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                sysInventory.uiInventory.UseItemByUIItemIndex(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                sysInventory.uiInventory.UseItemByUIItemIndex(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                sysInventory.uiInventory.UseItemByUIItemIndex(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                sysInventory.uiInventory.UseItemByUIItemIndex(4);
            }
        }
    }
}
