using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Collider patrolArea;

    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        patrolArea = transform.GetChild(1).GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInPatrolBounds();

        if (target == null)
        {
            GoHome();
        }
        else
        {
            ChaseTarget();
        }
    }

    private void ChaseTarget()
    {
        navAgent.SetDestination(target.position);
    }

    public void SetTarget(Transform inTarget)
    {
        target = inTarget;
    }



    private void CheckInPatrolBounds()
    {
        if (!patrolArea.bounds.Contains(navAgent.transform.position))
        {
            target = null;
        }
        else
        {
            if(patrolArea.bounds.Contains(PlayerMovementController.instance.playerTransform.position))
            {
                if (target == null)
                {
                    SetTarget(PlayerMovementController.instance.playerTransform);
                }
            }
            else
            {
                target = null;
            }
        }
    }


    private void GoHome()
    {
        if((navAgent.transform.position != patrolArea.bounds.center))
        {
            navAgent.SetDestination(patrolArea.bounds.center);
        }
        
    }
}
