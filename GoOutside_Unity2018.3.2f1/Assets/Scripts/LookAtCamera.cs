using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private Transform npcBody;

    [SerializeField]
    private float xLock = 45f;

    [SerializeField]
    private float maxYRot = 70f;

    [SerializeField]
    [Range(0.5f, 20f)]
    private float lerpSpeed = 2f;

    [SerializeField]
    private float distanceToKeepTop = 10f, distanceToKeepBottom = 10f;


    // Update is called once per frame
    void Update()
    {
        RotateDialogue();

        MoveDialogue();

    }

    private void MoveDialogue()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(npcBody.position.x, transform.position.y, npcBody.position.z), lerpSpeed * Time.deltaTime);


        Vector3 npcDirection = GlobalReferences.instance.usefulFunctions.FindTargetDirection(Camera.main.transform.position, transform.position);
        Vector3 camDownDirection = -Camera.main.transform.up;

        float dotProductZ = Vector3.Dot(npcDirection, camDownDirection);

        if (GlobalReferences.instance.playerMovement != null)
        {
            if (GlobalReferences.instance.playerMovement.transform.position.z >= (transform.position.z))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, npcBody.localPosition.z - dotProductZ * distanceToKeepTop), lerpSpeed * Time.deltaTime);
            }

            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, npcBody.localPosition.z - dotProductZ * distanceToKeepBottom), 2f * Time.deltaTime);

            }
        }




    }

    private void RotateDialogue()
    {
        transform.LookAt(Camera.main.transform);

        float yRot = transform.rotation.eulerAngles.y;

        if (yRot >= 180f + maxYRot) yRot = 180f + maxYRot;
        else if (yRot <= 180f - maxYRot) yRot = 180f - maxYRot;


        //ensures that rotation only happens on the y-axis
        transform.rotation = Quaternion.Euler(xLock, yRot, 0f);
    }


    //    private void OnDrawGizmos()
    //    {
    //        Vector3 npcDirection = GlobalReferences.instance.usefulFunctions.FindTargetDirection(Camera.main.transform.position, transform.position);
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + npcDirection * 100f);

    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Vector3.down * 100f);
    //    }
    //}
}