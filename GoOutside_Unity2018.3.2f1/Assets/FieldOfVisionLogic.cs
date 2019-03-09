using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfVisionLogic : MonoBehaviour
{
    public delegate void Detected();
    public Detected detected;
    public Detected notDetected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerModel"))
        {
            if(detected != null)
            {
                detected.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            if (notDetected != null)
            {
                notDetected.Invoke();
            }
        }
    }
}
