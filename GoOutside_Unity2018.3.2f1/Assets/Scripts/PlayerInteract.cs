using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool interacting = false;

    public delegate void Interact();
    public Interact interact;

    // Start is called before the first frame update
    void Start()
    {
        interact += Interacting;
    }

    private void Update()
    {
        bool holding =
            GlobalReferences.instance.inputController.interact1Hold(GlobalReferences.instance.inputController.player, GlobalReferences.instance.inputController.buttonHoldTime, ref GlobalReferences.instance.inputController.holdTimer, ref GlobalReferences.instance.inputController.startHoldTimer) ||
            GlobalReferences.instance.inputController.interact1Press(GlobalReferences.instance.inputController.player, GlobalReferences.instance.inputController.buttonHoldTime, ref GlobalReferences.instance.inputController.holdTimer, ref GlobalReferences.instance.inputController.startHoldTimer);


        if (holding)
        {
            interact.Invoke();
        }
    }

    private void Interacting()
    {
        //Debug.Log("Player Interact Attempt!");

        // add general functionality whenever a player interacts with something
    }

    private void OnDestroy()
    {
        interact -= Interacting;
    }
}
