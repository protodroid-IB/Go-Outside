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

    [HideInInspector]
    public NavMeshAgent navAgent;
    private Interactable interactable;

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


    [SerializeField]
    public GameObject interactSphere, dropZoneSphereSchool, dropZoneSphereHome;

    private bool wandering = true, still = true;

    private bool inTriggerArea = false;

    private float followSpeed;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float slowDownSpeedRatio = 0.5f;

    [SerializeField]
    private float followAcc = 0.5f, followDec = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // speed stuff
        maxSpeed = GlobalReferences.instance.playerMovement.GetMaxRunSpeed() * 2f;
        wanderSpeed = maxSpeed * 0.1f;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = maxSpeed;

        // interactable stuff
        interactable = GetComponent<Interactable>();
        interactable.beginInteract += InTriggerArea;
        interactable.interacting += FollowTarget;
        interactable.endInteract += SetWander;

        // wander sphere stuff
        wanderSphere = Instantiate(wanderPrefab, transform.position, Quaternion.identity, transform);
        wanderSphere.SetActive(false);

        dropZoneSphereHome.SetActive(false);
    }

    private void InTriggerArea()
    {
        inTriggerArea = true;
    }

    private void SetWander()
    {
        if(hasInteracted == true)
        {
            sisterState = SisterState.Wander;
            navAgent.speed = wanderSpeed;
            wandering = true;
        }

        inTriggerArea = false;

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

        Debug.Log(sisterState);
        
    }

    private void Still()
    {
        navAgent.isStopped = true;

        still = true;

        if (CalculateSqrDistanceFromPlayer() <= lookAtPlayerDistance * lookAtPlayerDistance)
        {
            RotateSisterToFacePlayer();
        }

        //sisterAnim.SetBool("Walking", false);
        sisterAnim.SetFloat("Speed", 0.0f);
    }

    private void FollowTarget()
    {
        hasInteracted = true;

        if (wandering == true || still == true)
        {
            wandering = false;
            still = false;
            followSpeed = GlobalReferences.instance.playerMovement.GetCurrentSpeed();
            sisterState = SisterState.Follow;
            runTimer = false;
            timer = 0f;
            navAgent.isStopped = false;
        }

        //float distanceFromPlayer = CalculateSqrDistanceFromPlayer();

        //if (distanceFromPlayer <= (distanceToKeepFromPlayer * distanceToKeepFromPlayer))
        //{
        //    navAgent.isStopped = false;
        //    followSpeed = Mathf.Lerp(followSpeed, GlobalReferences.instance.playerMovement.GetCurrentSpeed() * 0.65f, 2f);
        //    RotateSisterToFacePlayer();
        //    navAgent.SetDestination(GlobalReferences.instance.playerMovement.transform.position);
        //    //sisterAnim.SetFloat("Speed", 0.0f);
        //}
        //else
        //{
        //    //navAgent.speed = Mathf.Lerp(navAgent.speed, GlobalReferences.instance.playerMovement.GetCurrentSpeed(), 0.1f);
        //    navAgent.isStopped = false;
        //    navAgent.SetDestination(GlobalReferences.instance.playerMovement.transform.position);
        //    followSpeed = Mathf.Lerp(followSpeed, GlobalReferences.instance.playerMovement.GetCurrentSpeed(), 2f);

        //    //if (navAgent.velocity == Vector3.zero)
        //    //{
        //    //    if(CalculateSqrDistanceFromPlayer() <= lookAtPlayerDistance * lookAtPlayerDistance)
        //    //        RotateSisterToFacePlayer();
        //    //}
        //}

        navAgent.SetDestination(GlobalReferences.instance.playerMovement.transform.position);

        float distanceFromPlayer = CalculateDistanceFromPlayer();

        if (distanceFromPlayer <= distanceToKeepFromPlayer)
        {
            followSpeed = Mathf.Lerp(followSpeed, GlobalReferences.instance.playerMovement.GetCurrentSpeed() * slowDownSpeedRatio, followDec);
        }
        else
        {
            if (followSpeed > GlobalReferences.instance.playerMovement.GetCurrentSpeed() * slowDownSpeedRatio)
            {
                followSpeed = Mathf.Lerp(followSpeed, GlobalReferences.instance.playerMovement.GetCurrentSpeed() * slowDownSpeedRatio, followDec);
            }
            else
            {
                followSpeed = Mathf.Lerp(followSpeed, GlobalReferences.instance.playerMovement.GetCurrentSpeed(), followAcc);
            }
        }

        navAgent.speed = followSpeed;
        sisterAnim.SetFloat("Speed", followSpeed / maxSpeed);
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
        //sisterAnim.SetBool("Walking", true);
       

        if (wanderSphere.activeSelf == false)
        {
            wanderSphere.SetActive(true);
            navAgent.isStopped = true;
            // targetWanderPos = Vector3.zero;
        }

        sisterAnim.SetFloat("Speed", navAgent.speed / maxSpeed);

        if (navAgent.isStopped == true)
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


        if (inTriggerArea == false)
        {
            if (((transform.position - targetWanderPos).sqrMagnitude < 0.01) || (timer >= 0.75f))
            {
                sisterState = SisterState.Still;
                runTimer = true;
                timer = 0f;
                navAgent.isStopped = true;
            }
        }
        else
        {
            sisterState = SisterState.Follow;
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





    private float CalculateDistanceFromPlayer()
    {
        return (GlobalReferences.instance.playerMovement.transform.position - transform.position).magnitude;
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


    public void WalkIntoSchool(Vector3 position)
    {
        sisterState = SisterState.Target;
        interactable.interacting -= FollowTarget;
        interactable.endInteract -= SetWander;
        interactSphere.SetActive(false);
        dropZoneSphereSchool.SetActive(false);
        sisterAnim.SetFloat("Speed", 0.0f);
    }

    public void DisableSchoolDropOff()
    {
        dropZoneSphereSchool.SetActive(false);
    }

    public void EnableInteraction()
    {
        if(interactSphere.activeInHierarchy == false)
        {
            interactable.interacting += FollowTarget;
            interactable.endInteract += SetWander;
            interactSphere.SetActive(true);
            hasInteracted = false;
            sisterState = SisterState.Follow;
        }
        
    }

    public void EnableHomeDropOff()
    {
        dropZoneSphereHome.SetActive(true);
    }

    public void WalkIntoHome(Vector3 position)
    {
        sisterState = SisterState.Still;
        interactable.interacting -= FollowTarget;
        interactable.endInteract -= SetWander;
        interactSphere.SetActive(false);
        dropZoneSphereHome.SetActive(false);
        sisterAnim.SetFloat("Speed", 0.0f);
        Debug.Log("YEA HAN");
    }


}
