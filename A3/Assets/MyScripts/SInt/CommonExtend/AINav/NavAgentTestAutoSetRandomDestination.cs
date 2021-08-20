using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentTestAutoSetRandomDestination : MonoBehaviour {
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit hit;

        //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        //    {
        //        agent.destination = hit.point;
        //    }
        //}
        agent.SetDestination(AINavAgentCommon.RandomPosition(transform.position,5));
        //Wanderer.DrawGizmos();
    }

}