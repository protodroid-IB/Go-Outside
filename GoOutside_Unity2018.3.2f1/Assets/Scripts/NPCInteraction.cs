using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private Rigidbody npcRB;

    private bool freezeNPC = false;

    [SerializeField]
    private float freezeTimeOnCollision = 5f;
    private float freezeTimer = 0f;

    [SerializeField]
    [Header("The percentage of mental state to be deducted from player")]
    [Range(0.00f, 0.10f)]
    private float damage = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        npcRB = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(freezeNPC)
        {
            npcRB.velocity = Vector3.zero;

            if(freezeTimer >= freezeTimeOnCollision)
            {
                freezeNPC = false;
                npcRB.constraints &= ~RigidbodyConstraints.FreezePosition;
                freezeTimer = 0f;
            }

            freezeTimer += Time.deltaTime;
        }
    }


    public bool GetFreezeNPC()
    {
        return freezeNPC;
    }

    public void CollideWithPlayer(bool inFreeze)
    {
        if(freezeNPC == false)
        {
            freezeNPC = inFreeze;
            npcRB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
