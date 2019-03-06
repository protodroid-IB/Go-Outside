using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrandManager : MonoBehaviour
{
    [SerializeField]
    private int totalNumGymMachines = 6, totalNumLetters = 12, totalNumDogs = 5;

    private int currentNumGymMachines = 0, currentNumLetters = 0, currentNumDogs = 0;

    private bool sisterArrivedAtSchool = false, sisterBackFromSchool = false;

    private bool workoutComplete = false, lettersDelivered = false, dogsPatted = false;



    /*
     *      INCREMENT COUNT METHODS
     */

    public void IncrementGymMachineCount()
    {
        if (currentNumGymMachines < totalNumGymMachines)
        {
            currentNumGymMachines++;
            GlobalReferences.instance.uiManager.UpdateCountUI(Errands.Workout, currentNumGymMachines, totalNumGymMachines);
        }

        if (currentNumGymMachines >= totalNumGymMachines)
            WorkoutComplete();
    }

    public void IncrementLettersCount()
    {
        if (currentNumLetters < totalNumLetters)
        {
            currentNumLetters++;
            GlobalReferences.instance.uiManager.UpdateCountUI(Errands.DeliverLetters, currentNumLetters, totalNumLetters);
            Debug.Log(currentNumLetters.ToString() + "/" + totalNumLetters.ToString());
        }


        if (currentNumLetters >= totalNumLetters)
            LettersDelivered();
    }

    public void IncrementDogCount()
    {
        if (currentNumDogs < totalNumDogs)
        {
            currentNumDogs++;
            GlobalReferences.instance.uiManager.UpdateCountUI(Errands.PatDogs, currentNumDogs, totalNumDogs);
        }

        if (currentNumDogs >= totalNumDogs)
            DogsPatted();
    }



    /*
     *      SISTER ERRAND METHODS
     */

    public void SisterArrivedAtSchool()
    {
        sisterArrivedAtSchool = true;
        GlobalReferences.instance.uiManager.TickCheckbox(Errands.DropSister);
    }

    public void SisterBackFromSchool()
    {
        sisterBackFromSchool = true;
        GlobalReferences.instance.uiManager.TickCheckbox(Errands.PickUpSister);
    }




    /*
     *      ERRAND COMPLETED METHODS
     */

    private void WorkoutComplete()
    {
        workoutComplete = true;

        // update UI
        if (GlobalReferences.instance.uiManager != null)
        {
            GlobalReferences.instance.uiManager.TickCheckbox(Errands.Workout);
        }
    }

    private void LettersDelivered()
    {
        lettersDelivered = true;

        // update UI
        if (GlobalReferences.instance.uiManager != null)
        {
            GlobalReferences.instance.uiManager.TickCheckbox(Errands.DeliverLetters);
        }
    }

    private void DogsPatted()
    {
        dogsPatted = true;

        // update UI
        if (GlobalReferences.instance.uiManager != null)
        {
            GlobalReferences.instance.uiManager.TickCheckbox(Errands.PatDogs);
        }
    }



    /*
     *      GETTERS
     */

    public bool IsWorkoutComplete()
    {
        return workoutComplete;
    }

    public bool IsLettersDelivered()
    {
        return lettersDelivered;
    }

    public bool IsDogsPatted()
    {
        return dogsPatted;
    }

    public bool IsSisterAtSchool()
    {
        return sisterArrivedAtSchool;
    }

    public bool IsSisterBackHome()
    {
        return sisterBackFromSchool;
    }



    public int GetGymMachinesCompleted()
    {
        return currentNumGymMachines;
    }

    public int GetLettersDelivered()
    {
        return currentNumLetters;
    }

    public int GetDogsPatted()
    {
        return currentNumDogs;
    }


    public int GetTotalGymMachines()
    {
        return totalNumGymMachines;
    }

    public int GetTotalLetters()
    {
        return totalNumLetters;
    }

    public int GetTotalDogs()
    {
        return totalNumDogs;
    }






}
