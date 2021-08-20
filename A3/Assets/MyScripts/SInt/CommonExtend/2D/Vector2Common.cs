using UnityEngine;

public static class Vector2Common
{

    public static float GetRotBetween2Pos(Vector2 posA, Vector2 posB) // Redundant, Veector2.angle already does this
    {
        Vector2 dir = posA - posB;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }
    public static void LookAt2D(Transform origin, Transform target, float turnRate)
    {
        Vector3 diff = target.position - origin.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        var newRot = Quaternion.Euler(0f, 0f, rot_z - 90);
        //origin.rotation = newRot;
        origin.rotation = Quaternion.Slerp(origin.rotation, newRot, turnRate*Time.deltaTime);
        origin.eulerAngles = Vector3.forward * origin.eulerAngles.z;
    }
}
