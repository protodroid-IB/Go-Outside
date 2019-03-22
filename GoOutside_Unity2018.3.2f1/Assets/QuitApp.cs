using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApp : MonoBehaviour
{
    private int currentlySelected = 0;

    private enum QuitFunction { Menu, Desktop };

    [SerializeField]
    private QuitFunction quitFunction = QuitFunction.Menu;

    [SerializeField]
    private GameObject answers;

    private ApplicationIcon[] buttons;

    private float appMoveTimer = 0f;
    private float appTimeBetweenMoves = 0.25f;

    private void Start()
    {
        buttons = GetComponentsInChildren<ApplicationIcon>();
    }

    private void OnEnable()
    {
        if(buttons == null)
            buttons = GetComponentsInChildren<ApplicationIcon>();

        currentlySelected = 0;
        SetCurrentlySelected();
    }

    private void SetCurrentlySelected()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == currentlySelected)
                buttons[i].SetSelected(true);
            else
                buttons[i].SetSelected(false);
        }

        if (quitFunction == QuitFunction.Menu)
        {
            if (currentlySelected == 0)
            {
                GlobalReferences.instance.playerInteract.interact -= ToMainMenu;
                GlobalReferences.instance.playerInteract.interact += GlobalReferences.instance.mobilePhoneManager.BackHome;
            }
            else
            {
                GlobalReferences.instance.playerInteract.interact -= GlobalReferences.instance.mobilePhoneManager.BackHome;
                GlobalReferences.instance.playerInteract.interact += ToMainMenu;
            }
        }
        else
        {
            if (currentlySelected == 0)
            {
                GlobalReferences.instance.playerInteract.interact -= QuitGame;
                GlobalReferences.instance.playerInteract.interact += GlobalReferences.instance.mobilePhoneManager.BackHome;
            }
            else
            {
                GlobalReferences.instance.playerInteract.interact -= GlobalReferences.instance.mobilePhoneManager.BackHome;
                GlobalReferences.instance.playerInteract.interact += QuitGame;

            }
        }
    }

    private void Update()
    {
        Vector2 direction = GlobalReferences.instance.inputController.move(GlobalReferences.instance.inputController.player, transform.position);

        if (appMoveTimer >= appTimeBetweenMoves)
        {
            if (direction.x <= -0.3f)
            {
                appMoveTimer = 0f;
                currentlySelected--;
                if (currentlySelected < 0) currentlySelected = buttons.Length - 1;
                SetCurrentlySelected();
            }
            else if (direction.x >= 0.3f)
            {
                appMoveTimer = 0f;
                currentlySelected++;
                if (currentlySelected >= buttons.Length) currentlySelected = 0;
                SetCurrentlySelected();
            }
        }
        else
        {
            appMoveTimer += Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if (quitFunction == QuitFunction.Menu)
        {
            GlobalReferences.instance.playerInteract.interact -= ToMainMenu;
            GlobalReferences.instance.playerInteract.interact -= GlobalReferences.instance.mobilePhoneManager.BackHome;
        }
        else
        {
            GlobalReferences.instance.playerInteract.interact -= QuitGame;
            GlobalReferences.instance.playerInteract.interact -= GlobalReferences.instance.mobilePhoneManager.BackHome;
        }
    }


    private void ToMainMenu()
    {
        GameObject.FindWithTag("Preload").GetComponent<SceneController>().ChangeScene("MainMenu");
    }

    private void QuitGame()
    {
        GameObject.FindWithTag("Preload").GetComponent<SceneController>().QuitGame();
    }
}

