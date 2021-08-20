using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class AINavAgentCommon
{
    public static Vector3 RandomPosition(Vector3 origin, float radius)
    {
        var randDirection = Random.insideUnitSphere * radius;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, radius, -1);
        return navHit.position;
    }


}
