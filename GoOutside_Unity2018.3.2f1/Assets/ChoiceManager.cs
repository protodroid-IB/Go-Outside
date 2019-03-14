using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceManager : MonoBehaviour
{

    [SerializeField]
    private ChoiceUIReferences choiceReferences;

    [SerializeField]
    [Range(5, 20)]
    private int timeToMakeChoice = 10;
    private float timer = 0f;
    private bool startTimer = false;

    [SerializeField]
    private int numSecondsBetweenChoiceSwaps = 2;

    private int currentChoiceSwap = 1;
    private int totalNumChoiceSwaps;
    private int swapNum;

    private struct ChoiceButton
    {
        public int choiceNum;
        public int buttonType;
    }

    private ChoiceButton[] choiceButtons = new ChoiceButton[4];

    private void Start()
    {
        choiceReferences.gameObject.SetActive(true);
        choiceReferences.activate += StartChoiceSequence;
        choiceReferences.deactivate += DeactivateChoices;
        totalNumChoiceSwaps = timeToMakeChoice / numSecondsBetweenChoiceSwaps;
        swapNum = currentChoiceSwap * numSecondsBetweenChoiceSwaps;
    }

    private void Update()
    {
        if(startTimer)
        {
            float fillAmount = 1f - Mathf.Clamp01(timer / (float)timeToMakeChoice);
            Vector3 fillVector = new Vector3(fillAmount, choiceReferences.TimerFill.localScale.y, choiceReferences.TimerFill.localScale.z);
            choiceReferences.TimerFill.localScale = Vector3.Lerp(choiceReferences.TimerFill.localScale, fillVector, 60f * Time.deltaTime);

            if (timer > (float)timeToMakeChoice)
            {
                EndChoiceSequence();
            }


            if(timer >= 1f)
            {
                int theTime = (int)timer;

                if (theTime % swapNum == 0)
                {
                    currentChoiceSwap++;
                    swapNum = currentChoiceSwap * numSecondsBetweenChoiceSwaps;
                    SwapButtons();
                }
            }
            

            timer += Time.deltaTime;
        }
    }

    private void SwapButtons()
    {
        // CONTROL BACKEND AND FRONT END HERE!!!
        Debug.Log("Swap buttons!");
    }

    public void ActivateChoices(string[] inChoices)
    {
        choiceReferences.gameObject.SetActive(true);

        if (choiceReferences.animator != null)
            choiceReferences.animator.SetTrigger("Idle");
        else
        {
            choiceReferences.animator = choiceReferences.transform.GetComponent<Animator>();
            choiceReferences.animator.SetTrigger("Idle");
        }

        for(int i=0; i < choiceReferences.Choices.Length; i++)
        {
            choiceReferences.Choices[i].text = inChoices[i];
        }

    }



    public void StartChoiceSequence()
    {
        startTimer = true;
    }


    private void EndChoiceSequence()
    {
        startTimer = false;
        timer = 0f;
        choiceReferences.animator.SetTrigger("Off");
    }

    public void DeactivateChoices()
    {
        choiceReferences.TimerFill.localScale = new Vector3(1f, choiceReferences.TimerFill.localScale.y, choiceReferences.TimerFill.localScale.z);
        choiceReferences.gameObject.SetActive(false);
        GlobalReferences.instance.dialogueManager.SetDialogueActive(false);
    }











    private void OnDestroy()
    {
        choiceReferences.activate -= StartChoiceSequence;
        choiceReferences.deactivate -= DeactivateChoices;
    }

}
