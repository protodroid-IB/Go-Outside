using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCollisions : MonoBehaviour
{
    private NPCInteraction npcInteraction;

    private void Start()
    {
        npcInteraction = GetComponentInParent<NPCInteraction>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            npcInteraction.CollideWithPlayer(true);
        }
    }
}
