using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMovement : MonoBehaviour
{
    private PlayerMovementController playerMovementController;
    private Vector3 playerPosition = Vector3.zero;

    [SerializeField]
    private float heightAbovePlayer = 20f;

    [SerializeField]
    private float distanceFromPlayer = 30f;


    private void Start()
    {
        playerMovementController = GameObject.FindWithTag("Player").GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovementController != null)
            SetPlayerPosition(playerMovementController.transform.position);
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
