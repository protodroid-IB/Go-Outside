using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable), typeof(ProgressController))]
public class LetterBox : MonoBehaviour
{
    private Interactable interactable;
    private ProgressController progressController;

    private GameObject house;

    private bool delivered;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        progressController = GetComponent<ProgressController>();

        interactable.interacting += OnDelivering;
        progressController.progressComplete += OnDeliveryComplete;
    }

    private void OnDelivering()
    {
        // animate the machine!!!!
    }

    private void OnDeliveryComplete()
    {
        // update the errand manager
        GlobalReferences.instance.errandManager.IncrementLettersCount(this);
        // visually display some way to show the machine has been exercised on already!
    }


    public void SetHouse(GameObject inHouse)
    {
        house = inHouse;
        GlobalReferences.instance.errandManager.IncrementNumPariedLetterboxes();
    }

    public GameObject GetHouse()
    {
        return house;
    }
}
