using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private ResourceManager resourceManager;

    private TextMeshProUGUI ui_TimeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = GetComponent<ResourceManager>();
        ui_TimeOfDay = GameObject.FindWithTag("TimeOfDayUI").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDayUI(resourceManager.GetTimeOfDay());
    }

    void UpdateTimeOfDayUI(Vector2 inTimeOfDay)
    {
        string timeString = "";
        string timePhase = "AM";

        float hour = inTimeOfDay.x;
        float min = inTimeOfDay.y;


        if ((int)hour >= 12)
        {
            timePhase = "PM";
        }


        if ((int)hour > 12)
        {
            hour = hour - 12;
        }


        
        // if hours is lower than 10
        if(hour < 10)
        {
            timeString = "0" + ((int)(hour)).ToString() + ":";
        }
        else
        {
            timeString = ((int)(hour)).ToString() + ":";
        }

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
}
