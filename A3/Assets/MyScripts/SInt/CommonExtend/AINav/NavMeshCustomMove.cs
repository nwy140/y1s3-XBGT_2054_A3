using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

//ref: https://www.reddit.com/r/Unity3D/comments/4az4tc/use_navmesh_agent_for_direction_only/
public class NavMeshCustomMove : MonoBehaviour
{
    public Transform target;

    public NavMeshAgent agent;
    public UnitRefs _ownerUnitRefs;

    private void Awake()
    {
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.updatePosition = false;
        
    }
    private NavMeshPath path;
    private float elapsed = 0.0f;
    void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;
    }

    bool HasPathAndDrawPath()
    {
        // Update the way to the goal every second.
        bool result = false;
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            result = agent.CalculatePath(target.position, path);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        return result;
    }

    void Update()
    {
        if(_ownerUnitRefs.unitCompAbilityManager.eUnitPossesion == EUnitPossesionType.ai)
        {

        agent.SetDestination(target.position);
        CustomMove(agent.steeringTarget);
        }

    }

    public void CustomMove(Vector3 nextPos)
    {
        Debug.Log(nextPos);
        // custom move to destination
        var dir = nextPos - transform.position;
        _ownerUnitRefs.unitCompAbilityManager.BasicTaskAblSlot_A1.Axis = dir.normalized.x;
        _ownerUnitRefs.unitCompAbilityManager.BasicTaskAblSlot_A2.Axis = dir.normalized.y;
        //}
        
    }

}
