using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool canInteract = false;

    public delegate void Interacting();
    public Interacting interacting;
    public Interacting notInteracting;
    public Interacting beginInteract;
    public Interacting endInteract;


    public void SetCanInteract(bool inBool)
    {
        canInteract = inBool;

        if(canInteract)
        {
            GlobalReferences.instance.playerInteract.interact += InteractAction;
            GlobalReferences.instance.playerInteract.notInteracting += NotInteractionAction;
        }   
        else
        {
            GlobalReferences.instance.playerInteract.interact -= InteractAction;
            GlobalReferences.instance.playerInteract.notInteracting -= NotInteractionAction;
        }
            
    }

    public bool GetCanInteract()
    {
        return canInteract;
    }

    private void InteractAction()
    {
        if (interacting != null)
        {
            if(!GlobalReferences.instance.gameManager.GetStopAction())
            {
                interacting.Invoke();
            }
          
        }
    }

    private void NotInteractionAction()
    {
        if(notInteracting != null)
        {
            if (!GlobalReferences.instance.gameManager.GetStopAction())
            {
                notInteracting.Invoke();
            }
        }
    }
}
