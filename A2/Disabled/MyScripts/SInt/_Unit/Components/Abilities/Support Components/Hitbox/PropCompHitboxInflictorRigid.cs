using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ref: https://youtu.be/OMau8bhENp4?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU
// Ref: https://youtu.be/OMau8bhENp4?list=PLPYO_4PaVtLU-cxSTYaIGpK3PbBMDbldU&t=360

// Attach this script to the collider of your melee weapon prop 
public class PropCompHitboxInflictorRigid : MonoBehaviour, IPropComp
{
    #region Interface Related
    public UnitRefs _ownerUnitRefs;
    public UnitRefs _OwnerUnitRefs { get => _ownerUnitRefs; set => _ownerUnitRefs = value; }
    #endregion Interface Related
    public int index;
    //public bool willCallHitboxEventOnFirstCollisionPointOnly = true;
    // Contact Points
    public SupportCompHitboxInflictorRigidHandler hitboxRigidHandler;

    private void OnCollisionEnter(Collision collision)
    {

        ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoints);
        //if (willCallHitboxEventOnFirstCollisionPointOnly == true)
        //{
        //    hitboxRigidHandler.CollectCollisionPoint(contactPoints[0].point);
        //}
        //else
        //{
        foreach (ContactPoint c in contactPoints)
        {
            if (hitboxRigidHandler)
            {
                hitboxRigidHandler.CollectCollisionPoint(c.point);
            }
        }
        //}
        gameObject.SetActive(false);
    }

}
