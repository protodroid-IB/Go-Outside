using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePhoneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mobileGO;
    private Animator mobileAnimator;
    private bool mobileActive = false;

    private MobilePhoneState mobileState = MobilePhoneState.Closed;

    [System.Serializable]
    public class PhoneAlert
    {
        public TimeOfDay alertTime;
        public string alertMessageSender;
        public string alertMessageText;
    }

    [SerializeField]
    private PhoneAlert[] phoneAlerts;

    private TimeOfDay nextAlertTime = new TimeOfDay();
    
    
    // Start is called before the first frame update
    void Start()
    {
        mobileAnimator = mobileGO.GetComponent<Animator>();
        FindNextTimeOfDay();
        GlobalReferences.instance.playerInteract.mobilePhoneInteract += OpenPhone;
    }

    // Update is called once per frame
    void Update()
    {
        switch(mobileState)
        {
            case MobilePhoneState.Closed:
                Closed();
                break;

            case MobilePhoneState.Open:
                Open();
                break;

            case MobilePhoneState.Alert:
                Alert();
                break;
        }
    }

    private void FindNextTimeOfDay()
    {
        int hour = 100;
        int mins = 100;

        for(int i=0; i < phoneAlerts.Length; i++)
        {
            if(phoneAlerts[i].alertTime.hour <= hour)
            {
                hour = phoneAlerts[i].alertTime.hour;

                if(phoneAlerts[i].alertTime.minute <= mins)
                {
                    mins = phoneAlerts[i].alertTime.minute;
                }
            }
        }

        nextAlertTime.hour = hour;
        nextAlertTime.minute = mins;

        //Debug.Log(nextAlertTime.hour + "\t:" + nextAlertTime.minute);
    }

    private void Alert()
    {

        CheckState_Open();
    }

    private void Open()
    {
        
    }

    private void Closed()
    {

        CheckState_Alert();
        CheckState_Open();
    }

    private void CheckState_Alert()
    {
        if(mobileState == MobilePhoneState.Closed)
        {
            Vector2 timeOfDay = GlobalReferences.instance.resourceManager.GetTimeOfDay();

            //Debug.Log(timeOfDay.x + "\t:" + timeOfDay.y);

            if(nextAlertTime.hour == (int)timeOfDay.x)
            {
                if(nextAlertTime.minute == (int)timeOfDay.y)
                {
                    mobileState = MobilePhoneState.Alert;
                    mobileAnimator.SetBool("Alerted", true);
                }
            }
        }
    }

    private void CheckState_Open()
    {
        
    }

    private void OpenPhone()
    {
        if (!GlobalReferences.instance.gameManager.GetStopAction())
        {
            GlobalReferences.instance.playerInteract.mobilePhoneInteract -= OpenPhone;
            GlobalReferences.instance.playerInteract.mobilePhoneInteract += ClosePhone;

            mobileAnimator.SetTrigger("Open");

            mobileActive = true;
        }
    }

    private void ClosePhone()
    {
        GlobalReferences.instance.playerInteract.mobilePhoneInteract += OpenPhone;
        GlobalReferences.instance.playerInteract.mobilePhoneInteract -= ClosePhone;

        mobileAnimator.SetTrigger("Close");

        mobileActive = false;
    }

    private void CheckState_Closed()
    {

    }


    public bool IsMobileActive()
    {
        return mobileActive;
    }
}
