using UnityEngine;


public static class Vector3Common
{
    #region My Custom Library

    // Converts Vector3 to Float Speed
    // Ref: https://answers.unity.com/questions/608178/detecting-vector3-movement-speed.html
    public static float GetForwardSpeed(Vector3 velocity)
    {  
        return Vector3.Dot(velocity, Vector3.forward);
    }
    public static float GetAbsForwardSpeed(Vector3 velocity)
    {  
        return Mathf.Abs(Vector3.Dot(velocity, Vector3.forward));
    }

    public static Vector3 GetDirectionFrom2Pos(Vector3 origin, Vector3 target)
    {
        Vector3 fromPosition = origin;
        Vector3 toPosition = target;
        Vector3 direction = toPosition - fromPosition;
        return direction;
    }
    #endregion My Custom Library

    #region Vector3 EasyCharacterMovement Extension

    // Script Taken from https://gitlab.com/laizr03/ggj-2021/-/blob/master/Assets/_dependencies/_gametech/_gameengine/_codeextensions/_character&ai/Easy%20Character%20Movement/Scripts/Common/Extensions.cs

    /// <summary>
    /// Checks whether value is near to zero within a tolerance.
    /// </summary>
    public static bool isZero(this float value)
    {
        const float tolerance = 0.0000000001f;

        return Mathf.Abs(value) < tolerance;
    }

    /// <summary>
    /// Returns a copy of given vector with only X component of the vector.
    /// </summary>
    public static Vector3 onlyX(this Vector3 vector3)
    {
        vector3.y = 0.0f;
        vector3.z = 0.0f;

        return vector3;
    }

    /// <summary>
    /// Returns a copy of given vector with only Y component of the vector.
    /// </summary>
    public static Vector3 onlyY(this Vector3 vector3)
    {
        vector3.x = 0.0f;
        vector3.z = 0.0f;

        return vector3;
    }

    /// <summary>
    /// Returns a copy of given vector with only Z component of the vector.
    /// </summary>
    public static Vector3 onlyZ(this Vector3 vector3)
    {
        vector3.x = 0.0f;
        vector3.y = 0.0f;

        return vector3;
    }

    /// <summary>
    /// Returns a copy of given vector with only X and Z components of the vector.
    /// </summary>
    public static Vector3 onlyXZ(this Vector3 vector3)
    {
        vector3.y = 0.0f;

        return vector3;
    }

    /// <summary>
    /// Checks whether vector is near to zero within a tolerance.
    /// </summary>
    public static bool isZero(this Vector3 vector3)
    {
        return vector3.sqrMagnitude < 9.99999943962493E-11;
    }

    /// <summary>
    /// Checks whether vector is exceeding the magnitude within a small error tolerance.
    /// </summary>
    public static bool isExceeding(this Vector3 vector3, float magnitude)
    {
        // Allow 1% error tolerance, to account for numeric imprecision.

        const float errorTolerance = 1.01f;

        return vector3.sqrMagnitude > magnitude * magnitude * errorTolerance;
    }

    /// <summary>
    /// Returns a copy of given vector with a magnitude of 1,
    /// and outs its magnitude before normalization.
    /// 
    /// If the vector is too small to be normalized a zero vector will be returned.
    /// </summary>
    public static Vector3 normalized(this Vector3 vector3, out float magnitude)
    {
        magnitude = vector3.magnitude;
        if (magnitude > 9.99999974737875E-06)
            return vector3 / magnitude;

        magnitude = 0.0f;

        return Vector3.zero;
    }

    /// <summary>
    /// Returns a copy of given vector with its magnitude clamped to 0 and 1,
    /// and outs its magnitude before clamp.
    /// </summary>
    public static Vector3 clamped(this Vector3 vector3, out float magnitude)
    {
        magnitude = vector3.magnitude;

        return magnitude > 1.0f ? vector3 / magnitude : vector3;
    }

    /// <summary>
    /// Returns a copy of given vector with its magnitude clamped to maxLength.
    /// </summary>
    public static Vector3 clampedTo(this Vector3 vector3, float maxLength)
    {
        if (vector3.sqrMagnitude > maxLength * (double) maxLength)
            return vector3.normalized * maxLength;

        return vector3;
    }

    /// <summary>
    /// Transform a given vector to be relative to target transform.
    /// Eg: Use to perform movement relative to camera's view direction.
    /// </summary>
    public static Vector3 relativeTo(this Vector3 vector3, Transform target, bool onlyLateral = true)
    {
        var forward = target.forward;

        if (onlyLateral)
            forward = Vector3.ProjectOnPlane(forward, Vector3.up);
        
        //Ref: https://forum.unity.com/threads/simple-fix-for-look-rotation-viewing-vector-is-zero.411731/
        if (forward.sqrMagnitude > Mathf.Epsilon && forward != Vector3.zero)
        {
            return Quaternion.LookRotation(forward) * vector3;
        }
        else
        {
            return Vector3.zero;
        }
    }

    #endregion Vector3 EasyCharacterMovement Extension
}