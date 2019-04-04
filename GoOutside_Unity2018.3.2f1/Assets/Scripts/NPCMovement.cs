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
    private Animator npcAnimator;

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

    [HideInInspector]
    public bool withinPlayerRadius = false;



    private void Start()
    {
        npcInteraction = GetComponent<NPCInteraction>();
        navAgent = transform.GetChild(0).GetComponent<NavMeshAgent>();
        patrolCollider = transform.GetChild(1).GetComponent<Collider>();

        homePosition = transform.GetChild(0).position;

        npcInteraction.freeze += Freeze;
        npcInteraction.unfreeze += UnFreeze;
    }



    private void Update()
    {
        if(!GlobalReferences.instance.gameManager.GetStopAction())
        {
            switch (state)
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
                    Talking();
                    break;

                case NPCState.Collided:
                    Talking();
                    break;
            }
        }
        else
        {
            navAgent.isStopped = true;
        }

        withinPlayerRadius = CheckWithinPlayerRadius();
    }

    private void Talking()
    {
        npcAnimator.SetBool("Walking", false);
        ChangeAnimationSpeed(1f);
    }

    private void Idle()
    {
        // check state changes
        StateChange_Chasing();
    }

    private void Chasing()
    {
        navAgent.isStopped = false;

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
        navAgent.isStopped = false;

        if (navAgent.speed != walkSpeed)
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
            npcAnimator.SetBool("Walking", true);
            ChangeAnimationSpeed(1.5f);
        }
    }

    private void StateChange_Idle()
    {
        if(GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(transform.GetChild(0).position, homePosition) <= 0.001)
        {
            state = NPCState.Idle;
            npcAnimator.SetBool("Walking", false);
            ChangeAnimationSpeed(1f);
        }
    }

    private void StateChange_GoingHome()
    {
        if (!GlobalReferences.instance.usefulFunctions.CheckPointInBounds(patrolCollider, GlobalReferences.instance.playerMovement.transform.position))
        {
            state = NPCState.GoingHome;
            npcAnimator.SetBool("Walking", true);
            ChangeAnimationSpeed(1f);
        }
    }


    public void SetState(NPCState inState)
    {
        state = inState;
    }

    public void Freeze()
    {
        state = NPCState.Collided;
    }

    public void UnFreeze()
    {
        state = NPCState.GoingHome;
    }


    private bool CheckWithinPlayerRadius()
    {
        float inRadius = GlobalReferences.instance.sphereMask.GetRadius();

        if (GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromPlayer(transform.position) <= (inRadius * inRadius))
        {
            return true;
        }

        return false;   
    }


    private void ChangeAnimationSpeed(float inSpeed)
    {
        npcAnimator.speed = inSpeed;
    }


    private void OnDestroy()
    {
        npcInteraction.freeze -= Freeze;
        npcInteraction.unfreeze -= UnFreeze;
    }

    
}
