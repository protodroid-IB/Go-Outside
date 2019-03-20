using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePhoneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mobileGO;
    private Animator mobileAnimator;

    [SerializeField]
    private GameObject applicationsGO;
    private Animator applicationsAnimator;

    private ApplicationIcon[] applicationIcons;
    private int currentlySelected = 0;

    private float appMoveTimer = 0f;

    [SerializeField]
    private float appTimeBetweenMoves = 0.25f;

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
        applicationsAnimator = applicationsGO.GetComponent<Animator>();
        applicationIcons = applicationsGO.GetComponentsInChildren<ApplicationIcon>();

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

    public bool IsMobileActive()
    {
        return mobileActive;
    }




    private void Alert()
    {
        CheckClosePhoneAnimation();
        CheckState_Open();
    }
   
    private void Open()
    {
        if(!GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(applicationsAnimator, "Apps_Idle"))
        {
            if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
            {
                applicationsAnimator.ResetTrigger("Open");
                applicationsAnimator.ResetTrigger("Close");
                applicationsAnimator.SetTrigger("Open");
            }
        }

        if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
        {
            NavigateApplications();
        }
    }

    private void Closed()
    {
        CheckClosePhoneAnimation();
        CheckState_Alert();
        CheckState_Open();
    }


    private void OpenPhone()
    {
        if (!GlobalReferences.instance.gameManager.GetStopAction())
        {
            GlobalReferences.instance.playerInteract.mobilePhoneInteract -= OpenPhone;
            GlobalReferences.instance.playerInteract.mobilePhoneInteract += ClosePhone;

            mobileAnimator.ResetTrigger("Open");
            mobileAnimator.ResetTrigger("Close");
            mobileAnimator.SetTrigger("Open");
            mobileState = MobilePhoneState.Open;

            mobileActive = true;
            SetApplicationIconSelected(0);
        }
    }

    private void ClosePhone()
    {
        GlobalReferences.instance.playerInteract.mobilePhoneInteract += OpenPhone;
        GlobalReferences.instance.playerInteract.mobilePhoneInteract -= ClosePhone;

        mobileState = MobilePhoneState.Closed;
        applicationsAnimator.ResetTrigger("Open");
        applicationsAnimator.ResetTrigger("Close");
        applicationsAnimator.SetTrigger("Close");

        mobileActive = false;
    }



    private void CheckState_Alert()
    {
        if (mobileState == MobilePhoneState.Closed)
        {
            Vector2 timeOfDay = GlobalReferences.instance.resourceManager.GetTimeOfDay();

            //Debug.Log(timeOfDay.x + "\t:" + timeOfDay.y);

            if (nextAlertTime.hour == (int)timeOfDay.x)
            {
                if (nextAlertTime.minute == (int)timeOfDay.y)
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

    private void CheckClosePhoneAnimation()
    {
        if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
        {
            if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(applicationsAnimator, "Apps_Off"))
            {
                mobileAnimator.ResetTrigger("Open");
                mobileAnimator.ResetTrigger("Close");
                mobileAnimator.SetTrigger("Close");
            }
        }
    }

    private void CheckState_Closed()
    {

    }

    private void NavigateApplications()
    {
        Vector2 direction = GlobalReferences.instance.inputController.move(GlobalReferences.instance.inputController.player, transform.position);
        bool interactPress = GlobalReferences.instance.inputController.interact1Press(GlobalReferences.instance.inputController.player, GlobalReferences.instance.inputController.buttonHoldTime, ref GlobalReferences.instance.inputController.holdTimer, ref GlobalReferences.instance.inputController.startHoldTimer);
        bool exitOptions = false;

        bool gamePadSelect = GlobalReferences.instance.inputController.option1(GlobalReferences.instance.inputController.player);
        bool gamePadBack = GlobalReferences.instance.inputController.option2(GlobalReferences.instance.inputController.player);

        MoveThroughApplications(direction);
    }

    private void MoveThroughApplications(Vector2 direction)
    {
        if (appMoveTimer >= appTimeBetweenMoves)
        {
            if (direction.x <= -0.3f)
            {
                appMoveTimer = 0f;
                currentlySelected--;
                if (currentlySelected < 0) currentlySelected = applicationIcons.Length - 1;
                SetApplicationIconSelected(currentlySelected);
            }
            else if (direction.x >= 0.3f)
            {
                appMoveTimer = 0f;
                currentlySelected++;
                if (currentlySelected >= applicationIcons.Length) currentlySelected = 0;
                SetApplicationIconSelected(currentlySelected);
            }
        }
        else
        {
            appMoveTimer += Time.deltaTime;
        }

        Debug.Log(currentlySelected);
    }

    private void SetApplicationIconSelected(int inIcon)
    {
        for(int i=0; i < applicationIcons.Length; i++)
        {
            if (i == inIcon) applicationIcons[i].SetSelected(true);
            else applicationIcons[i].SetSelected(false);
        }
    }
}
