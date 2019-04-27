using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndStateUIManager : MonoBehaviour
{
    [SerializeField]
    private Color successMainColor, successComplimentaryColor, failMainColor, failComplimentaryColor;

    [SerializeField]
    private string sucessTitle = "SUCCESS", failTitle = "FAILED";

    [SerializeField]
    private string successMessage = "You completed all the tasks set out for you!";
    [SerializeField]
    private string failMessage = "You failed to complete all the tasks set out for you";

    [SerializeField]
    private TextMeshProUGUI objectiveState, objectiveMessage, teleportedTimes, timeTaken;


    public void SetEndStateUI(bool succeded, int numTimesTeleported, int timeTakenSeconds)
    {
        if(succeded == true)
        {
            objectiveState.text = sucessTitle;
            objectiveState.color = successMainColor;

            objectiveMessage.text = successMessage;
            objectiveMessage.color = successComplimentaryColor;

            teleportedTimes.color = successComplimentaryColor;
            timeTaken.color = successComplimentaryColor;
        }
        else
        {
            objectiveState.text = failTitle;
            objectiveState.color = failMainColor;

            objectiveMessage.text = failMessage;
            objectiveMessage.color = failComplimentaryColor;

            teleportedTimes.color = failComplimentaryColor;
            timeTaken.color = failComplimentaryColor;
        }

        teleportedTimes.text = numTimesTeleported.ToString();

        int mins = timeTakenSeconds / 60;
        int seconds = timeTakenSeconds % 60;

        timeTaken.text = mins + " minutes & " + seconds + " seconds";
    }
}
