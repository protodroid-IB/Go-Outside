using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    private BuildingController buildingController;
    // Start is called before the first frame update
    void Start()
    {
        buildingController = GetComponentInParent<BuildingController>();
    }


    private void OnTriggerStay(Collider other)
    {
        bool inDoorWay = false;

        if (CompareTag("DoorEnter"))
        {
            if (other.CompareTag("PlayerModel"))
            {
                buildingController.SetDissolveRoofBool(true);
                buildingController.SetTargetDissolve(1.0f);
                buildingController.ActivateDissolveLerp();
                inDoorWay = true;
            }

            
        }

        if (CompareTag("DoorExit") && inDoorWay == false)
        {
            if (other.CompareTag("PlayerModel"))
            {
                buildingController.SetTargetDissolve(0.0f);
                buildingController.ActivateDissolveLerp();
            }

            
        }
    }

}
