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
    }

    public GameObject cursor;
    public GameObject projectilePrefab;
    public GameObject muzzleFX;
    public List<Transform> muzzleSockets;

    public float aimRotOffset;

    public override void AbilityFunctionalityPlayer()
    {
        base.AbilityFunctionalityPlayer();


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3000))
        {
            cursor.transform.position = hit.point;
            Debug.Log(hit.point);
        }

        foreach (Transform muzzle in muzzleSockets)
        {
            bool hasImpactEffectComp;
            ImpactEffect impactEffect;
            var rot = Quaternion.Euler((GetRotBetween2Pos(cursor.transform.position, muzzle.position) + aimRotOffset) * Vector3.forward);
            muzzle.rotation = rot;
            Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            Instantiate(muzzleFX, muzzle.position, muzzle.rotation);
            hasImpactEffectComp = projectilePrefab.TryGetComponent(out impactEffect);

            if (hasImpactEffectComp)
            {
                impactEffect.instigator = _ownerUnitRefs.gameObject;
            }
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

            cursor.SetActive(button);
            if (button == false)
            {
                foreach (Transform muzzle in muzzleSockets)
                {
                    muzzle.localRotation = Quaternion.identity;
                }
            }
        }
    }

    float GetRotBetween2Pos(Vector3 posA, Vector3 posB)
    {
        Vector3 dir = posA - posB;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
}
