using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCollisions : MonoBehaviour
{
    private NPCInteraction npcInteraction;

    private float timer = 0f;

    private bool canCollide = true;

    private void Start()
    {
        npcInteraction = GetComponentInParent<NPCInteraction>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (canCollide == true)
            {
                canCollide = false;
                npcInteraction.CollideWithPlayer(true);
            }
                
        }
    }

    private void Update()
    {
        if(!canCollide)
        {
            if (npcInteraction.GetHasSpoken())
            {
                if (timer >= npcInteraction.GetNoDamageTime())
                {
                    timer = 0f;
                    canCollide = true;
                }

                timer += Time.deltaTime;
            }
            
        }
        
    }
}
