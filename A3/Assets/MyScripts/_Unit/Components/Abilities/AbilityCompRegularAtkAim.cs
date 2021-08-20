using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCompRegularAtkAim : AbilityBaseComp
{
    protected override void Awake()
    {
        base.Awake();
        eAbilityTechniques = EAbilityTechniques.RegularAtkAim;

        //desc = "Perform Regular Ground Attack Combo to inflict damage on Opponents";
        //devComment = "Get Melee Handler Component and call Atk Method";
        cursor.transform.position = transform.position + transform.up * 5;
    }

    public GameObject cursor;
    public GameObject projectilePrefab;
    public GameObject muzzleFX;
    public List<Transform> muzzleSockets;

    public float aimRotOffset = -90;

    public Vector2 targetPos;
    public bool isResetRotAfterAim = false;


    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();

        if (eUnitPossesion == EUnitPossesionType.player)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3000))
            {
                cursor.transform.position = hit.point;
                cursor.transform.eulerAngles = Vector3.zero;
                //Debug.Log(hit.point);
                targetPos = hit.point;
            }
        }
        else if (eUnitPossesion == EUnitPossesionType.ai)
        {
            targetPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            // Or 
            // Set Target pos as detected obj pos in SightPerception
            cursor.transform.position = transform.position + transform.up * 5;
        }

        int index = 0;
        foreach (Transform muzzle in muzzleSockets)
        {
            bool hasImpactEffectComp;
            ImpactEffect impactEffect;
            var rot = Quaternion.Euler((Vector2Common.GetRotBetween2Pos(targetPos, muzzle.position) + aimRotOffset) * Vector3.forward);
            muzzle.rotation = rot;
            if (index!=0 && button)
            {
                Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
                Instantiate(muzzleFX, muzzle.position, muzzle.rotation);
                hasImpactEffectComp = projectilePrefab.TryGetComponent(out impactEffect);

                if (hasImpactEffectComp)
                {
                    impactEffect.instigator = _ownerUnitRefs.gameObject;
                }
            }
            index++;
        }
    }

    public override void OnAbilityActiveEnter()
    {
        base.OnAbilityActiveEnter();
        AbilityFunctionality();

    }

    void OnGUI()
    {
        if (cursor)
        {
            //cursor.SetActive(button);
            //if (button == false && isResetRotAfterAim)
            //{
            //    foreach (Transform muzzle in muzzleSockets)
            //    {
            //        muzzle.localRotation = Quaternion.identity;
            //    }
            //}
        }
    }


}
