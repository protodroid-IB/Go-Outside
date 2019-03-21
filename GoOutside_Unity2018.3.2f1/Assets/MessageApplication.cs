using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageApplication : MonoBehaviour
{
    [SerializeField]
    private GameObject playerMessageGO;
    private Animator playerMessageAnimator;

    [SerializeField]
    private GameObject npcMessageGO;

    [SerializeField]
    private TextMeshProUGUI npcTime, npcName, npcMessage, playerTime, playerMessage;

    private void Start()
    {
        playerMessageAnimator = playerMessageGO.GetComponent<Animator>();
        npcMessageGO.SetActive(false);
    }

    public void SetNPCMessageActive(bool inActive)
    {
        npcMessageGO.SetActive(inActive);
    }

    public void SetNPCTime(TimeOfDay inTimeOfDay)
    {
        npcTime.text = GenerateUITime(inTimeOfDay);
    }

    public void SetNPCName(string inName)
    {
        npcName.text = inName;
    }

    public void SetNPCMessage(string inMessage)
    {
        npcMessage.text = inMessage;
    }

    public void SetPlayerTime(TimeOfDay inTimeOfDay)
    {
        playerTime.text = GenerateUITime(inTimeOfDay);
    }

    public void PlayerRespond(string inMessage)
    {
        playerMessage.text = inMessage;
        playerMessageAnimator.SetTrigger("Enter");
    }

    public void NewAlert()
    {
        playerMessageAnimator.SetTrigger("Off");
    }


    private string GenerateUITime(TimeOfDay inTimeOfDay)
    {
        string timeString = "";
        string timePhase = "am";

        float hour = inTimeOfDay.hour;
        float min = inTimeOfDay.minute;

        if ((int)hour >= 12)
        {
            timePhase = "pm";
        }


        if ((int)hour > 12)
        {
            hour = hour - 12;
        }

        timeString = ((int)(hour)).ToString() + ":";


        if (inTimeOfDay.minute < 10)
        {
            timeString += "0" + ((int)(inTimeOfDay.minute)).ToString() + " " + timePhase;
        }
        else
        {
            timeString += ((int)(inTimeOfDay.minute)).ToString() + " " + timePhase;
        }

        return timeString;
    }
}
