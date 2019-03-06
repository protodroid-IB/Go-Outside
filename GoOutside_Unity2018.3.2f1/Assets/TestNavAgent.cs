using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavAgent : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        navAgent.SetDestination(target.position);
    }
}
