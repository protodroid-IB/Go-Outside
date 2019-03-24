using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Interactable), typeof(ProgressController))]
public class GymMachine : MonoBehaviour
{
    private Interactable interactable;
    private ProgressController progressController;

    [SerializeField]
    private int machineNumber;

    [SerializeField]
    private TextMeshProUGUI numberUI;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        progressController = GetComponent<ProgressController>();

        interactable.interacting += OnExercising;
        progressController.progressComplete += OnExerciseCompleted;

        numberUI.text = machineNumber.ToString();
    }

    private void OnExercising()
    {
        // animate the machine!!!!
    }

    private void OnExerciseCompleted()
    {
        if(GlobalReferences.instance.exerciseApplication.CheckExerciseComplete(machineNumber))
        {
            // update the errand manager
            GlobalReferences.instance.errandManager.IncrementGymMachineCount();
        }
        else
        {
            GlobalReferences.instance.resourceManager.UpdateMentalState(-0.02f, true);
        }
        
        // visually display some way to show the machine has been exercised on already!
    }


    public int GetMachineNumber()
    {
        return machineNumber;
    }
}
