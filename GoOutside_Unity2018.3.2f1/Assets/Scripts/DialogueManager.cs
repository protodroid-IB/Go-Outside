using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueGO;

    private Animator dialogueAnimator;

    private bool dialogueActive = false;

    private void Start()
    {
        dialogueAnimator = dialogueGO.GetComponent<Animator>();

        UpdateDialogue();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogueActive = !dialogueActive;

            
        }
    }


    private void UpdateDialogue()
    {
        if (dialogueActive)
        {
            StartDialogue("Hello, I am the biggest of dads. Welcome to dadville!");
        }
        else
        {
            EndDialogue();
        }
    }


    public bool IsDialogueActive()
    {
        return dialogueActive;
    }


    public void StartDialogue(string inDialogue)
    {
        WindowActivate(true);
    }

    public void EndDialogue()
    {
        WindowActivate(false);
    }



    private void WindowActivate(bool inActive)
    {
        if (inActive)
            dialogueAnimator.SetTrigger("Idle");
        else
            dialogueAnimator.SetTrigger("Off");
    }
}
