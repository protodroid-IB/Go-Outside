using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool activateGameLoop = false;


    public bool disableActions = false;

    private bool stopAction = false;

    private int numTimesDied = 0;
    private bool teleport = false;

    [SerializeField]
    private Transform playerTeleportLocation;

    private bool diedTalk = false;

    private Vector2 nextTime = Vector2.zero;

    [SerializeField]
    private BuildingController playerRoof;

    private bool gameOver = false;

    private float gameTimer = 0f;

    

    // Start is called before the first frame update
    void Start()
    {
        if(activateGameLoop)
            Invoke("StartMumTalking", 3f);
    }

    private void StartMumTalking()
    {
        GlobalReferences.instance.mumDialogue.Talk();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver && activateGameLoop)
        {
            // if end state screen has not yet been enabled
            if (!GlobalReferences.instance.endStateUIManager.gameObject.activeInHierarchy)
            {
                bool completedTasks = GlobalReferences.instance.errandManager.allTasksComplete;
                disableActions = true;

                GlobalReferences.instance.endStateUIManager.gameObject.SetActive(true);
                GlobalReferences.instance.endStateUIManager.SetEndStateUI(completedTasks, numTimesDied, (int)gameTimer);
            }
        }
        else
        {
            Animator fadeAnimator = GlobalReferences.instance.sceneFader.GetAnimator();

            if (GlobalReferences.instance.resourceManager.GetMentalState() <= 0f && teleport == false)
            {
                teleport = true;
                numTimesDied++;
                GlobalReferences.instance.sceneFader.FadeToBlack();
                disableActions = true;
                GlobalReferences.instance.playerMovement.velocity = Vector3.zero;
                nextTime.x = GlobalReferences.instance.resourceManager.GetTimeOfDay().x + 1f;
                nextTime.y = GlobalReferences.instance.resourceManager.GetTimeOfDay().y + 60f;
            }

            if (teleport && GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(fadeAnimator, "SceneFader-Black"))
            {
                GlobalReferences.instance.sceneFader.FadeFromBlack();
                teleport = false;
                GlobalReferences.instance.resourceManager.SetMentalState(1f - 0.1f * (float)numTimesDied);
                GlobalReferences.instance.playerMovement.transform.position = playerTeleportLocation.position;
                GlobalReferences.instance.playerMovement.transform.rotation = playerTeleportLocation.rotation;
                GlobalReferences.instance.playerMovement.velocity = Vector3.zero;
                diedTalk = true;
                GlobalReferences.instance.resourceManager.SetTimeOfDay((int)nextTime.x, (int)((int)nextTime.y % 60));
                playerRoof.DissolveRoof();
            }

            if(diedTalk)
            {
                if (GlobalReferences.instance.dialogueManager.HasFinishedTalking())
                {
                    diedTalk = false;
                    disableActions = false;
                }

                if (GlobalReferences.instance.usefulFunctions.CheckAnimationPlaying(fadeAnimator, "SceneFader-Clear"))
                {
                    GlobalReferences.instance.mumDialogue.TalkYouDied();
                }  
            }

            if (GlobalReferences.instance.resourceManager.DayEnded) gameOver = true;
            if (GlobalReferences.instance.errandManager.allTasksComplete) gameOver = true;

            gameTimer += Time.deltaTime;
        }

        if(GlobalReferences.instance.resourceManager.GetTimeOfDay().x == 15 && GlobalReferences.instance.resourceManager.GetTimeOfDay().y < 2f)
        {
            GlobalReferences.instance.sisterMovement.EnableInteraction();
            GlobalReferences.instance.sisterMovement.EnableHomeDropOff();
        }
        else if(GlobalReferences.instance.resourceManager.GetTimeOfDay().x == 14 && GlobalReferences.instance.resourceManager.GetTimeOfDay().y > 30f)
        {
            GlobalReferences.instance.sisterMovement.DisableSchoolDropOff();
        }



        UpdateStopAction();
    }

    private void UpdateStopAction()
    {
        bool dialogueActive = GlobalReferences.instance.dialogueManager.IsDialogueActive();
        bool mobileActive = GlobalReferences.instance.mobilePhoneManager.IsMobileActive();
       // bool pauseActive = GlobalReferences.instance.pauseManager.IsPauseActive();

        if(dialogueActive || mobileActive || disableActions)
        {
            stopAction = true;
        }
        else
        {
            stopAction = false;
        }
    }

    public bool GetStopAction()
    {
        return stopAction;
    }

    public void SetStopAction(bool inActive)
    {
        stopAction = inActive;
    }
}
