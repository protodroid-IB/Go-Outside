using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SisterMovement : MonoBehaviour
{
    private float maxSpeed = 2f;

    private NavMeshAgent navAgent;

    private Interactable interactable;

    [SerializeField]
    private float distanceToKeepFromPlayer = 1f;

    [SerializeField]
    private GameObject wanderPrefab;

    private GameObject wanderSphere;

    private bool foundWanderPosition = false;

    private Vector3 targetWanderPos;

    

    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = GlobalReferences.instance.playerMovement.GetMaxSpeed();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = maxSpeed * 0.5f;

        interactable = GetComponent<Interactable>();
        interactable.interacting += FollowTarget;
        interactable.endInteract += Wander;

        wanderSphere = Instantiate(wanderPrefab, transform.position, Quaternion.identity, transform);
        wanderSphere.SetActive(false);
    }


    private void FollowTarget()
    {
        float distanceFromPlayer = CalculateSqrDistanceFromPlayer();

        if (distanceFromPlayer < (distanceToKeepFromPlayer * distanceToKeepFromPlayer))
        {
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
        }
        else
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(GlobalReferences.instance.playerMovement.transform.position);
        }
    }

    private void Wander()
    {
        if(wanderSphere.activeSelf == false)
        {
            wanderSphere.SetActive(true);
            navAgent.isStopped = false;
        }

        if(navAgent.isStopped == false)
        {
            targetWanderPos = FindPositionInSphere();
            navAgent.isStopped = false;
            navAgent.SetDestination(targetWanderPos);
            Debug.Log("FOUND NEW TARGET!");
        }

        if(navAgent.destination == targetWanderPos)
        {
            wanderSphere.SetActive(false);
        }   
    }

    private Vector3 FindPositionInSphere()
    {
        SphereCollider collider = wanderSphere.GetComponent<SphereCollider>();

        float distance = UnityEngine.Random.Range(0.3f, 0.8f) * collider.radius;

        Vector3 playerDirection = FindPlayerDirection();

        Vector2 directionX_MinMax = FindDirectionAwayFromPlayer(playerDirection.x);
        Vector2 directionZ_MinMax = FindDirectionAwayFromPlayer(playerDirection.z);

        Vector3 direction = new Vector3(UnityEngine.Random.Range(directionX_MinMax.x, directionX_MinMax.y), 0f, UnityEngine.Random.Range(directionX_MinMax.x, directionX_MinMax.y));

        Debug.Log("PLAYER DIRECTION: " + playerDirection + "\tTARGET DIRECTION: " + direction);

        return direction;
    }





    private float CalculateSqrDistanceFromPlayer()
    {
        return (transform.position - GlobalReferences.instance.playerMovement.transform.position).sqrMagnitude;
    }

    private Vector3 FindPlayerDirection()
    {
        return (transform.position - GlobalReferences.instance.playerMovement.transform.position).normalized;
    }







    private Vector2 FindDirectionAwayFromPlayer(float inDirComponent)
    {
        Vector2 minMax = Vector2.zero;

        float num1 = inDirComponent - 0.3f;
        float num2 = inDirComponent + 0.3f;

        if(num1 < 0)
        {
            num1 = 1 + (num1);
        }
        else if(num1 > 1)
        {
            num1 = (num1 - 1);
        }

        if (num2 < 0)
        {
            num2 = 1 + (num2);
        }
        else if (num2 > 1)
        {
            num2 = (num2 - 1);
        }

        if(num1 < num2)
        {
            minMax.x = num1;
            minMax.y = num2;
        }
        else
        {
            minMax.x = num2;
            minMax.y = num1;
        }


        if(inDirComponent > minMax.x && inDirComponent < minMax.y)
        {
            int coinFlip = UnityEngine.Random.Range(0, 2);

            if(coinFlip == 0)
            {
                minMax.y = minMax.x;
                minMax.x = 0f;
            }
            else
            {
                minMax.x = minMax.y;
                minMax.y = 1f;
            }
        }

        return minMax;


    }

    private void OnDestroy()
    {
        interactable.interacting -= FollowTarget;
        interactable.endInteract -= Wander;
    }


}
