using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(Interactable), typeof(ProgressController))]
public class GymMachine : MonoBehaviour
{
    private Interactable interactable;
    private ProgressController progressController;

    [SerializeField]
    private int machineNumber;

    [SerializeField]
    private TextMeshProUGUI numberUI;

    [SerializeField]
    private MeshRenderer beltMR;
    private Material beltMaterial;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        progressController = GetComponent<ProgressController>();
        
        if(beltMR != null)
            beltMaterial = beltMR.material;

        interactable.interacting += OnExercising;
        interactable.notInteracting += NotExercising;
        progressController.progressComplete += OnExerciseCompleted;

        numberUI.text = machineNumber.ToString();
    }

    private void NotExercising()
    {
        if (beltMR != null)
            beltMaterial.SetFloat("_TreadmillOnBool", 0.0f);
    }

    private void OnExercising()
    {
        if (beltMR != null)
            beltMaterial.SetFloat("_TreadmillOnBool", 1.0f);
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

        if (beltMR != null)
            beltMaterial.SetFloat("_TreadmillOnBool", 0.0f);

        interactable.interacting -= OnExercising;
        interactable.notInteracting -= NotExercising;
        progressController.progressComplete -= OnExerciseCompleted;
    }


    public int GetMachineNumber()
    {
        return machineNumber;
    }
}
