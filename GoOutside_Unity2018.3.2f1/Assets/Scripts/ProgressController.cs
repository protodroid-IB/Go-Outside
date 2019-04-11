using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class ProgressController : MonoBehaviour
{
    private Interactable interactable;

    private float progress = 0f;

    [SerializeField]
    private float timeToProgress = 5f;
    private float fullProgress = 1f;
    private float progressSpeed = 1f;

    [SerializeField]
    private float timeToDrainProgress = 25f;
    private float drainSpeed = 1f;

    private bool isFull = false;
    private bool isEmpty = true;

    public delegate void Complete();
    public Complete progressComplete;



    // Start is called before the first frame update
    void Start()
    {
        // grab interactable component
        interactable = GetComponent<Interactable>();

        // subscribe to interact events
        interactable.beginInteract += BeginProgress;
        interactable.interacting += UpdateProgress;
        interactable.endInteract += EndProgress;

        // find progress increase and decrease speeds
        progressSpeed = fullProgress / timeToProgress;
        drainSpeed = fullProgress / timeToDrainProgress;
    }



    // Update is called once per frame
    void Update()
    {
        CheckProgressState();

        if (!isFull && !isEmpty)
            DecreaseProgress();
    }

    




    /*
     * THE INTERACT EVENT METHODS
    */

    private void BeginProgress()
    {
        //throw new NotImplementedException();
    }

    private void UpdateProgress()
    {
        if (!isFull)
        {
            IncreaseProgress();
        }
    }

    private void EndProgress()
    {
        //throw new NotImplementedException();
    }


    /*
     * FUNCTIONALITY 
    */ 

    private void IncreaseProgress()
    {
        progress += progressSpeed * Time.deltaTime;

        if (progress >= 1.0f)
        {
            OnFull();
        }
    }

    private void DecreaseProgress()
    {
        progress -= drainSpeed * Time.deltaTime;
    }

    private void OnFull()
    {
        interactable.beginInteract -= BeginProgress;
        interactable.interacting -= UpdateProgress;
        interactable.endInteract -= EndProgress;

        progressComplete.Invoke();
    }

    


    private void CheckProgressState()
    {
        if (progress <= 0.0f)
        {
            isEmpty = true;
            progress = 0.0f;
        }
        else isEmpty = false;

        if (progress >= 1.0f)
        {
            isFull = true;
            progress = 1.0f;
        }
        else isFull = false;
    }

    public bool ProgressComplete()
    {
        if (isFull)
            return true;
        else
            return false;
    }

    public bool ProgressNone()
    {
        if (isEmpty)
            return true;
        else
            return false;
    }

    public float GetProgress()
    {
        return progress;
    }



    private void OnDestroy()
    {
        interactable.beginInteract -= BeginProgress;
        interactable.interacting -= UpdateProgress;
        interactable.endInteract -= EndProgress;
    }
}
