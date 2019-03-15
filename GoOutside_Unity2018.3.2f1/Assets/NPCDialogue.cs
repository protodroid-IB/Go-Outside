using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NPCInteraction))]
public class NPCDialogue : MonoBehaviour
{
    private NPCInteraction npcInteraction;
    private PassiveDialogueController passiveDialogue;

    [SerializeField]
    private string dialogue;

    [SerializeField]
    private bool askingQuestion = false;


    [SerializeField]
    private Choice[] choices = new Choice[4];

    // Start is called before the first frame update
    void Start()
    {
        npcInteraction = GetComponent<NPCInteraction>();
        passiveDialogue = GetComponentInChildren<PassiveDialogueController>();

        npcInteraction.SetHasSpoken(false);
        npcInteraction.dialogue += NPCSpeak;
    }

    private void NPCSpeak()
    {
        if(!askingQuestion)
            GlobalReferences.instance.dialogueManager.Speak(dialogue);
        else
            GlobalReferences.instance.dialogueManager.Speak(dialogue, choices);

        if (passiveDialogue != null)
            passiveDialogue.DeactivatePassiveDialogue();
    }

    private void OnDestroy()
    {
        npcInteraction.dialogue -= NPCSpeak;
    }
}
