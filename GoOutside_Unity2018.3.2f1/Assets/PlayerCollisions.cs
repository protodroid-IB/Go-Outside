using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    bool collided = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.CompareTag("PlayerCollide"))
        {
            Debug.Log("TRIGGERING?!");
            collided = true;
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.CompareTag("PlayerCollide"))
        {
            Debug.Log("TRIGGERING?!");
            collided = true;
        }

    }

    private void LateUpdate()
    {
        if (collided == true)
        {
            GlobalReferences.instance.playerMovement.velocity = Vector3.zero;
            GlobalReferences.instance.playerMovement.playerRB.velocity = Vector3.zero;
        }


        //Debug.Log(GlobalReferences.instance.playerMovement.playerRB.velocity.magnitude);


        collided = false;
    }
}
