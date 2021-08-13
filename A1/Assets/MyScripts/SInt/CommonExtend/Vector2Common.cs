using UnityEngine;

public static class Vector2Common
{

    public static float GetRotBetween2Pos(Vector2 posA, Vector2 posB) // Redundant, Veector2.angle already does this
    {
        Vector2 dir = posA - posB;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }

}
