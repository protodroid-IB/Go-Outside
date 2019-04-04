using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveDialogueController : MonoBehaviour
{
    private Interactable interactable;
    private Animator animator;


    private void Start()
    {
        interactable = GetComponent<Interactable>();
        animator = GetComponent<Animator>();

        interactable.beginInteract += DialogueEnter;
        interactable.endInteract += DialogueExit;
    }



    private void DialogueEnter()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Off");
        animator.SetTrigger("Idle");
    }

    private void DialogueExit()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Off");
        animator.SetTrigger("Off");
    }

    public void DeactivatePassiveDialogue()
    {
        interactable.beginInteract -= DialogueEnter;
        interactable.endInteract -= DialogueExit;
        DialogueExit();
    }

    private void OnDestroy()
    {
        interactable.beginInteract -= DialogueEnter;
        interactable.endInteract -= DialogueExit;
    }
}
