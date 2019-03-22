using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairHouse : MonoBehaviour
{
    private LetterBox letterbox;

    private void Awake()
    {
        letterbox = GetComponentInParent<LetterBox>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(letterbox.GetHouse() == null)
        {
            if (other.CompareTag("House"))
            {
                letterbox.SetHouse(other.gameObject);
            }
            else if (other.CompareTag("HouseMesh"))
            {
                letterbox.SetHouse(other.transform.parent.gameObject);
            }
        }
    }
}
