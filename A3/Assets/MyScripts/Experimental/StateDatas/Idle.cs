using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    [CreateAssetMenu(fileName = "Idle", menuName = "AnimFSM/Idle")]
    public class Idle : StateData
    {
        public override void UpdateState(UnitStates charState, Animator animator)
        {
            base.UpdateState(charState, animator);
        }
    }
}
 