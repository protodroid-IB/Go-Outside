using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCInteraction : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private NPCCollisions npcCollisions;

    public delegate void FreezeUnfreeze();
    public FreezeUnfreeze unfreeze;
    public FreezeUnfreeze freeze;

    //private NPCMovement npcMovement;

    private bool freezeNPC = false;

    [SerializeField]
    private float freezeTimeOnCollision = 5f;
    private float freezeTimer = 0f;

    [SerializeField]
    private float noDamageTime = 2f;

    [SerializeField]
    [Header("The percentage of mental state to be deducted from player")]
    [Range(0.00f, 0.10f)]
    private float damage = 0.02f;

    private Quaternion turnRotation = Quaternion.identity;
    private Transform npcTransform;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponentInChildren<NavMeshAgent>();
        //npcMovement = GetComponent<NPCMovement>();
        npcTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(freezeNPC)
        {
            if(freezeTimer >= freezeTimeOnCollision)
            {
                freezeNPC = false;
                navAgent.isStopped = false;
                freezeTimer = 0f;
                if(unfreeze != null)
                    unfreeze.Invoke();
                //npcMovement.SetState(NPCState.GoingHome);
            }

            freezeTimer += Time.deltaTime;

            
            GlobalReferences.instance.usefulFunctions.RotateToFacePlayer(ref npcTransform, ref turnRotation, 3f);
        }
    }


    public bool GetFreezeNPC()
    {
        return freezeNPC;
    }

    public float GetNoDamageTime()
    {
        return noDamageTime;
    }

    public void CollideWithPlayer(bool inFreeze)
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        freezeNPC = inFreeze;
        GlobalReferences.instance.resourceManager.UpdateMentalState(-damage, true);
        if(freeze != null)
            freeze.Invoke();
        //npcMovement.SetState(NPCState.Collided);
    }
}
