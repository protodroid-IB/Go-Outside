using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MumDialogue : MonoBehaviour
{
    [SerializeField]
    private string[] startDialogue, idleDialogue, endDialogue;

    [SerializeField]
    private string[] diedDialogue;
    private Interactable interactable;

    private enum MumTalkState { Start, Idle, End};

    private bool gameStarted = false;

    private MumTalkState talkState = MumTalkState.Start;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    public void Talk()
    {
        switch(talkState)
        {
            case MumTalkState.Start:
                GlobalReferences.instance.dialogueManager.Speak(startDialogue);
                talkState = MumTalkState.Idle;
                break;

            case MumTalkState.Idle:
                GlobalReferences.instance.dialogueManager.Speak(idleDialogue);
                break;

            case MumTalkState.End:
                GlobalReferences.instance.dialogueManager.Speak(endDialogue);
                break;

        }
    }

    public void TalkYouDied()
    {
        GlobalReferences.instance.dialogueManager.Speak(diedDialogue);
    }

    private void Update()
    {
        if(talkState == MumTalkState.Idle)
        {
            if(!GlobalReferences.instance.dialogueManager.IsDialogueActive() && gameStarted == false)
            {
                gameStarted = true;
                GlobalReferences.instance.musicManager.StartGame();
                GlobalReferences.instance.gameManager.disableActions = false;
                GlobalReferences.instance.resourceManager.SetMentalState(1f);
                interactable.interacting += Talk;
            }
        }
    }


    private void OnDestroy()
    {
        interactable.interacting -= Talk;
    }
}
