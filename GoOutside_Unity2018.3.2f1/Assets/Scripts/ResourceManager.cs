using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    #region singleton instance
    public static ResourceManager instance;

    private void MakeSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }
    #endregion


    private string timeOfDay = "";
    private int hour = 8;
    private float mins = 0;

    private float mentalDeductSpeed = 1f;
    private float mentalState = 1f;

    private SphereMask sphereMask;


    [Header("THE TOTAL REAL TIME LENGTH THAT A DAY BETWEEN 8AM AND 8PM SHOULD RUN FOR IN MINUTES")]
    [SerializeField]
    private float totalGameTime = 1f;

    private float totalInGameMinutes = 720f;
    private float gameSpeed;

    [SerializeField]
    private bool startGameTime = false;




    //[SerializeField]
    //private float timeProgressionSpeed = 5f;

    // Start is called before the fcirst frame update
    void Start()
    {
        //timeProgressionSpeed = totalInGameMinutes / totalGameTime;
        
        sphereMask = GameObject.FindWithTag("Player").GetComponent<SphereMask>();
        gameSpeed = totalInGameMinutes / (totalGameTime * 60f);
        mentalDeductSpeed = 1f / (totalGameTime * 60f);
    }




    // Update is called once per frame
    void Update()
    {
        if (startGameTime)
        {
            TrackTime();
            MentalStateWithTime();
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



    public Vector2 GetTimeOfDay()
    {
        return new Vector2((float)hour, mins);
    }







}
