using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMovement : MonoBehaviour
{
    private Vector3 playerPosition = Vector3.zero;

    [SerializeField]
    private float heightAbovePlayer = 20f;

    [SerializeField]
    private float distanceFromPlayer = 30f;

    private PlayerMovementController playerMovement;


    private void Start()
    {
        playerMovement = GlobalReferences.instance.playerMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement != null)
            SetPlayerPosition(GlobalReferences.instance.playerMovement.transform.position);
        else
            SetPlayerPosition(GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>().transform.position);
        MoveCamera();
    }

    private void MoveCamera()
    {
        transform.position = new Vector3(playerPosition.x, playerPosition.y + heightAbovePlayer, playerPosition.z - distanceFromPlayer);
    }

    public void SetPlayerPosition(Vector3 inPosition)
    {
        playerPosition = inPosition;
    }
}
