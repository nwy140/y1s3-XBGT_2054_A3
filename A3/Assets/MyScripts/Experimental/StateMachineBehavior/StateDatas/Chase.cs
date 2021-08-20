using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    [CreateAssetMenu(fileName = "Chase", menuName = "AnimFSM/Chase")]
    public class Chase : StateData
    {
        public override void UpdateState(UnitStates charState, Animator animator)
        {
            base.UpdateState(charState, animator);
            var frontTarget = ((AbilityCompAssistCameraLockOn2D)_ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.LockOn)).m_targetInFrontClosest;
            if (frontTarget != null)
            {
                var targetPos = frontTarget.transform.position;
                if (Vector2.Distance(_ownerUnitRefs.transform.position, targetPos) > 5f)
                    _ownerUnitRefs.NavMeshAgentTargetDestinationPoint.transform.position = targetPos;
                else
                    _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.RegularAtkRange2D).buttonDown = true;

            }
        }
    }
}
