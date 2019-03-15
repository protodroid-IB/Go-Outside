using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceUIReferences : MonoBehaviour
{
    [SerializeField]
    private GameObject master;

    [SerializeField]
    private Image backing;

    [SerializeField]
    private RectTransform timerFill;

    [SerializeField]
    private Animator timerAnimator;

    [SerializeField]
    private TextMeshProUGUI[] choices;

    [SerializeField]
    private Animator[] choiceAnimators;

    [SerializeField]
    private GameObject[] buttonSpots;

    [SerializeField]
    private GameObject[] buttons;

    [SerializeField]
    private Animator[] buttonAnimators;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public int[] buttonAnimateOrder = { 0, 1, 2, 3 };

    public delegate void Activate();
    public Activate activate;
    public Activate deactivate;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public GameObject Master { get => master;}
    public Image Backing { get => backing;}
    public RectTransform TimerFill { get => timerFill;}
    public TextMeshProUGUI[] Choices { get => choices;}
    public GameObject[] ButtonSpots { get => buttonSpots;}
    public GameObject[] Buttons { get => buttons;}

    public void ActivateChoices()
    {
        if(activate != null)
            activate.Invoke();
    }

    public void DeactivateChoices()
    {
        if(deactivate != null)
            deactivate.Invoke();
    }

    public void ChoiceOne()
    {
        AnimateChoice(0);
        AnimateButton(buttonAnimateOrder[0]);
    }

    public void ChoiceTwo()
    {
        AnimateChoice(1);
        AnimateButton(buttonAnimateOrder[1]);
    }

    public void ChoiceThree()
    {
        AnimateChoice(2);
        AnimateButton(buttonAnimateOrder[2]);
    }

    public void ChoiceFour()
    {
        AnimateChoice(3);
        AnimateButton(buttonAnimateOrder[3]);
    }

    private void AnimateChoice(int inIndex)
    {
        if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(choiceAnimators[inIndex], "Option_Off"))
        {
            choiceAnimators[inIndex].SetTrigger("Idle");
        }
        else
        {
            choiceAnimators[inIndex].SetTrigger("Off");
        }
    }

    private void AnimateButton(int inIndex)
    {
        if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(buttonAnimators[inIndex], "Option_Off"))
        {
            buttonAnimators[inIndex].SetTrigger("Idle");
        }
        else
        {
            buttonAnimators[inIndex].SetTrigger("Off");
        }
    }

    public void AnimateTimer()
    {
        if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(timerAnimator, "Option_Off"))
        {
            timerAnimator.SetTrigger("Idle");
        }
        else
        {
            timerAnimator.SetTrigger("Off");
        }
    }

    public void AnimateSelected(int inChoiceNum, bool inState)
    {
        choiceAnimators[inChoiceNum].SetBool("Selected", inState);

        Animator buttonAnim = buttonSpots[inChoiceNum].GetComponentInChildren<Animator>();

        buttonAnim.SetBool("Selected", inState);
    }


    public void SetButtonAnimatorOrderArray(int[] inArray)
    {
        for(int i=0; i < inArray.Length; i++)
        {
            buttonAnimateOrder[i] = inArray[i];
        }
    }



}
