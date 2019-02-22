using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private InputController inputController;

    [SerializeField]
    [Range(1, 20)]
    private float maxSpeed;

    private Vector2 velocity;

    private void Start()
    {
        inputController = GameObject.FindWithTag("Managers").GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = inputController.move(inputController.player, transform.position);

        velocity = direction * maxSpeed * Time.deltaTime;

        transform.position += new Vector3(velocity.x, 0f, velocity.y);
    }
}
