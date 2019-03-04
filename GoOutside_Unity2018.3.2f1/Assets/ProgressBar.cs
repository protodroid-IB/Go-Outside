using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform fillTransform;

    private Animator thisAnimator;

    private Interactable interactable;

    [SerializeField]
    private float timeToFillBar = 5f;
    private float barFullAmount = 1f;
    private float fillBarSpeed = 1f;

    private float timeToDrainBarFromFull = 25f;
    private float drainBarSpeed = 1f;

    private bool isFull = false;
    private bool isEmpty = true;

    private float fillAmount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponentInParent<Interactable>();
        thisAnimator = GetComponent<Animator>();

        interactable.interacting += UpdateBar;
        interactable.beginInteract += BeginInteract;
        interactable.endInteract += EndInteract;

        fillBarSpeed = barFullAmount / timeToFillBar;
        drainBarSpeed = barFullAmount / timeToDrainBarFromFull;
    }

    private void OnDestroy()
    {
        interactable.interacting -= UpdateBar;
        interactable.beginInteract -= BeginInteract;
        interactable.endInteract -= EndInteract;
    }

    private void OnFull()
    {
        interactable.interacting -= UpdateBar;
    }

    private void UpdateBar()
    {
        if (!isFull)
        {
            IncreaseBar();
        } 
    }

    private void Update()
    {
        CheckBarState();

        if(!isFull && !isEmpty)
            DecreaseBar();

        UpdateBarVisuals();
    }

    private void IncreaseBar()
    {
        fillAmount += fillBarSpeed * Time.deltaTime;

        if (isFull)
        {
            OnFull();
        }
    }

    private void DecreaseBar()
    {
        fillAmount -= drainBarSpeed * Time.deltaTime;
    }

    private void UpdateBarVisuals()
    {
        fillTransform.localScale = new Vector3(fillAmount, fillTransform.localScale.y, fillTransform.localScale.z);
    }



    private void CheckBarState()
    {
        if (fillAmount <= 0.0f)
        {
            isEmpty = true;
            fillAmount = 0.0f;
        }
        else isEmpty = false;

        if (fillAmount >= 1.0f)
        {
            isFull = true;
            fillAmount = 1.0f;
        }
        else isFull = false;
    }


    private void BeginInteract()
    {
        thisAnimator.SetTrigger("Idle");
    }

    private void EndInteract()
    {
        thisAnimator.SetTrigger("Off");
    }

}
