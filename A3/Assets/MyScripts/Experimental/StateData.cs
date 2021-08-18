using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    public abstract class StateData : ScriptableObject
    {
        public virtual void UpdateState(UnitStates charState, Animator animator){

        }

    }
}