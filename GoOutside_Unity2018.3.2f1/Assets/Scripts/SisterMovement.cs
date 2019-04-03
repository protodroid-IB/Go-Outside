using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SisterMovement : MonoBehaviour
{
    private enum SisterState {Still, Follow, Wander, Target}

    [SerializeField]
    private Animator sisterAnim;

    private SisterState sisterState = SisterState.Still;

    private NavMeshAgent navAgent;
    private Interactable interactable;

    [SerializeField]
    private float maxSpeed = 2f;
    private float wanderSpeed;

    [SerializeField]
    private float distanceToKeepFromPlayer = 1f;

    private Quaternion turnRotation = Quaternion.identity;
    [SerializeField]
    private float turnSpeed = 3f;
    private float lookAtPlayerDistance = 35f; 



    [SerializeField]
    private GameObject wanderPrefab;
    private GameObject wanderSphere;
    //private bool wander = false;
    private Vector3 targetWanderPos;


    [SerializeField]
    private float timeBetweenWanderings = 4f; 
    private float timer = 0f;
    private bool runTimer = false;

    private bool hasInteracted = false;
    



    // Start is called before the first frame update
    void Start()
    {
        // speed stuff
        maxSpeed = GlobalReferences.instance.playerMovement.GetMaxSpeed() * 0.5f;
        wanderSpeed = maxSpeed * 0.4f;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = maxSpeed;

        // interactable stuff
        interactable = GetComponent<Interactable>();
        interactable.interacting += FollowTarget;
        interactable.endInteract += SetWander;

        // wander sphere stuff
        wanderSphere = Instantiate(wanderPrefab, transform.position, Quaternion.identity, transform);
        wanderSphere.SetActive(false);
    }


    private void SetWander()
    {
        if(hasInteracted == true)
        {
            sisterState = SisterState.Wander;
            navAgent.speed = wanderSpeed;
        }

    }

    private void Update()
    {
        if(!GlobalReferences.instance.gameManager.GetStopAction())
        {
            switch (sisterState)
            {
                case SisterState.Still:
                    Still();
                    break;

                case SisterState.Wander:
                    Wander();
                    break;

                case SisterState.Target:

                    break;

                default:
                    // do nothing
                    break;
            }


            if (runTimer)
            {
                if (timer >= timeBetweenWanderings)
                {
                    sisterState = SisterState.Wander;
                    runTimer = false;
                    timer = 0f;
                    wanderSphere.SetActive(false);
                }
                else
                {
                    timer += Time.deltaTime;
                    RotateSisterToFacePlayer();
                }
            }
        }
        else
        {
            navAgent.isStopped = true;
        }
        
    }

    private void Still()
    {
        navAgent.isStopped = true;

        if(CalculateSqrDistanceFromPlayer() <= lookAtPlayerDistance * lookAtPlayerDistance)
        {
            RotateSisterToFacePlayer();
        }

        sisterAnim.SetBool("Walking", false);
    }

    private void FollowTarget()
    {
        hasInteracted = true; 

        if(navAgent.speed != maxSpeed)
        {
            sisterState = SisterState.Follow;
            runTimer = false;
            timer = 0f;
            navAgent.speed = maxSpeed;
        }

        sisterAnim.SetBool("Walking", true);


        float distanceFromPlayer = CalculateSqrDistanceFromPlayer();

        if (distanceFromPlayer <= (distanceToKeepFromPlayer * distanceToKeepFromPlayer))
        {
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
            RotateSisterToFacePlayer();
        }
        else
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(GlobalReferences.instance.playerMovement.transform.position);

            if (navAgent.velocity == Vector3.zero)
            {
                if(CalculateSqrDistanceFromPlayer() <= lookAtPlayerDistance * lookAtPlayerDistance)
                    RotateSisterToFacePlayer();
            }
        }

        
    }

    private void RotateSisterToFacePlayer()
    {
        Vector3 currentDirection = ((transform.position + transform.forward) - transform.position).normalized;
        Vector3 playerDirection = FindPlayerDirection();

        Vector3 nextDirection = Vector3.Slerp(currentDirection, playerDirection, turnSpeed * Time.deltaTime);

        turnRotation.SetFromToRotation(currentDirection, nextDirection);

        transform.rotation *= turnRotation;
    }

    private void Wander()
    {
        sisterAnim.SetBool("Walking", true);

        if (wanderSphere.activeSelf == false)
        {
            wanderSphere.SetActive(true);
            navAgent.isStopped = true;
           // targetWanderPos = Vector3.zero;
        }

        if(navAgent.isStopped == true)
        {
            //targetWanderPos = FindPositionInSphere();
            SphereCollider collider = wanderSphere.GetComponent<SphereCollider>();
            targetWanderPos = RandomPointInCollider(collider.bounds);

            NavMeshHit hit;
            NavMesh.SamplePosition(targetWanderPos, out hit, 200f, 1);

            targetWanderPos = hit.position;

            navAgent.isStopped = false;
            navAgent.SetDestination(targetWanderPos);
        }
        

        if(((transform.position - targetWanderPos).sqrMagnitude < 0.01) || (timer >= 0.75f))
        {
            sisterState = SisterState.Still;
            runTimer = true;
            timer = 0f;
            navAgent.isStopped = true;
        }

        if (navAgent.velocity == Vector3.zero)
        {
            timer += Time.deltaTime;

            if(CalculateSqrDistanceFromPlayer() <= lookAtPlayerDistance * lookAtPlayerDistance)
                RotateSisterToFacePlayer();
        }
    }


    private Vector3 RandomPointInCollider(Bounds bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            transform.position.y,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }





    private float CalculateSqrDistanceFromPlayer()
    {
        return (GlobalReferences.instance.playerMovement.transform.position - transform.position).sqrMagnitude;
    }

    private Vector3 FindPlayerDirection()
    {
        return (GlobalReferences.instance.playerMovement.transform.position - transform.position).normalized;
    }


    private void OnDestroy()
    {
        interactable.interacting -= FollowTarget;
        interactable.endInteract -= SetWander;
    }


}
