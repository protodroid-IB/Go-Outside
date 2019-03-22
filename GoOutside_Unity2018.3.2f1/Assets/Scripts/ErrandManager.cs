﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrandManager : MonoBehaviour
{ 
    [SerializeField]
    private GameObject letterboxParent;

    private Dictionary<int, LetterBox> chosenLetterboxes = new Dictionary<int, LetterBox>();

    private int numLetterBoxesPairedWithHouses = 0;


    [SerializeField]
    private int totalNumGymMachines = 6, totalNumLetters = 12, totalNumDogs = 5;

    private int currentNumGymMachines = 0, currentNumLetters = 0, currentNumDogs = 0;

    private bool sisterArrivedAtSchool = false, sisterBackFromSchool = false;

    private bool workoutComplete = false, lettersDelivered = false, dogsPatted = false;



    private void InitialiseMarkers()
    {
        FindHousesToDeliverTo();
        GlobalReferences.instance.mapUIManager.InitialiseHouseMarkers(chosenLetterboxes);
    }

    public void IncrementNumPariedLetterboxes()
    {
        numLetterBoxesPairedWithHouses++;

        if (numLetterBoxesPairedWithHouses >= letterboxParent.transform.childCount)
        {
            InitialiseMarkers();
        } 
    }




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

    public void IncrementLettersCount(LetterBox inLetterBox)
    {
        if (chosenLetterboxes.ContainsValue(inLetterBox))
        {
            if (currentNumLetters < totalNumLetters)
            {
                currentNumLetters++;
                GlobalReferences.instance.uiManager.UpdateCountUI(Errands.DeliverLetters, currentNumLetters, totalNumLetters);
                int key = 0;

                foreach (KeyValuePair<int, LetterBox> letterBox in chosenLetterboxes)
                {
                    if (letterBox.Value == inLetterBox)
                    {
                        key = letterBox.Key;
                    }
                }

                GlobalReferences.instance.mapUIManager.RemoveLetterMarker(key);
            }
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


    private void FindHousesToDeliverTo()
    {
        for(int i=0; i < totalNumLetters; i++)
        {
            int rand = UnityEngine.Random.Range(0, letterboxParent.transform.childCount);

            while(chosenLetterboxes.ContainsKey(rand))
            {
                rand = UnityEngine.Random.Range(0, letterboxParent.transform.childCount);
            }

            chosenLetterboxes.Add(rand, letterboxParent.transform.GetChild(rand).GetComponent<LetterBox>());
        }
    }




}
