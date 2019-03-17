using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    public enum MenuElement { Play, ControllerType, Options, Exit };

    [System.Serializable]
    public class MenuItem
    {
        public MenuElement menuElement;
        public Animator animator;
        public TextMeshProUGUI text;
        public string sceneName;
        public Transform pointerSpotLocation;
        public Button button;
    }

    [SerializeField]
    private GameObject pointer;

    [SerializeField]
    private GameObject optionsPanel;

    [SerializeField]
    private float pointerMoveSpeed = 10f;

    [SerializeField]
    private float timeBetweenMoves = 1f;

    [SerializeField]
    private TextMeshProUGUI controllerText;

    [SerializeField]
    private MenuItem[] menuItems;

    private InputController inputController;


    private float moveTimer = 0f;

    private Vector3 newPointerPosition;
    private int currentMenuItemSelected = 0;

    private void Start()
    {
        inputController = GameObject.FindWithTag("Preload").GetComponent<InputController>();
        newPointerPosition = menuItems[0].pointerSpotLocation.position;

        controllerText.text = inputController.ControllerType.ToString();
    }

    private void ReceiveInput()
    {
        Vector2 direction = inputController.move(inputController.player, transform.position);
        bool interactPress = inputController.interact1Press(inputController.player, inputController.buttonHoldTime, ref inputController.holdTimer, ref inputController.startHoldTimer);
        bool exitOptions = false;

        bool gamePadSelect = inputController.option1(inputController.player);
        bool gamePadBack = inputController.option2(inputController.player);

        

        if (moveTimer >= timeBetweenMoves)
        {
            if(inputController.ControllerType == ControllerType.Keyboard)
            {
                gamePadSelect = false;
                gamePadBack = false;
            }
            else
            {
                interactPress = false;
            }

            if (interactPress == true || gamePadSelect == true)
            {
                MenuElement currentMenu = menuItems[currentMenuItemSelected].menuElement;

                switch (currentMenu)
                {
                    case MenuElement.Play:
                        ChangeScene();
                        break;

                    case MenuElement.ControllerType:
                        ChangeControls();
                        break;

                    case MenuElement.Options:
                        ActivateOptionsPanel();
                        break;

                    case MenuElement.Exit:
                        ExitGame();
                        break;
                }
            }

            if(optionsPanel.activeInHierarchy == true)
            {
                if(gamePadBack || exitOptions)
                {
                    DeactivateOptionsPanel();
                }
            }

            if (direction.y <= -0.3f)
            {
                moveTimer = 0f;
                IncrementMenuSelection();
                newPointerPosition = menuItems[currentMenuItemSelected].pointerSpotLocation.position;
                Debug.Log("SCROLL TO MENU ITEM: " + menuItems[currentMenuItemSelected].sceneName);
            }
            else if(direction.y >= 0.3f)
            {
                moveTimer = 0f;
                DecrementMenuSelection();
                newPointerPosition = menuItems[currentMenuItemSelected].pointerSpotLocation.position;
                Debug.Log("SCROLL TO MENU ITEM: " + menuItems[currentMenuItemSelected].sceneName);
            }
        }
        else
        {
            moveTimer += Time.deltaTime;
        }
    }

    private void IncrementMenuSelection()
    {
        currentMenuItemSelected++;
        ClampMenuSelection();
    }

    private void DecrementMenuSelection()
    {
        currentMenuItemSelected--;
        ClampMenuSelection();
    }

    private void ClampMenuSelection()
    {
        if (currentMenuItemSelected < 0) currentMenuItemSelected = menuItems.Length - 1;
        else if (currentMenuItemSelected >= menuItems.Length) currentMenuItemSelected = 0;
    }

    private void UpdatePointerPosition()
    {
        pointer.transform.position = Vector3.Lerp(pointer.transform.position, newPointerPosition, pointerMoveSpeed * Time.deltaTime);
    }



    // Update is called once per frame
    void Update()
    {
        ReceiveInput();
        UpdatePointerPosition();
    }

    public void SetMenuItemSelected(int inSelection)
    {
        currentMenuItemSelected = inSelection;
        newPointerPosition = menuItems[currentMenuItemSelected].pointerSpotLocation.position;
        Debug.Log("SCROLL TO MENU ITEM: " + menuItems[currentMenuItemSelected].sceneName);
    }

    public void ChangeControls()
    {
        if ((int)inputController.ControllerType == 0)
        {
            inputController.ChangeControls((ControllerType)1);
        }
        else
        {
            inputController.ChangeControls((ControllerType)0);
        }

        controllerText.text = inputController.ControllerType.ToString();
    }

    public void ChangeScene()
    {
        SceneController.instance.ChangeScene(menuItems[currentMenuItemSelected].sceneName);
    }

    public void ActivateOptionsPanel()
    {
        optionsPanel.SetActive(true);
    }

    public void DeactivateOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        SceneController.instance.QuitGame();
    }

    
}
