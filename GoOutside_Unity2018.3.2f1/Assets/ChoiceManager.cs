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
    private bool canSwap = false;

    [SerializeField]
    [Range(5, 100)]
    private float buttonSwapSpeed = 25f;

    private bool canSelectChoice = false;
    private bool choiceSelected = false;
    private int choiceSelectedNum = -1;

    float[] choiceEffects = new float[4];



    private class ChoiceButton
    {
        public int choiceNum;
        public char buttonType;
        public Vector3 buttonPosition;

        public int GetButtonNumber()
        {
            int buttonNum = 0;

            switch(buttonType)
            {
                case 'W':
                    buttonNum = 0;
                    break;

                case 'A':
                    buttonNum = 1;
                    break;

                case 'S':
                    buttonNum = 2;
                    break;

                case 'D':
                    buttonNum = 3;
                    break;
            }

            return buttonNum;
        }
    }

    private ChoiceButton[] choiceButtons = new ChoiceButton[4];

    private int GetButtonNumFromChoiceNum(int inChoice)
    {
        int num = 0;

        for(int i=0; i < choiceButtons.Length; i++)
        {
            if(inChoice == choiceButtons[i].choiceNum)
            {
                num = i;
                return num;
            }
        }

        return num;
    }

    private void Start()
    {
        choiceReferences.gameObject.SetActive(true);
        choiceReferences.activate += StartChoiceSequence;
        choiceReferences.deactivate += DeactivateChoices;
        totalNumChoiceSwaps = timeToMakeChoice / numSecondsBetweenChoiceSwaps;

        GlobalReferences.instance.playerInteract.option1Interact += Button1Selected;
        GlobalReferences.instance.playerInteract.option2Interact += Button2Selected;
        GlobalReferences.instance.playerInteract.option3Interact += Button3Selected;
        GlobalReferences.instance.playerInteract.option4Interact += Button4Selected;

        InitialiseButtons();
    }

    private void Update()
    {
        if(startTimer)
        {
            canSelectChoice = true;
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

            if(canSwap)
            {
                    UpdateButtonPositions();   
            }

            timer += Time.deltaTime;
        }
    }


    private void SelectChoice(int inChoiceNum)
    {
        if(canSelectChoice)
        {
            if (!choiceSelected)
            {
                choiceSelected = true;
                choiceSelectedNum = inChoiceNum;
                choiceReferences.AnimateSelected(choiceSelectedNum, true);
                startTimer = false;

                Invoke("EndChoiceSequence", 1f);
            }
        }   
    }

    public void Button1Selected()
    {
        SelectChoice(choiceButtons[0].choiceNum);
    }

    public void Button2Selected()
    {
        SelectChoice(choiceButtons[1].choiceNum);
    }

    public void Button3Selected()
    {
        SelectChoice(choiceButtons[2].choiceNum);
    }

    public void Button4Selected()
    {
        SelectChoice(choiceButtons[3].choiceNum);
    }
















    private void InitialiseButtons()
    {

        currentChoiceSwap = 1;
        swapNum = currentChoiceSwap * numSecondsBetweenChoiceSwaps;
        canSwap = false;
        choiceReferences.buttonAnimateOrder = new int[4] { 0, 1, 2, 3 };
        choiceSelected = false;


        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i] = new ChoiceButton();
            choiceReferences.Buttons[i].transform.position = choiceReferences.ButtonSpots[i].transform.position;
            choiceButtons[i].choiceNum = i;
        }

        choiceButtons[0].buttonType = 'W';
        choiceButtons[1].buttonType = 'A';
        choiceButtons[2].buttonType = 'S';
        choiceButtons[3].buttonType = 'D';

    }

    private void UpdateButtonPositions()
    {
        for(int i=0; i < choiceButtons.Length; i++)
        {
            choiceReferences.Buttons[i].transform.position = Vector3.Lerp(choiceReferences.Buttons[i].transform.position, choiceButtons[i].buttonPosition, buttonSwapSpeed * Time.deltaTime);
        }
    }

    private void SwapButtons()
    {
        canSwap = true;
        int[] spotIndex = { 0, 1, 2, 3 };
        GlobalReferences.instance.usefulFunctions.ShuffleArray(ref spotIndex);
        choiceReferences.SetButtonAnimatorOrderArray(spotIndex);

       // string test = "BUTTON ANIMATE ORDER: \t";
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceReferences.Buttons[i].transform.parent = choiceReferences.ButtonSpots[spotIndex[i]].transform;
            choiceButtons[i].choiceNum = spotIndex[i];
            choiceButtons[i].buttonPosition = choiceReferences.ButtonSpots[choiceButtons[i].choiceNum].transform.position;

            //test += choiceReferences.Buttons[i].name + "| " + choiceButtons[i].buttonType.ToString() + ": " + choiceButtons[i].choiceNum.ToString() + "\t" + choiceButtons[i].buttonPosition.ToString() + "\t\t";
        }
        //Debug.Log(test);
    }







    public void ActivateChoices(Choice[] inChoices)
    {
        choiceReferences.gameObject.SetActive(true);

        InitialiseButtons();

        if (choiceReferences.animator != null)
            choiceReferences.animator.SetTrigger("Idle");
        else
        {
            choiceReferences.animator = choiceReferences.transform.GetComponent<Animator>();
            choiceReferences.animator.SetTrigger("Idle");
        }

        for(int i=0; i < choiceReferences.Choices.Length; i++)
        {
            choiceReferences.Choices[i].text = inChoices[i].text;
            choiceEffects[i] = inChoices[i].mentalStateEffect;
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

        if(choiceSelectedNum >= 0)
            choiceReferences.AnimateSelected(GetButtonNumFromChoiceNum(choiceSelectedNum), false);
       

        canSelectChoice = false;
    }


    public void DeactivateChoices()
    {
        choiceReferences.TimerFill.localScale = new Vector3(1f, choiceReferences.TimerFill.localScale.y, choiceReferences.TimerFill.localScale.z);
        choiceReferences.gameObject.SetActive(false);
        GlobalReferences.instance.dialogueManager.SetDialogueActive(false);

        if(choiceSelectedNum < 0)
            GlobalReferences.instance.resourceManager.UpdateMentalState(-0.1f, true);
        else
        {
            if(choiceEffects[choiceSelectedNum] < 0)
                GlobalReferences.instance.resourceManager.UpdateMentalState(choiceEffects[choiceSelectedNum], true);
            else
                GlobalReferences.instance.resourceManager.UpdateMentalState(choiceEffects[choiceSelectedNum]);
        }

        Debug.Log(choiceEffects[choiceSelectedNum]);

        choiceSelectedNum = -1;
    }











    private void OnDestroy()
    {
        choiceReferences.activate -= StartChoiceSequence;
        choiceReferences.deactivate -= DeactivateChoices;
        GlobalReferences.instance.playerInteract.option1Interact -= Button1Selected;
        GlobalReferences.instance.playerInteract.option2Interact -= Button2Selected;
        GlobalReferences.instance.playerInteract.option3Interact -= Button3Selected;
        GlobalReferences.instance.playerInteract.option4Interact -= Button4Selected;
    }

}
