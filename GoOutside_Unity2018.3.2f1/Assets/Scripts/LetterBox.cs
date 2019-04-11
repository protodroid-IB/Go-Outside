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

    [SerializeField]
    private Animator letterBoxAnim;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        progressController = GetComponent<ProgressController>();

        interactable.interacting += OnDelivering;
        interactable.notInteracting += NotDelivering;
        progressController.progressComplete += OnDeliveryComplete;
    }

    private void NotDelivering()
    {
        letterBoxAnim.SetBool("Delivering", false);
    }

    private void OnDelivering()
    {
        // animate the machine!!!!
        letterBoxAnim.SetBool("Delivering", true);
    }

    private void OnDeliveryComplete()
    {
        letterBoxAnim.SetBool("Delivering", true);
        letterBoxAnim.SetBool("Delivered", true);

        // update the errand manager
        GlobalReferences.instance.errandManager.IncrementLettersCount(this);
        // visually display some way to show the machine has been exercised on already!
        interactable.interacting -= OnDelivering;
        interactable.notInteracting -= NotDelivering;
        progressController.progressComplete -= OnDeliveryComplete;
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
