using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    [HideInInspector]
    public Transform playerTransform;

    [SerializeField]
    [Range(0.5f, 20)]
    private float maxSpeed = 14f;

    [SerializeField]
    private float moveDeadZone = 0.5f;

    [SerializeField]
    [Range(0.25f, 10)]
    private float moveDeceleration = 1.0f;

    [SerializeField]
    [Range(0.25f, 30)]
    private float moveAcceleration = 1.0f;

    [HideInInspector]
    public Vector3 velocity = Vector3.zero;

    [SerializeField]
    [Range(0.25f, 10)]
    private float turnSpeed = 1.0f;
    private Quaternion turnRotation = new Quaternion();

    [HideInInspector]
    public Rigidbody playerRB;

    private Vector2 direction;
    private bool useGravity = true;
    private float floorHeight = 0f;

    private bool collided = false;


    private void Awake()
    {
        playerTransform = transform;
    }


    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalReferences.instance.gameManager.GetStopAction())
        {
            direction = GlobalReferences.instance.inputController.move(GlobalReferences.instance.inputController.player, transform.position);

            Turn(direction);
            Move(direction);
        }
        else
        {
            transform.position += new Vector3(velocity.x, 0f, velocity.z);
            velocity = Vector3.Slerp(velocity, Vector3.zero, moveDeceleration * Time.deltaTime);
        }


        
        
        
    }


    private void Turn(Vector2 inDirection)
    {
        Vector3 currentDirection = ((transform.position + transform.forward) - transform.position).normalized;
        Vector3 inputDirection = new Vector3(inDirection.x, currentDirection.y, inDirection.y);

        Vector3 nextDirection = Vector3.Slerp(currentDirection, inputDirection, turnSpeed * Time.deltaTime);

        turnRotation.SetFromToRotation(currentDirection, nextDirection);

        transform.rotation *= turnRotation;
    }

    private void Move(Vector2 inDirection)
    {
        if(inDirection.sqrMagnitude >= (moveDeadZone * moveDeadZone))
        {
            velocity = Vector3.Slerp(velocity, inDirection.magnitude * transform.forward * maxSpeed * Time.deltaTime, moveAcceleration * Time.deltaTime);
        }

        transform.position += new Vector3(velocity.x, 0f, velocity.z);

        velocity = Vector3.Slerp(velocity, Vector3.zero, moveDeceleration * Time.deltaTime);
    }

    protected void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        if (collided == true)
        {
            playerRB.velocity = Vector3.zero;
            velocity = Vector3.zero;
            playerRB.constraints |= RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            playerRB.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }


        collided = false;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerCollide"))
        {
            collided = true;
        }

    }
}
