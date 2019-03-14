﻿using System.Collections;
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


    private void Start()
    {
        dialogueAnimator = dialogueGO.GetComponent<Animator>();
        dialogueText = dialogueGO.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = "";

        UpdateDialogue();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    dialogueActive = !dialogueActive;
        //}

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
        }
        else
        {
            EndDialogue();
        }
    }

    private void ProgressDialogue()
    {
        if (HasFinishedTalking())
        {
            dialogueActive = false;
        }
    }


    public void Speak(string inDialogue)
    {
        if(!dialogueActive)
        {
            inputDialogue = inDialogue;
            dialogueActive = true;
            GlobalReferences.instance.playerInteract.interact += ProgressDialogue;
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
}
