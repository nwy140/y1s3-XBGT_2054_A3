using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


// https://forum.unity.com/threads/how-to-call-animator-setbool-using-unityevent.745361/
public class AnimationEventHelper : MonoBehaviour
{
    public Animator animator;
    public string boolName;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

 
    public void SetBool(bool value)
    {
        if (boolName != null)
        {
            animator.SetBool(boolName, value);
        }
    }
    public void TurnBoolOnOff()
    {
        if (boolName != null)
        {
            animator.SetBool(boolName, !animator.GetBool(boolName));
        }
    }
    public void TurnBoolOnOffByName(string name)
    {
        if (boolName != null)
        {
            animator.SetBool(name, !animator.GetBool(name));
            Debug.Log("Set Bool: " + name + " for Animator: " + animator.name);
        }
    }
    public void SetBoolName(string newBoolname)
    {
        boolName = newBoolname;
    }

    public void SetAnimatorIsEnabled(bool isEnabled) {
        animator.enabled = isEnabled;
    }
}




#region Unused
//public List<string> boolNames;
//public int currentBoolNameIndex;
//public void TurnBoolOnOff()
//{
//    if (boolNames.Count > 0)
//    {
//        if (boolNames[currentBoolNameIndex] != null)
//        {
//            animator.SetBool(boolNames[currentBoolNameIndex], !animator.GetBool(boolNames[currentBoolNameIndex]));
//        }
//    }
//}
#endregion Unused


