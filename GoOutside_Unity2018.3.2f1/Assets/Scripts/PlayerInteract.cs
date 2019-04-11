using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool interacting = false;

    public delegate void Interact();
    public Interact interact;
    public Interact notInteracting;
    public Interact option1Interact;
    public Interact option2Interact;
    public Interact option3Interact;
    public Interact option4Interact;
    public Interact mobilePhoneInteract;
    public Interact pauseInteract;


    // Start is called before the first frame update
    void Start()
    {
        interact += Interacting;
    }

    private void Update()
    {
        bool holding = GlobalReferences.instance.inputController.interact1Hold(GlobalReferences.instance.inputController.player, GlobalReferences.instance.inputController.buttonHoldTime, ref GlobalReferences.instance.inputController.holdTimer, ref GlobalReferences.instance.inputController.startHoldTimer);
        bool pressing = GlobalReferences.instance.inputController.interact1Press(GlobalReferences.instance.inputController.player, GlobalReferences.instance.inputController.buttonHoldTime, ref GlobalReferences.instance.inputController.holdTimer, ref GlobalReferences.instance.inputController.startHoldTimer);

        bool mobilePhone = GlobalReferences.instance.inputController.interact2Press(GlobalReferences.instance.inputController.player);
        bool pause = GlobalReferences.instance.inputController.pause(GlobalReferences.instance.inputController.player);

        bool option1 = GlobalReferences.instance.inputController.option1(GlobalReferences.instance.inputController.player);
        bool option2 = GlobalReferences.instance.inputController.option2(GlobalReferences.instance.inputController.player);
        bool option3 = GlobalReferences.instance.inputController.option3(GlobalReferences.instance.inputController.player);
        bool option4 = GlobalReferences.instance.inputController.option4(GlobalReferences.instance.inputController.player);

        if (holding || pressing)
        {
            interact.Invoke();
        }
        else
        {
            if(notInteracting != null)
            {
                notInteracting.Invoke();
            }
            
        }

        if(mobilePhone)
        {
            if(mobilePhoneInteract != null)
                mobilePhoneInteract.Invoke();
        }

        if(pause)
        {
            if (pauseInteract != null)
                pauseInteract.Invoke();
        }

        if(option1)
        {
            if (option1Interact != null)
                option1Interact.Invoke();
                
        }

        if (option2)
        {
            if (option2Interact != null)
                option2Interact.Invoke();
        }

        if (option3)
        {
            if (option3Interact != null)
                option3Interact.Invoke();
        }

        if (option4)
        {
            if (option4Interact != null)
                option4Interact.Invoke();
        }
    }

    private void Interacting()
    {
        //Debug.Log("Player Interact Attempt!");

        // add general functionality whenever a player interacts with something
    }

    private void OnDestroy()
    {
        interact -= Interacting;
    }
}
