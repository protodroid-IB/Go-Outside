using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementController playerMovement;

    private Vector3 playerPosition = Vector3.zero;

    [SerializeField]
    private float heightAbovePlayer = 20f;

    [SerializeField]
    private float distanceFromPlayer = 30f;

    // Update is called once per frame
    void Update()
    {
        if (playerMovement != null)
            SetPlayerPosition(playerMovement.transform.position);
            
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

    private void OnValidate()
    {
        MoveCamera();
    }
}
