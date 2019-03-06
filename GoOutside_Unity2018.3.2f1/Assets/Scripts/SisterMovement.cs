using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SisterMovement : MonoBehaviour
{
    private float maxSpeed = 2f;
    private float wanderSpeed;

    private NavMeshAgent navAgent;

    private Interactable interactable;

    [SerializeField]
    private float distanceToKeepFromPlayer = 1f;

    [SerializeField]
    private GameObject wanderPrefab;

    private GameObject wanderSphere;

    private bool wander = false;

    private Vector3 targetWanderPos;

    private float timer = 0f;

    [SerializeField]
    private float timeBetweenWanderings = 4f;

    private bool runTimer = false;

    

    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = GlobalReferences.instance.playerMovement.GetMaxSpeed() * 0.5f;
        wanderSpeed = maxSpeed * 0.4f;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = maxSpeed;

        interactable = GetComponent<Interactable>();
        interactable.interacting += FollowTarget;
        interactable.endInteract += SetWander;

        wanderSphere = Instantiate(wanderPrefab, transform.position, Quaternion.identity, transform);
        wanderSphere.SetActive(false);
    }


    private void SetWander()
    {
        wander = true;
        navAgent.speed = wanderSpeed;
    }

    private void Update()
    {
        if(wander)
        {
            Wander();
        }

        if(runTimer)
        {
            if(timer >= timeBetweenWanderings)
            {
                wander = true;
                runTimer = false;
                timer = 0f;
                wanderSphere.SetActive(false);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void FollowTarget()
    {
        if(navAgent.speed != maxSpeed)
        {
            wander = false;
            runTimer = false;
            timer = 0f;
            navAgent.speed = maxSpeed;
        }
        

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
            wander = false;
            runTimer = true;
            timer = 0f;
            navAgent.isStopped = true;
        }

        if (navAgent.velocity == Vector3.zero)
        {
            timer += Time.deltaTime;
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
        return (transform.position - GlobalReferences.instance.playerMovement.transform.position).sqrMagnitude;
    }

    private Vector3 FindPlayerDirection()
    {
        return (transform.position - GlobalReferences.instance.playerMovement.transform.position).normalized;
    }


    private void OnDestroy()
    {
        interactable.interacting -= FollowTarget;
        interactable.endInteract -= Wander;
    }


}
