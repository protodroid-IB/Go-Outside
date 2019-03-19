using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{ 
    private string timeOfDay = "";
    private int hour = 8;
    private float mins = 0;

    private float mentalDeductSpeed = 1f;
    private float mentalState = 1f;


    [Header("THE TOTAL REAL TIME LENGTH THAT A DAY BETWEEN 8AM AND 8PM SHOULD RUN FOR IN MINUTES")]
    [SerializeField]
    private float totalGameTime = 1f;

    private float totalInGameMinutes = 720f;
    private float gameSpeed;

    [SerializeField]
    private bool startGameTime = false;

    private bool dayEnded = false;
    private Vector2 endGameTime = new Vector2(20, 0);

    public bool DayEnded { get => dayEnded;}



    // Start is called before the fcirst frame update
    void Start()
    {

        gameSpeed = totalInGameMinutes / (totalGameTime * 60f);
        mentalDeductSpeed = 1f / (totalGameTime * 60f);
    }




    // Update is called once per frame
    void Update()
    {
        if (startGameTime)
        {
            if(!GlobalReferences.instance.gameManager.GetStopAction())
            {
                TrackTime();
                MentalStateWithTime();
            }           
        }
        else
        {
            UpdateMentalState(0f);
        }
            

    }






    private void TrackTime()
    {
        if(hour >= 20)
        {
            hour = 20;
            mins = 0;
            startGameTime = false;
        }
        else
        {
            mins += (gameSpeed * Time.deltaTime);

            if (mins >= 60)
            {
                hour++;
                mins = 0;
            }
        }

        timeOfDay = hour + ":" + (int)mins;

        CheckEndOfDay();
    }


    private void MentalStateWithTime()
    {
        UpdateMentalState(-mentalDeductSpeed * Time.deltaTime);
    }



    public void UpdateMentalState(float inNum)
    {
        mentalState += inNum;
        GlobalReferences.instance.sphereMask.UpdateMask(mentalState);
    }

    public void UpdateMentalState(float inNum, bool withCameraShake)
    {
        mentalState += inNum;
        GlobalReferences.instance.sphereMask.UpdateMask(mentalState);

        float camShakeTrauma = 10f * Mathf.Abs(inNum);

        if (camShakeTrauma >= 0.9) camShakeTrauma = 0.9f;

        GlobalReferences.instance.cameraShake.AddToTrauma(camShakeTrauma);
        GlobalReferences.instance.chromaticAbberationEffect.TriggerEffect(camShakeTrauma);
    }

    public void CheckEndOfDay()
    {
        if(GetTimeOfDay() == endGameTime)
        {
            dayEnded = true;
        }
    }



    public Vector2 GetTimeOfDay()
    {
        return new Vector2((float)hour, mins);
    }


    public float GetMentalState()
    {
        return mentalState;
    }







}
