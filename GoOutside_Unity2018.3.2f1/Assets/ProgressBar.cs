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
    private ProgressController progressController;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponentInParent<Interactable>();
        progressController = interactable.transform.GetComponent<ProgressController>();
        thisAnimator = GetComponent<Animator>();

        interactable.interacting += UpdateBar;
        interactable.beginInteract += BeginInteract;
        interactable.endInteract += EndInteract;

        progressController.progressComplete += OnFull;
    }

    private void OnDestroy()
    {
        interactable.interacting -= UpdateBar;
        interactable.beginInteract -= BeginInteract;
        interactable.endInteract -= EndInteract;

        progressController.progressComplete -= OnFull;
    }

    private void OnFull()
    {
        thisAnimator.SetBool("Full", true);
        interactable.interacting -= UpdateBar;
    }

    private void UpdateBar()
    {
        // somethign here
    }

    private void Update()
    {
         UpdateBarVisuals();
    }

    private void UpdateBarVisuals()
    {
        if(progressController != null && fillTransform != null)
            fillTransform.localScale = new Vector3(progressController.GetProgress(), fillTransform.localScale.y, fillTransform.localScale.z);
    }

    private void BeginInteract()
    {
        thisAnimator.ResetTrigger("Off");
        thisAnimator.SetTrigger("Idle");
    }

    private void EndInteract()
    {
        thisAnimator.ResetTrigger("Idle");
        thisAnimator.SetTrigger("Off");
    }

}
