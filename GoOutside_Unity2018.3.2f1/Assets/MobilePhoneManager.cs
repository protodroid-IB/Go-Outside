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

    [SerializeField]
    private GameObject messageAppGO, mapAppGo, exerciseAppGO;

    private MessageApplication messageApp;
    private bool canShowMessage = false;
    private string latestPlayerResponse;

    private bool inApplication = false;

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
        private int index = 0;
        public TimeOfDay alertTime;
        public string alertMessageSender;
        public string alertMessageText;
        public Choice[] responses = new Choice[4];
        private bool playerResponded = false;

        public bool GetPlayerResponded()
        {
            return playerResponded;
        }

        public void SetPlayerResponded(bool inResponded)
        {
            playerResponded = inResponded;
        }

        public void SetIndex(int inIndex)
        {
            index = inIndex;
        }

        public int GetIndex()
        {
            return index;
        }
    }

    [SerializeField]
    private PhoneAlert[] phoneAlerts;

    private int currentPhoneAlert = -1;

    private TimeOfDay nextAlertTime = new TimeOfDay();

    public MobilePhoneState MobileState { get => mobileState;}


    // Start is called before the first frame update
    void Start()
    {
        mobileAnimator = mobileGO.GetComponent<Animator>();
        applicationsAnimator = applicationsGO.GetComponent<Animator>();
        applicationIcons = applicationsGO.GetComponentsInChildren<ApplicationIcon>();
        messageApp = messageAppGO.GetComponent<MessageApplication>();

        for (int i=0; i < phoneAlerts.Length; i++)
        {
            phoneAlerts[i].SetIndex(i);
        }

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
            if(!phoneAlerts[i].GetPlayerResponded())
            {
                if (phoneAlerts[i].alertTime.hour <= hour)
                {
                    hour = phoneAlerts[i].alertTime.hour;

                    if (phoneAlerts[i].alertTime.minute <= mins)
                    {
                        mins = phoneAlerts[i].alertTime.minute;
                        currentPhoneAlert = phoneAlerts[i].GetIndex();
                    }
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
        if (!inApplication)
        {
            if (!GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(applicationsAnimator, "Apps_Idle"))
            {
                if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
                {
                    applicationsAnimator.ResetTrigger("Open");
                    applicationsAnimator.ResetTrigger("Close");
                    applicationsAnimator.SetTrigger("Open");
                }
            }
        }
        else
        {
            switch (currentlySelected)
            {
                case 0:
                    MessageLogic();
                    break;

                case 1:
                    MapLogic();
                    break;

                case 2:
                    
                    break;
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
            GlobalReferences.instance.playerInteract.interact += SelectApplication;
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
        currentlySelected = 0;

        if(!canShowMessage)
        {
            messageApp.SetNPCMessageActive(false);
        }
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
                    canShowMessage = true;
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
        
        if(!messageAppGO.activeInHierarchy && !mapAppGo.activeInHierarchy && !exerciseAppGO.activeInHierarchy)
        {
            if (inApplication)
            {
                if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
                {
                    if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(applicationsAnimator, "Apps_Off"))
                    {
                        switch (currentlySelected)
                        {
                            case 0:
                                messageAppGO.SetActive(true);
                                break;

                            case 1:
                                mapAppGo.SetActive(true);
                                break;

                            case 2:
                                exerciseAppGO.SetActive(true);
                                break;
                        }
                    }
                }
            }
            else
            {
                MoveThroughApplications(direction);
            }
        }
        
        
        
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
    }

    private void SetApplicationIconSelected(int inIcon)
    {
        for (int i = 0; i < applicationIcons.Length; i++)
        {
            if (i == inIcon) applicationIcons[i].SetSelected(true);
            else applicationIcons[i].SetSelected(false);
        }
    }

    private void SelectApplication()
    {
        if(inApplication == false)
        {
            if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
            {
                if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(applicationsAnimator, "Apps_Idle"))
                {
                    applicationsAnimator.ResetTrigger("Open");
                    applicationsAnimator.ResetTrigger("Close");
                    applicationsAnimator.SetTrigger("Close");

                    GlobalReferences.instance.playerInteract.interact -= SelectApplication;
                    GlobalReferences.instance.playerInteract.mobilePhoneInteract -= ClosePhone;
                    GlobalReferences.instance.playerInteract.mobilePhoneInteract += BackHome;

                    inApplication = true;
                }
            }
        }
        
    }

    private void BackHome()
    {
        if (inApplication == true)
        {
            if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(mobileAnimator, "MobilePhone_LargeIdle"))
            {
                if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(applicationsAnimator, "Apps_Off"))
                {

                    GlobalReferences.instance.playerInteract.mobilePhoneInteract -= BackHome;
                    GlobalReferences.instance.playerInteract.interact += SelectApplication;
                    GlobalReferences.instance.playerInteract.mobilePhoneInteract += ClosePhone;

                    applicationsAnimator.ResetTrigger("Open");
                    applicationsAnimator.ResetTrigger("Close");
                    applicationsAnimator.SetTrigger("Open");


                    if (messageAppGO.activeInHierarchy)
                    {
                        messageAppGO.SetActive(false);
                        messageApp.SetNPCMessageActive(false);
                    }
                    else if (mapAppGo.activeInHierarchy)
                        mapAppGo.SetActive(false);
                    else if (exerciseAppGO.activeInHierarchy)
                        exerciseAppGO.SetActive(false);

                    inApplication = false;
                }
            }
        }
    }

    private void MapLogic()
    {
        GlobalReferences.instance.mapCameraMovement.MoveCamera();
    }

    private void MessageLogic()
    {
        if(canShowMessage)
        {
            messageApp.SetNPCMessageActive(true);

            if (!phoneAlerts[currentPhoneAlert].GetPlayerResponded())
            {
                messageApp.SetNPCName(phoneAlerts[currentPhoneAlert].alertMessageSender);
                messageApp.SetNPCMessage(phoneAlerts[currentPhoneAlert].alertMessageText);
                messageApp.SetNPCTime(phoneAlerts[currentPhoneAlert].alertTime);

                if (!GlobalReferences.instance.choiceManager.ChoiceReferences.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    GlobalReferences.instance.choiceManager.ActivateChoices(phoneAlerts[currentPhoneAlert].responses);
                    GlobalReferences.instance.playerInteract.mobilePhoneInteract -= BackHome;
                }
            }
        }
    }

    public void ChoiceMade(string choice)
    {
        phoneAlerts[currentPhoneAlert].SetPlayerResponded(true);
        messageApp.PlayerRespond(choice);
        latestPlayerResponse = choice;
        TimeOfDay currentTime = new TimeOfDay();
        currentTime.hour = (int)GlobalReferences.instance.resourceManager.GetTimeOfDay().x;
        currentTime.minute = (int)GlobalReferences.instance.resourceManager.GetTimeOfDay().y;
        messageApp.SetPlayerTime(currentTime);
        canShowMessage = false;
        mobileAnimator.SetBool("Alerted", false);
        FindNextTimeOfDay();
        GlobalReferences.instance.playerInteract.mobilePhoneInteract += BackHome;
    }


}
