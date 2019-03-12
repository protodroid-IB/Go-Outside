using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SisterCollisions : MonoBehaviour
{
    private SisterManager sisterManager;

    private void Start()
    {
        sisterManager = transform.parent.GetComponent<SisterManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!sisterManager.GetDroppedAtSchool() || !sisterManager.GetBackHome())
        {
            if (!sisterManager.GetDroppedAtSchool())
            {
                if (other.CompareTag("SchoolSisterZone"))
                {
                    sisterManager.SetDroppedAtSchool(true);
                }
            }
            else
            {
                if (other.CompareTag("HomeSisterZone"))
                {
                    sisterManager.SetBackHome(true);
                }
            }
        }
    }
}
