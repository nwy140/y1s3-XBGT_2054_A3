using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public static class TransformCommon
{
    #region Sibling Index

    #endregion Sibling Index

    #region Constraint


    public static Quaternion RotateTowards(Transform selfTransform,Transform targetTransform, float rotSpeed =1f)
    {
        Vector3 targetDirection = (targetTransform.position - selfTransform.position).normalized;
        var targetRotation = Quaternion.LookRotation(targetDirection);
        return  Quaternion.RotateTowards(selfTransform.rotation, targetRotation , Time.deltaTime * rotSpeed);
    }

    #endregion Constraint
}