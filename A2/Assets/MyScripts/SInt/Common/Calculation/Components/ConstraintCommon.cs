// Static Class for Constraint components, // i.e parent constraint

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public static class ConstraintCommon
{
    // Ref: https://www.programmersought.com/article/43906742531/
    public static void ResetCSrcToSingleTargetParent(ParentConstraint parentConstraint, Transform targetTrans)
    {
        if (parentConstraint != null)
        {
            //The parameters when the target object weight is 0
            parentConstraint.translationAtRest = Vector3.zero;
            parentConstraint.rotationAtRest = Vector3.zero;

            //Freeze the axis, automatically associate the target object
            parentConstraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
            parentConstraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
        }

        //Add target object
        ConstraintSource constraintSource = new ConstraintSource() {sourceTransform = targetTrans, weight = 1};
        // for (int i = 0; i < parentConstraint.sourceCount; i++)
        // {
        //     parentConstraint.RemoveSource(i);
        // }
        // parentConstraint.AddSource(constraintSource);
        parentConstraint.SetSources(new List<ConstraintSource>() {constraintSource});
        //Set the relative offset
        parentConstraint.SetTranslationOffset(0, Vector3.zero);
        parentConstraint.SetRotationOffset(0, Vector3.zero);
    }
    public static void SetFirstCSourceSnapTargetParent(ParentConstraint parentConstraint, Transform targetTrans)
    {
        if (parentConstraint != null)
        {
            //The parameters when the target object weight is 0
            parentConstraint.translationAtRest = Vector3.zero;
            parentConstraint.rotationAtRest = Vector3.zero;

            //Freeze the axis, automatically associate the target object
            parentConstraint.translationAxis = Axis.X | Axis.Y | Axis.Z;
            parentConstraint.rotationAxis = Axis.X | Axis.Y | Axis.Z;
        }
        //Add target object
        ConstraintSource constraintSource = new ConstraintSource() {sourceTransform = targetTrans, weight = 1};
        if (parentConstraint.sourceCount == 0)
        {
            parentConstraint.AddSource(constraintSource);
        }
        else
        {
            parentConstraint.SetSource(0,constraintSource);
        }
        // parentConstraint.SetSources(new List<ConstraintSource>() {constraintSource});
        //Set the relative offset
        parentConstraint.SetTranslationOffset(0, Vector3.zero);
        parentConstraint.SetRotationOffset(0, Vector3.zero);
    }

    #region Redundant
    public static ConstraintSource NewCSourceParent(Transform srcTransform, float weight)
    {
        ConstraintSource cSourceTarget = new ConstraintSource();
        cSourceTarget.sourceTransform = srcTransform;
        cSourceTarget.weight = weight;
        return cSourceTarget;
    }

    #endregion Redundant
}