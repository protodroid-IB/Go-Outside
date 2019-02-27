using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private string timeOfDay = "";
    private int hour = 8;
    private float mins = 0;


    [SerializeField]
    private float mentalStateDeductTime = 10f;

    private float mentalDeductSpeed = 1f;
    private float mentalState = 1f;

    private SphereMask sphereMask;


    [SerializeField]
    private float totalGameTime = 30f;

    private float totalInGameMinutes = 720f;
    private float gameSpeed;




    //[SerializeField]
    //private float timeProgressionSpeed = 5f;

    // Start is called before the fcirst frame update
    void Start()
    {
        //timeProgressionSpeed = totalInGameMinutes / totalGameTime;
        
        sphereMask = GameObject.FindWithTag("Player").GetComponent<SphereMask>();
        gameSpeed = totalInGameMinutes / totalGameTime;
        mentalDeductSpeed = 1f / (totalGameTime * 60f);
    }

    // Update is called once per frame
    void Update()
    {
        TrackTime();
        MentalStateWithTime();

    }


    private void TrackTime()
    {
        mins += (gameSpeed * Time.deltaTime);

        if (mins >= 60)
        {
            hour++;
            mins = 0;
        }

        timeOfDay = hour + ":" + (int)mins;
    }

    public Vector2 GetTimeOfDay()
    {
        return new Vector2((float)hour, mins);
    }

    private void MentalStateWithTime()
    {
        UpdateMentalState(-mentalDeductSpeed * Time.deltaTime);
    }



    public void UpdateMentalState(float inNum)
    {
        mentalState += inNum;
        sphereMask.UpdateMask(mentalState);
    }







}
