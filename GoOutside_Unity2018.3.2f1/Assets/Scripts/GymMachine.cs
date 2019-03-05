using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable), typeof(ProgressController))]
public class GymMachine : MonoBehaviour
{
    private Interactable interactable;
    private ProgressController progressController;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        progressController = GetComponent<ProgressController>();

        interactable.interacting += OnExercising;
        progressController.progressComplete += OnExerciseCompleted;
    }

    private void OnExercising()
    {
        // animate the machine!!!!
    }

    private void OnExerciseCompleted()
    {
        // update the errand manager
        GlobalReferences.instance.errandManager.IncrementGymMachineCount();

        // visually display some way to show the machine has been exercised on already!
    }
}
