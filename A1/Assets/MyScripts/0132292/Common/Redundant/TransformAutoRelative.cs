using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Redundant, Use Unity's own Built In parent cosntraint instead
public class TransformAutoRelative : MonoBehaviour
{
    public Transform relativeObj;

    public bool isSnapPosToObj;

    // Fake Parent Relative Transform
    public bool isRelativePos;
    public bool isRelativeRot;
    public bool isRelativeScale;

    private void Update()
    {
        if (isSnapPosToObj)
            transform.position = relativeObj.position;
        if (isRelativePos)
            transform.position = transform.position.relativeTo(relativeObj);
        if (isRelativeRot)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.relativeTo(relativeObj));
        if (isRelativeScale)
            transform.localScale = transform.localScale.relativeTo(relativeObj);
        
    }
}