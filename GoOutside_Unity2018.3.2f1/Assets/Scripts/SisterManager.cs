using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SisterManager : MonoBehaviour
{
    private bool droppedAtSchool = false;
    private bool backHome = false;

    private SisterMovement sisterMovement;

    [SerializeField]
    private Transform walkIntoSchoolPosition, waitAtSchoolPosition, walkIntoHomePosition;

    private Vector3 positionToRemember = Vector3.zero;
    private Vector3 farAwayPosition = new Vector3(-1000f, -1000f, -1000f);


    private void Start()
    {
        sisterMovement = GetComponent<SisterMovement>();
    }

    private void Update()
    {
        Vector2 timeOfDay = GlobalReferences.instance.resourceManager.GetTimeOfDay();

        // if not dropped off or picked up
        if(droppedAtSchool == false && backHome == false)
        {
            // if between only not dropped at school yet
            if(timeOfDay.x >= 9 && timeOfDay.x < 15)
            {
                GlobalReferences.instance.resourceManager.adjustableSpeedRate = 2f;
            }
            // if not dropped at school or dropped home in time
            else if(timeOfDay.x >= 15)
            {
                GlobalReferences.instance.resourceManager.adjustableSpeedRate = 3f;
            }
        }
        else if(droppedAtSchool == true && backHome == false)
        {
            if (timeOfDay.x >= 15)
            {
                GlobalReferences.instance.resourceManager.adjustableSpeedRate = 2f;
            }
        }
        else if (droppedAtSchool == false && backHome == true)
        {
                GlobalReferences.instance.resourceManager.adjustableSpeedRate = 2f;
        }
        else
        {
            GlobalReferences.instance.resourceManager.adjustableSpeedRate = 1f;
        }


    }

    public void SetDroppedAtSchool(bool inBool)
    {
        droppedAtSchool = inBool;
        GlobalReferences.instance.errandManager.SisterArrivedAtSchool();
        sisterMovement.WalkIntoSchool(walkIntoSchoolPosition.position);
    }


    public void SetBackHome(bool inBool)
    {
        backHome = inBool;
        GlobalReferences.instance.errandManager.SisterBackFromSchool();
        sisterMovement.WalkIntoHome(walkIntoHomePosition.position);
    }




    public bool GetDroppedAtSchool()
    {
        return droppedAtSchool;
    }

    public bool GetBackHome()
    {
        return backHome;
    }


    
}
