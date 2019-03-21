using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 minPosition;

    [SerializeField]
    private Vector3 maxPosition;

    [SerializeField]
    private float mapMoveSpeed = 20f;
    [SerializeField]
    private float mapMoveAcc = 15f;
    [SerializeField]
    private float mapMoveDec = 15f;
    private float mapMoveDeadzone = 0.5f;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if(GlobalReferences.instance.mobilePhoneManager.MobileState == MobilePhoneState.Closed)
        {
            transform.position = GlobalReferences.instance.playerMovement.transform.position;
            velocity = Vector3.zero;
        }
    }

    public void MoveCamera()
    {
        Vector2 direction = GlobalReferences.instance.inputController.move(GlobalReferences.instance.inputController.player, transform.position);

        if (direction.sqrMagnitude >= (mapMoveDeadzone * mapMoveDeadzone))
        {
            velocity = Vector3.Slerp(velocity, direction.magnitude * new Vector3(direction.x, 0f, direction.y) * mapMoveSpeed * Time.deltaTime, mapMoveAcc * Time.deltaTime);
        }

        transform.position += new Vector3(velocity.x, 0f, velocity.z);

        velocity = Vector3.Slerp(velocity, Vector3.zero, mapMoveDec * Time.deltaTime);
    }

    private void LateUpdate()
    {
        Vector3 newPosition = transform.position;

        if (newPosition.x > maxPosition.x) newPosition.x = maxPosition.x;
        else if (newPosition.x < minPosition.x) newPosition.x = minPosition.x;
        if (newPosition.z > maxPosition.z) newPosition.z = maxPosition.z;
        else if (newPosition.z < minPosition.z) newPosition.z = minPosition.z;

        transform.position = newPosition;
    }
}
