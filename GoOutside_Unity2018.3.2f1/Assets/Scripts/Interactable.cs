using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool canInteract = false;

    public delegate void Interacting();
    public Interacting interacting;
    public Interacting beginInteract;
    public Interacting endInteract;


    public void SetCanInteract(bool inBool)
    {
        canInteract = inBool;

        if(canInteract)
        {
            GlobalReferences.instance.playerInteract.interact += InteractAction;
            Debug.Log("Player can interact with: " + transform.name);
        }   
        else
            GlobalReferences.instance.playerInteract.interact -= InteractAction;
    }

    public bool GetCanInteract()
    {
        return canInteract;
    }

    private void InteractAction()
    {
        if (interacting != null)
        {
            interacting.Invoke();  
        }
        Debug.Log("Player interacting with: " + transform.name);
    }
}
