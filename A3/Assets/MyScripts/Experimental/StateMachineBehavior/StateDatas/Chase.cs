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
            var LockOnComp = ((AbilityCompAssistCameraLockOn2D)_ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.LockOn));
            if (LockOnComp.m_CandidateTargets.Count > 0)
            {
                var frontTarget =LockOnComp.m_CandidateTargets[0];
                if (frontTarget != null)
                {
                    var targetPos = frontTarget.transform.position;
                    if (Vector2.Distance(_ownerUnitRefs.transform.position, targetPos) > 20f)
                        _ownerUnitRefs.NavMeshAgentTargetDestinationPoint.transform.position = targetPos;
                    else
                        _ownerUnitRefs.unitCompAbilityManager.GetActiveAbilityCompByEnum(EAbilityTechniques.RegularAtkAim).buttonDown = true;

                }

            }
        }
    }
}
