
using System.Collections;
using UnityEngine;

// Our Documentation: https://docs.google.com/document/d/1HGl2VWe2WO3Wv1X3tET8ymVM_4GU11_Q-2J0vo-QfyY/edit#heading=h.jpu7yib05mfv
public class AbilityCompDebugEmpty : AbilityBaseComp
{


    public override void AbilityFunctionality()
    {
        base.AbilityFunctionality();
        // Place the Logic Of How Your Ability Works here
        Debug.Log("AbilityFunctionality : " + title);
    }

    //public override IEnumerator OnAbilityStarted()
    //{
    //    base.AbilityFunctionality();

    //    return base.OnAbilityStarted();
    //}
}
