using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SisterManager : MonoBehaviour
{
    private bool droppedAtSchool = false;
    private bool backHome = false;




    public void SetDroppedAtSchool(bool inBool)
    {
        droppedAtSchool = inBool;
        GlobalReferences.instance.errandManager.SisterArrivedAtSchool();
    }

    public void SetBackHome(bool inBool)
    {
        backHome = inBool;
        GlobalReferences.instance.errandManager.SisterBackFromSchool();
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
