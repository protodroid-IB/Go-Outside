using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    private Interactable interactable;

    private void Start()
    {
        interactable = GetComponentInParent<Interactable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerModel"))
        {
            interactable.SetCanInteract(true);
            if(interactable.beginInteract != null)
            {
                interactable.beginInteract.Invoke();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            interactable.SetCanInteract(false);
            if (interactable.endInteract != null)
            {
                interactable.endInteract.Invoke();
            }
        }
    }
}
