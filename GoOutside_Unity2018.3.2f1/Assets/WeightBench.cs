using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightBench : MonoBehaviour
{
    private Animator animator;
    private Interactable interactable;
    private ProgressController progressController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        interactable = GetComponent<Interactable>();
        progressController = GetComponent<ProgressController>();

        interactable.interacting += OnLifting;
        interactable.notInteracting += NotLifting;
        interactable.endInteract += NotLifting;
        progressController.progressComplete += OnCompletedLift;
    }

    private void OnCompletedLift()
    {
        animator.SetBool("Lifting", false);
        interactable.interacting -= OnLifting;
        interactable.notInteracting -= NotLifting;
        interactable.endInteract -= NotLifting;
        progressController.progressComplete -= OnCompletedLift;
    }

    private void NotLifting()
    {
        animator.SetBool("Lifting", false);
    }

    private void OnLifting()
    {
        animator.SetBool("Lifting", true);

    }

}
