using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueGO;

    private Animator dialogueAnimator;
    private TextMeshProUGUI dialogueText;

    private bool dialogueActive = false;
    private string inputDialogue = "";

    private bool canStartTalking = true;

    private string currentDialogue = "";
    private int currentCharacter = 0;

    private float dialogueTextTimer = 0f;

    [SerializeField]
    private float dialogueTextSpeed = 2f;

    private bool askingQuestion = false;

    private Choice[] choices;

    private List<string> inputDialogueSequence = new List<string>();


    private void Start()
    {
        dialogueGO.SetActive(true);
        dialogueAnimator = dialogueGO.GetComponent<Animator>();
        dialogueText = dialogueGO.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "";

        UpdateDialogue();
    }

    private void Update()
    {
        UpdateDialogue();
    }


    private void UpdateDialogue()
    {
        if (dialogueActive)
        {
            if (canStartTalking)
            {
                StartDialogue(inputDialogue);
            }
            else
            {
                if(askingQuestion)
                {
                    GlobalReferences.instance.choiceManager.ActivateChoices(choices);
                    askingQuestion = false;
                }
            }
        }
        else
        {
            EndDialogue();
        }
    }

    private void ProgressDialogue()
    {
        if (HasFinishedTalking() && inputDialogueSequence.Count <= 0)
        {
            dialogueActive = false;
        }
        else
        {
            if(HasFinishedTalking())
            {
                dialogueText.text = "";
                currentDialogue = "";
                currentCharacter = 0;
                dialogueTextTimer = 0f;
                inputDialogue = inputDialogueSequence[0];
                inputDialogueSequence.RemoveAt(0);
                canStartTalking = true;
            }
            
        }

    }


    public void Speak(string inDialogue)
    {
        if(!dialogueActive)
        {
            inputDialogueSequence.Clear();
            inputDialogue = inDialogue;
            dialogueActive = true;
            askingQuestion = false;
            GlobalReferences.instance.playerInteract.interact += ProgressDialogue;
        }
    }

    public void Speak(string[] inDialogue)
    {
        if (!dialogueActive)
        {
            inputDialogueSequence.Clear();
            for(int i=1; i < inDialogue.Length; i++)
            {
                inputDialogueSequence.Add(inDialogue[i]);
            }

            inputDialogue = inDialogue[0];
            dialogueActive = true;
            askingQuestion = false;
            GlobalReferences.instance.playerInteract.interact += ProgressDialogue;
        }
    }

    public void Speak(string inDialogue, Choice[] inChoices)
    {
        if (!dialogueActive)
        {
            inputDialogueSequence.Clear();
            inputDialogue = inDialogue;
            choices = inChoices;

            dialogueActive = true;
            askingQuestion = true;
        }
    }



    public bool IsDialogueActive()
    {
        return dialogueActive;
    }

    public bool HasFinishedTalking()
    {
        return !canStartTalking;
    }


    private void StartDialogue(string inDialogue)
    {
        //if(GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(dialogueAnimator, "DialogueOff") || GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(dialogueAnimator, "Dialogue_ExitIdle"))
        WindowActivate(true);

        WriteDialogue(inDialogue);
    }

    private void WriteDialogue(string inDialogue)
    {
        bool idleAnimPlaying = GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(dialogueAnimator, "Dialogue_Idle");

        if (idleAnimPlaying)
        {
            dialogueText.text = currentDialogue;

            if (currentDialogue.Length < inDialogue.Length)
            {
                dialogueTextTimer += dialogueTextSpeed * Time.deltaTime;
                currentCharacter = (int)dialogueTextTimer;

                if (currentCharacter > inDialogue.Length) currentCharacter = inDialogue.Length;

                currentDialogue = inDialogue.Substring(0, currentCharacter);
            }
            else
            {
                canStartTalking = false;
            }
        }
    }



    private void EndDialogue()
    {
        WindowActivate(false);

        bool offAnimPlaying = GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(dialogueAnimator, "DialogueOff");

        if (offAnimPlaying)
        {
            if(!canStartTalking)
                GlobalReferences.instance.playerInteract.interact -= ProgressDialogue;

            dialogueText.text = "";
            currentDialogue = "";
            currentCharacter = 0;
            dialogueTextTimer = 0f;
            canStartTalking = true;
        }
    }



    private void WindowActivate(bool inActive)
    {
        if (inActive)
        {
            dialogueAnimator.ResetTrigger("Idle");
            dialogueAnimator.ResetTrigger("Off");
            dialogueAnimator.SetTrigger("Idle");
        }
        else
        {
            dialogueAnimator.ResetTrigger("Idle");
            dialogueAnimator.ResetTrigger("Off");
            dialogueAnimator.SetTrigger("Off");
        }
    }

    public void SetDialogueActive(bool inActive)
    {
        dialogueActive = inActive;
    }
}
