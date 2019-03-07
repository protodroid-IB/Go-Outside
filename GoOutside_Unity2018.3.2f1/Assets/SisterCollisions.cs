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
        if(other.CompareTag("SchoolSisterZone"))
        {
            if(!sisterManager.GetDroppedAtSchool())
            {
                sisterManager.SetDroppedAtSchool(true);
            }
        }
    }
}
