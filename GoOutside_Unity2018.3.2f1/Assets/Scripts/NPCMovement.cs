using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NPCState state = NPCState.Idle;

    private NavMeshAgent navAgent;
    private Collider patrolCollider;
    private NPCInteraction npcInteraction;

    [SerializeField]
    [Range(1f, 10f)]
    private float runSpeed = 5f;

    [SerializeField]
    [Range(1f, 10f)]
    private float walkSpeed = 5f;

    [SerializeField]
    [Range(1f, 5f)]
    private float turnSpeed = 3f;

    private Quaternion turnRotation = Quaternion.identity;

    private Vector3 homePosition;



    private void Start()
    {
        npcInteraction = GetComponent<NPCInteraction>();
        navAgent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        patrolCollider = transform.GetChild(1).GetComponent<Collider>();

        homePosition = transform.GetChild(0).position;
    }



    private void Update()
    {
        switch(state)
        {
            case NPCState.Idle:
                Idle();
                break;

            case NPCState.Chasing:
                Chasing();
                break;

            case NPCState.GoingHome:
                GoingHome();
                break;

            case NPCState.Talking:

                break;
        }
    }

    private void Idle()
    {
        // check state changes
        StateChange_Chasing();
    }

    private void Chasing()
    {
        if (navAgent.speed != runSpeed)
        {
            navAgent.speed = runSpeed;
        }

        Vector3 playerPos = GlobalReferences.instance.playerMovement.transform.position;
        NavMeshHit hit;
        //NavMeshPath path = navAgent.path;
        NavMesh.SamplePosition(playerPos, out hit, 80f, 1);
        //NavMesh.CalculatePath(transform.GetChild(0).position, hit.position, 1, path);
        //navAgent.path = path;
        navAgent.SetDestination(hit.position);

        // check state changes
        StateChange_GoingHome();
    }

    private void GoingHome()
    {
        if(navAgent.speed != walkSpeed)
        {
            navAgent.speed = walkSpeed;
            navAgent.SetDestination(homePosition);
        }

        // check state changes
        StateChange_Idle();
        StateChange_Chasing();
    }



    private void StateChange_Chasing()
    {
        if(GlobalReferences.instance.usefulFunctions.CheckPointInBounds(patrolCollider, GlobalReferences.instance.playerMovement.transform.position))
        {
            state = NPCState.Chasing;
        }
    }

    private void StateChange_Idle()
    {
        if(GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(transform.GetChild(0).position, homePosition) <= 0.001)
        {
            state = NPCState.Idle;
        }
    }

    private void StateChange_GoingHome()
    {
        if (!GlobalReferences.instance.usefulFunctions.CheckPointInBounds(patrolCollider, GlobalReferences.instance.playerMovement.transform.position))
        {
            state = NPCState.GoingHome;
        }
    }


    public void SetState(NPCState inState)
    {
        state = inState;
    }



    // Start is called before the first frame update
    //void Start()
    //{
    //    navAgent = transform.GetChild(0).GetComponent<NavMeshAgent>();
    //    patrolArea = transform.GetChild(1).GetComponent<Collider>();
    //    npcInteraction = GetComponent<NPCInteraction>();
    //}

    //// Update is called once per frame
    //void Update()
    //{


    //    if(!npcInteraction.GetFreezeNPC())
    //    {
    //        navAgent.isStopped = false;

    //        CheckInPatrolBounds();

    //        if (target == null)
    //        {
    //            GoHome();
    //        }
    //        else
    //        {
    //            ChaseTarget();
    //        }
    //    }
    //    else
    //    {
    //        if (target != null) target = null;
    //        navAgent.isStopped = true;
    //    }
    //}

    //private void ChaseTarget()
    //{
    //    navAgent.SetDestination(target.position);
    //}

    //public void SetTarget(Transform inTarget)
    //{
    //    target = inTarget;
    //}



    //private void CheckInPatrolBounds()
    //{
    //    if (!patrolArea.bounds.Contains(navAgent.transform.position))
    //    {
    //        target = null;
    //    }
    //    else
    //    {
    //        if(patrolArea.bounds.Contains(GlobalReferences.instance.playerMovement.playerTransform.position))
    //        {
    //            if (target == null)
    //            {
    //                SetTarget(GlobalReferences.instance.playerMovement.playerTransform);
    //            }
    //        }
    //        else
    //        {
    //            target = null;
    //        }
    //    }
    //}


    //private void GoHome()
    //{
    //    if((navAgent.transform.position != patrolArea.bounds.center))
    //    {
    //        navAgent.SetDestination(patrolArea.bounds.center);
    //    }
    //}
}
