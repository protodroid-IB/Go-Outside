using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private InputController inputController;

    private bool interacting = false;

    public delegate void Interact();
    public Interact interact;

    // Start is called before the first frame update
    void Start()
    {
        inputController = GameObject.FindWithTag("Managers").GetComponent<InputController>();
        interact += Interacting;
    }

    private void Update()
    {
        bool holding = inputController.interact1Hold(inputController.player, inputController.buttonHoldTime, ref inputController.holdTimer, ref inputController.startHoldTimer);

        if (holding)
        {
            interact.Invoke();
        }
    }

    private void Interacting()
    {
        Debug.Log("Player Interact Interacting!!!");
    }

    private void OnDestroy()
    {
        interact -= Interacting;
    }
}
