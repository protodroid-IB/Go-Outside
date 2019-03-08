using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    private ProgressController progressController;

    // Start is called before the first frame update
    void Start()
    {
        progressController = GetComponent<ProgressController>();

        progressController.progressComplete += DogPattingComplete;
    }

    private void Update()
    {
        if(progressController.progressComplete == null)
        {
            Debug.Log(transform.parent.parent.name + ": WE NULL BABY!");
        }
    }

    private void DogPattingComplete()
    {
        GlobalReferences.instance.errandManager.IncrementDogCount();
        Invoke("UnsubscribeDelegate", 1f);
    }

    private void UnsubscribeDelegate()
    {
        progressController.progressComplete -= DogPattingComplete;
    }
}
