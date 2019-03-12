using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogOwnerMovement : MonoBehaviour
{
    private enum DogOwnerState { FollowDog, Idle, ChasePlayer, Collided };

    private DogOwnerState state = DogOwnerState.FollowDog;
    private NavMeshAgent navAgent;
    private DogMovement dogMovement;
    private NPCInteraction npcInteraction;
    private Transform dogOwnerTransform;

    [SerializeField]
    private Transform dogBody;

    [SerializeField]
    [Range(5f, 20f)]
    private float minDistanceFromDog = 10f, maxDistanceFromDog = 20f;

    private float normalDistance;

    [SerializeField]
    private float minSpeed = 1f, normalSpeed = 2.5f, maxSpeed = 4f, runningSpeed = 7f;


    private float timer = 0f;

    [SerializeField]
    private float timeBeforeFollowingDog = 3f;

    [SerializeField]
    float idleRotateVariation = 12f;

    [SerializeField]
    float dogPosVariation = 12f;

    private Quaternion rotationAmount = Quaternion.identity;

    private FieldOfVisionLogic fieldOfVision;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponentInChildren<NavMeshAgent>();
        dogOwnerTransform = navAgent.transform;
        dogMovement = dogBody.GetComponentInParent<DogMovement>();
        normalDistance = (minDistanceFromDog + maxDistanceFromDog) / 2f;

        fieldOfVision = GetComponentInChildren<FieldOfVisionLogic>();
        fieldOfVision.detected += PlayerDetected;
        fieldOfVision.notDetected += PlayerNotDetected;

        npcInteraction = GetComponentInChildren<NPCInteraction>();
        npcInteraction.freeze += Freeze;
        npcInteraction.unfreeze += UnFreeze;
    }

    private void Update()
    {
        switch(state)
        {
            case DogOwnerState.FollowDog:
                FollowDog();
                break;

            case DogOwnerState.Idle:
                Idle();
                break;

            case DogOwnerState.ChasePlayer:
                ChasePlayer();
                break;

            case DogOwnerState.Collided:

                break;
        }

        //if(GlobalReferences.instance.usefulFunctions.Ca)
        //Debug.Log(state);
    }

    private void ChasePlayer()
    {
        navAgent.isStopped = false;

        navAgent.speed = runningSpeed;

        Vector3 nextPosition = GlobalReferences.instance.playerMovement.transform.position;

        float distanceFromPlayer = GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromPlayer(navAgent.transform.position);

        NavMeshHit hit;
        NavMesh.SamplePosition(nextPosition, out hit, distanceFromPlayer, 1);
        navAgent.SetDestination(hit.position);
    }

    private void FollowDog()
    {
        Vector3 nextPosition = dogBody.position + dogBody.right * (dogPosVariation * Mathf.Sin(Time.time * 0.15f));


        nextPosition = CheckInPark(nextPosition, dogPosVariation);

        NavMeshHit hit;
        NavMesh.SamplePosition(nextPosition, out hit, maxDistanceFromDog, 1);
        navAgent.SetDestination(hit.position);

        if(dogMovement.GetState() != DogState.Idle || GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(navAgent.transform.position, dogBody.position) >= (maxDistanceFromDog * maxDistanceFromDog))
        {
            navAgent.isStopped = false;

            if (GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(navAgent.transform.position, dogBody.position) >= ((normalDistance - 1.5f) * (normalDistance - 1.5f)) &&
            GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(navAgent.transform.position, dogBody.position) <= ((normalDistance + 1.5f) * (normalDistance + 1.5f)))
            {
                navAgent.speed = normalSpeed;
            }

            // the dog owner is too close to the dog
            if (GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(navAgent.transform.position, dogBody.position) <= (minDistanceFromDog * minDistanceFromDog))
            {
                navAgent.speed = minSpeed;
            }

            if (GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(navAgent.transform.position, dogBody.position) >= (maxDistanceFromDog * maxDistanceFromDog))
            {
                navAgent.speed = maxSpeed;
            }
        }
        else
        {
            if(!fieldOfVision.isDetected)
                state = DogOwnerState.Idle;   
        }

        
    }


    private void Idle()
    {
        navAgent.isStopped = true;

        if(dogMovement.GetState() != DogState.Idle)
        {
            if(timer >= timeBeforeFollowingDog)
            {
                timer = 0f;
                state = DogOwnerState.FollowDog;
            }
            else
            {
                timer += Time.deltaTime;
            } 
        }

       
        float sinRotation = idleRotateVariation * Mathf.Sin(Time.time);

        if (sinRotation < (idleRotateVariation * 0.8f) && sinRotation > -(idleRotateVariation * 0.8f))
        {
            Vector3 target = dogBody.position + dogBody.right * sinRotation;
            GlobalReferences.instance.usefulFunctions.RotateToFaceTarget(ref dogOwnerTransform, ref rotationAmount, 2f, target);
        }
    }



    private Vector3 CheckInPark(Vector3 inPosition, float distanceToMoveAwayFromBounds)
    {
        Collider parkCollider = GlobalReferences.instance.parkZoneCollider;
        bool inBounds = GlobalReferences.instance.usefulFunctions.CheckPointInBounds(parkCollider, inPosition);

        while (!inBounds)
        {
            if (inPosition.x < parkCollider.bounds.min.x)
            {
                inPosition += new Vector3(distanceToMoveAwayFromBounds, 0f, 0f);
            }
            else if (inPosition.x > parkCollider.bounds.max.x)
            {
                inPosition -= new Vector3(distanceToMoveAwayFromBounds, 0f, 0f);
            }

            if (inPosition.z < parkCollider.bounds.min.z)
            {
                inPosition += new Vector3(0f, 0f, distanceToMoveAwayFromBounds);
            }
            else if (inPosition.z > parkCollider.bounds.max.z)
            {
                inPosition -= new Vector3(0f, 0f, distanceToMoveAwayFromBounds);
            }

            inBounds = GlobalReferences.instance.usefulFunctions.CheckPointInBounds(parkCollider, navAgent.transform.position);
        }

        return inPosition;
    }


    public void Freeze()
    {
        state = DogOwnerState.Collided;
    }

    public void UnFreeze()
    {
        if(!fieldOfVision.isDetected)
        {
            if (dogMovement.GetState() == DogState.Idle)
                state = DogOwnerState.Idle;
            else
                state = DogOwnerState.FollowDog;
        }
        else
        {
                state = DogOwnerState.ChasePlayer;
        }
    }


    public void PlayerDetected()
    {
        if (!npcInteraction.GetFreezeNPC())
            state = DogOwnerState.ChasePlayer;
    }

    public void PlayerNotDetected()
    {
        Invoke("UnFreeze", 2f);
    }

    private void OnDestroy()
    {
        fieldOfVision.detected -= PlayerDetected;
        fieldOfVision.notDetected -= PlayerNotDetected;
        npcInteraction.freeze -= Freeze;
        npcInteraction.unfreeze -= UnFreeze;
    }
}
