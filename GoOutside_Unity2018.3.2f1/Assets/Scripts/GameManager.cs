using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool activateGameLoop = false;

    [SerializeField]
    private bool stopAction = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalReferences.instance.resourceManager.DayEnded && activateGameLoop)
        {
            SceneController.instance.ChangeScene("MainMenu");
        }

        UpdateStopAction();
    }

    private void UpdateStopAction()
    {
        bool dialogueActive = GlobalReferences.instance.dialogueManager.IsDialogueActive();
        bool mobileActive = GlobalReferences.instance.mobilePhoneManager.IsMobileActive();
        bool pauseActive = GlobalReferences.instance.pauseManager.IsPauseActive();

        if(dialogueActive || mobileActive || pauseActive)
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
