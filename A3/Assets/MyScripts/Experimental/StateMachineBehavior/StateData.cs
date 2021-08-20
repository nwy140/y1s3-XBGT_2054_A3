using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    public abstract class StateData : ScriptableObject
    {
        public UnitRefs _ownerUnitRefs;
        public virtual void UpdateState(UnitStates charState, Animator animator){
            _ownerUnitRefs = animator.GetComponent<SupportCompGetOwnerUnitRef>()._ownerUnitRefs;
        }

    }
}