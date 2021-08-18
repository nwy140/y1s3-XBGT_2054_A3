using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace experimental
{
    [CreateAssetMenu(fileName = "Idle", menuName = "AbilityData/Idle")]
    public class Idle : StateData
    {
        public override void UpdateAbility(CharState charState, Animator animator)
        {
            base.UpdateAbility(charState, animator);
            Debug.Log("Idle" + willActivateAbility_CurFrame + usageState);
        }
    }
}
 