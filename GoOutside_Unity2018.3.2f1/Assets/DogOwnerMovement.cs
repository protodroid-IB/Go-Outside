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

    [SerializeField]
    private Transform dogBody;

    [SerializeField]
    [Range(5f, 20f)]
    private float minDistanceFromDog = 10f, maxDistanceFromDog = 20f;

    private float normalDistance;

    [SerializeField]
    private float minSpeed = 1f, normalSpeed = 2.5f, maxSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponentInChildren<NavMeshAgent>();
        dogMovement = dogBody.GetComponentInParent<DogMovement>();
        normalDistance = (minDistanceFromDog + maxDistanceFromDog) / 2f;
    }

    private void Update()
    {
        switch(state)
        {
            case DogOwnerState.FollowDog:
                FollowDog();
                break;

            case DogOwnerState.Idle:

                break;

            case DogOwnerState.ChasePlayer:

                break;

            case DogOwnerState.Collided:

                break;
        }
    }

    private void FollowDog()
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(dogBody.position, out hit, maxDistanceFromDog, 1);
        navAgent.SetDestination(hit.position);

        if(dogMovement.GetState() != DogState.Idle)
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
            navAgent.isStopped = true;
        }

        
    }
}
