using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI ui_TimeOfDay;

    private Image ui_dropOffSisterTick, ui_workoutTick, ui_deliverLettersTick, ui_patDogsTick, ui_pickUpSisterTick;

    private TextMeshProUGUI ui_workoutCount, ui_letterCount, ui_DogCount;

    // Start is called before the first frame update
    void Start()
    {
        ui_TimeOfDay = GameObject.FindWithTag("TimeOfDayUI").GetComponent<TextMeshProUGUI>();

        GrabToDoListReferences();

        for(int i=0; i < System.Enum.GetNames(typeof(Errands)).Length; i++)
        {
            UntickCheckbox((Errands)i);
        }

        UpdateCountUI(Errands.Workout, GlobalReferences.instance.errandManager.GetGymMachinesCompleted(), GlobalReferences.instance.errandManager.GetTotalGymMachines());
        UpdateCountUI(Errands.DeliverLetters, GlobalReferences.instance.errandManager.GetLettersDelivered(), GlobalReferences.instance.errandManager.GetTotalLetters());
        UpdateCountUI(Errands.PatDogs, GlobalReferences.instance.errandManager.GetDogsPatted(), GlobalReferences.instance.errandManager.GetTotalDogs());

    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDayUI(GlobalReferences.instance.resourceManager.GetTimeOfDay());
    }

    private void UpdateTimeOfDayUI(Vector2 inTimeOfDay)
    {
        string timeString = "";
        string timePhase = "am";

        float hour = inTimeOfDay.x;
        float min = inTimeOfDay.y;


        if ((int)hour >= 12)
        {
            timePhase = "pm";
        }


        if ((int)hour > 12)
        {
            hour = hour - 12;
        }


        

        timeString = ((int)(hour)).ToString() + ":";


        if(inTimeOfDay.y < 10)
        {
            timeString += "0" + ((int)(inTimeOfDay.y)).ToString() + " " + timePhase;
        }
        else
        {
            timeString += ((int)(inTimeOfDay.y)).ToString() + " " + timePhase;
        }

        ui_TimeOfDay.text = timeString;
    }

    private void GrabToDoListReferences()
    {
        ui_dropOffSisterTick = GameObject.FindWithTag("TickDropSister").GetComponent<Image>();
        ui_workoutTick = GameObject.FindWithTag("TickWorkout").GetComponent<Image>();
        ui_deliverLettersTick = GameObject.FindWithTag("TickLetters").GetComponent<Image>();
        ui_patDogsTick = GameObject.FindWithTag("TickDogs").GetComponent<Image>();
        ui_pickUpSisterTick = GameObject.FindWithTag("TickPickUpSister").GetComponent<Image>();

        ui_workoutCount = GameObject.FindWithTag("UICountWorkout").GetComponent<TextMeshProUGUI>();
        ui_letterCount = GameObject.FindWithTag("UICountLetters").GetComponent<TextMeshProUGUI>();
        ui_DogCount = GameObject.FindWithTag("UICountDogs").GetComponent<TextMeshProUGUI>();
    }



    public void TickCheckbox(Errands inErrand)
    {
        switch(inErrand)
        {
            case Errands.DropSister:
                ui_dropOffSisterTick.enabled = true;
                break;

            case Errands.Workout:
                ui_workoutTick.enabled = true;
                break;

            case Errands.DeliverLetters:
                ui_deliverLettersTick.enabled = true;
                break;

            case Errands.PatDogs:
                ui_patDogsTick.enabled = true;
                break;

            case Errands.PickUpSister:
                ui_pickUpSisterTick.enabled = true;
                break;
        }
    }

    public void UntickCheckbox(Errands inErrand)
    {
        switch (inErrand)
        {
            case Errands.DropSister:
                ui_dropOffSisterTick.enabled = false;
                break;

            case Errands.Workout:
                ui_workoutTick.enabled = false;
                break;

            case Errands.DeliverLetters:
                ui_deliverLettersTick.enabled = false;
                break;

            case Errands.PatDogs:
                ui_patDogsTick.enabled = false;
                break;

            case Errands.PickUpSister:
                ui_pickUpSisterTick.enabled = false;
                break;
        }
    }



    public void UpdateCountUI(Errands inErrand, int currentNum, int totalNum)
    {
        string count = "(" + currentNum + " / " + totalNum + ")";

        switch(inErrand)
        {
            case Errands.Workout:
                ui_workoutCount.text = count;
                break;

            case Errands.DeliverLetters:
                ui_letterCount.text = count;
                break;

            case Errands.PatDogs:
                ui_DogCount.text = count;
                break;
        }
    }
}
