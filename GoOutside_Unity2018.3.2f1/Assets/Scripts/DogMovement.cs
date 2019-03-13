using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogMovement : MonoBehaviour
{
    private GameObject dogBody;
    private NavMeshAgent navAgent;

    private DogState state = DogState.Walking;

    [SerializeField]
    private Transform waypointsParent;
    private int numWayPoints = 10;
    private GameObject[] waypoints = null;
    private Quaternion waypointTurnRot;

    private int currentWaypoint = 0;

    [Range(2f,10f)]
    private float minDistance = 3f, maxDistance = 10f;

    private void Start()
    {
        dogBody = transform.GetChild(0).gameObject;
        navAgent = dogBody.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!GlobalReferences.instance.dialogueManager.IsDialogueActive())
        {
            switch (state)
            {
                case DogState.Idle:
                    Idle();
                    break;

                case DogState.Walking:
                    Walking();
                    break;

                case DogState.Petted:
                    Petted();
                    break;
            }
        }
        else
        {
            navAgent.isStopped = true;
        }
    }

    private void Petted()
    {
        //throw new NotImplementedException();
    }

    private void Walking()
    {

        navAgent.isStopped = false;

        if(waypoints == null)
        {
            GenerateWaypoints();
        }

        // the current waypoint is between 0 and the second last waypoint
        if(currentWaypoint >= 0 && (currentWaypoint < waypoints.Length - 1))
        {
            navAgent.SetDestination(waypoints[currentWaypoint].transform.position);

            if (GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(new Vector3(dogBody.transform.position.x, 0f, dogBody.transform.position.z), new Vector3(waypoints[currentWaypoint].transform.position.x, 0f, waypoints[currentWaypoint].transform.position.z)) <= 0.001)
            {
                currentWaypoint++;
            }
        }
        // the last waypoint
        else
        {
            navAgent.SetDestination(waypoints[currentWaypoint].transform.position);

            if (GlobalReferences.instance.usefulFunctions.CalculateSqrDistanceFromTarget(new Vector3(dogBody.transform.position.x, 0f, dogBody.transform.position.z), new Vector3(waypoints[currentWaypoint].transform.position.x, 0f, waypoints[currentWaypoint].transform.position.z)) <= 0.001)
            {
                state = DogState.Idle;
            }
        }
        
    }

    private void Idle()
    {
        navAgent.isStopped = true;

        //throw new NotImplementedException();
        if(waypoints != null)
        {
            Invoke("SetState_Walking", 4f);
            ClearWaypoints();
        }
        
    }


    private void GenerateWaypoints()
    {
        waypoints = new GameObject[numWayPoints];

        for (int i = 0; i < waypoints.Length; i++)
        {
            GameObject waypointGO = new GameObject();
            waypointGO.name = "DOG WAYPOINT " + i;
            waypointGO.transform.parent = waypointsParent;

            Vector3 direction;
            float distance = UnityEngine.Random.Range(minDistance, maxDistance);

            if (i == 0)
            {
                direction = ((dogBody.transform.forward) + new Vector3(0.5f * UnityEngine.Random.Range(-1f, 1f), 0f, 0.5f * UnityEngine.Random.Range(-1f, 1f))).normalized;
                waypointGO.transform.position = dogBody.transform.position + distance * direction;           
            }
            else
            {
                direction = ((waypoints[i-1].transform.forward) + new Vector3(0.5f * UnityEngine.Random.Range(-1f, 1f), 0f, 0.5f * UnityEngine.Random.Range(-1f, 1f))).normalized;
                waypointGO.transform.position = waypoints[i-1].transform.position + distance * direction;
            }

            Collider parkCollider = GlobalReferences.instance.parkZoneCollider;
            bool inBounds = GlobalReferences.instance.usefulFunctions.CheckPointInBounds(parkCollider, waypointGO.transform.position);

            while (!inBounds)
            {
                if (waypointGO.transform.position.x < parkCollider.bounds.min.x)
                {
                    waypointGO.transform.position += new Vector3(maxDistance / 4f, 0f, 0f);
                }
                else if (waypointGO.transform.position.x > parkCollider.bounds.max.x)
                {
                    waypointGO.transform.position -= new Vector3(maxDistance / 4f, 0f, 0f);
                }

                if (waypointGO.transform.position.z < parkCollider.bounds.min.z)
                {
                    waypointGO.transform.position += new Vector3(0f, 0f, maxDistance / 4f);
                }
                else if (waypointGO.transform.position.z > parkCollider.bounds.max.z)
                {
                    waypointGO.transform.position -= new Vector3(0f, 0f, maxDistance / 4f);
                }

                inBounds = GlobalReferences.instance.usefulFunctions.CheckPointInBounds(parkCollider, waypointGO.transform.position);
            }

            NavMeshHit hit;
            NavMesh.SamplePosition(waypointGO.transform.position, out hit, maxDistance, 1);
            waypointGO.transform.position = hit.position;

            waypointTurnRot.SetFromToRotation(waypointGO.transform.forward, direction);
            waypointGO.transform.rotation *= waypointTurnRot;
            waypoints[i] = waypointGO;    
        }
    }

    private void ClearWaypoints()
    {
        currentWaypoint = 0;
        waypoints = null;

        for(int i=0; i < waypointsParent.childCount; i++)
        {
            Destroy(waypointsParent.GetChild(i).gameObject);
        }
    }


    private void SetState_Walking()
    {
        state = DogState.Walking;
    }


    public DogState GetState()
    {
        return state;
    }


    //private void OnDrawGizmos()
    //{
    //    if (waypoints != null)
    //    {
    //        for (int i = 0; i < waypoints.Length; i++)
    //        {
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawCube(waypoints[i].transform.position, Vector3.one);
    //        }
    //    }
    //}
}
