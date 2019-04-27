using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class InputController : MonoBehaviour
{

    [HideInInspector]
    public PlayerIndex player;

    [Header("Controller Settings")]
    [Space(2)]
    [SerializeField]
    private bool enableControllerChecking = true;

    [SerializeField]
    private ControllerType controllerType;

    [SerializeField]
    private float gamePadDeadZone = 0.1f;

    public float buttonHoldTime = 0.12f;

    
    [HideInInspector]
    public float holdTimer = 0f;

    [HideInInspector]
    public bool startHoldTimer = false;

    private KeyboardControls keyboardControls;
    private GamepadControls gamepadControls;

    // input Delegates
    public delegate Vector2 Directional(PlayerIndex player, Vector3 position);
    public delegate bool Press(PlayerIndex player);
    public delegate bool Hold(PlayerIndex player, float holdTime, ref float timer, ref bool startTimer);


    // standard inputs
    public Directional move;
    public Directional look;
    public Hold interact1Press;
    public Hold interact1Hold;
    public Press pause;
    public Press interact2Press;
    public Press interact3Press;
    public Press interact4Press;

    // dialogue option selecting
    public Press option1;
    public Press option2;
    public Press option3;
    public Press option4;

    public ControllerType ControllerType { get => controllerType;}










    // Start is called before the first frame update
    void Awake()
    {
        player = PlayerIndex.One;

        if (enableControllerChecking)
            CheckControllerConnected();

        SubscribeInputs();
    }

    private void Update()
    {
        if (startHoldTimer)
        {
            holdTimer += Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeInputs();
    }

    public void ChangeControls(ControllerType inControlType)
    {
        UnsubscribeInputs();

        if (inControlType == ControllerType.Gamepad)
            CheckControllerConnected();
        else
            controllerType = ControllerType.Keyboard;
        
        SubscribeInputs();
    }

    private void TestStuff()
    {
        Vector2 movement = move(player, transform.position);
        Vector2 looking = look(player, transform.position);
        bool press = interact1Press(player, buttonHoldTime, ref holdTimer, ref startHoldTimer);
        bool hold = interact1Hold(player, buttonHoldTime, ref holdTimer, ref startHoldTimer);
        bool pausing = pause(player);
        bool phoning = interact2Press(player);
        bool button1 = option1(player);
        bool button2 = option2(player);
        bool button3 = option3(player);
        bool button4 = option4(player);

        transform.LookAt(new Vector3(transform.position.x + looking.x, transform.position.y, transform.position.z + looking.y));

        Debug.Log
        (
            "\tINTERACT PRESS: " + press + "\tINTERACT HOLD: " + hold +
            "\tPAUSE: " + pausing + "\tMOBILE: " + phoning + "\tOPTION1: " + button1 + "\tOPTION2: " + button2 + "\tOPTION3: " +
            button3 + "\tOPTION4: " + button4
        );

        //if(Input.GetKeyDown(KeyCode.Return))
        //{
        //    ChangeControls(ControllerType.Gamepad);
        //}
    }



    // This function determines if a gamepad is connected and enables gamepad controls if so
    // if it isn't it will enable keyboard controls
    private void CheckControllerConnected()
    {
        bool controllerConnected = false;
        int controllerNumber = 0;

        controllerType = ControllerType.Keyboard;

        while(controllerConnected == false && controllerNumber < 4)
        {
            player = (PlayerIndex)controllerNumber;

            GamePadState gamePadState = GamePad.GetState(player);

            if (gamePadState.IsConnected)
            {
                controllerConnected = true;
                controllerType = ControllerType.Gamepad;
            }

            controllerNumber++;
        }
    }

    // This function subscribes the appropriate control inputs to the delegates depending on the controller type
    private void SubscribeInputs()
    {
        if(controllerType == ControllerType.Gamepad)
        {
            gamepadControls = new GamepadControls(gamePadDeadZone);
            move += gamepadControls.Move;
            look += gamepadControls.Look;
            interact1Press += gamepadControls.InteractPress;
            interact1Hold += gamepadControls.InteractHold;
            pause += gamepadControls.Pause;
            interact2Press += gamepadControls.Interact2;
            interact3Press += gamepadControls.Interact3;
            interact4Press += gamepadControls.Interact4;
            option1 += gamepadControls.Option1;
            option2 += gamepadControls.Option2;
            option3 += gamepadControls.Option3;
            option4 += gamepadControls.Option4;
        }
        else
        {
            keyboardControls = new KeyboardControls();
            move += keyboardControls.Move;
            look += keyboardControls.Look;
            interact1Press += keyboardControls.InteractPress;
            interact1Hold += keyboardControls.InteractHold;
            pause += keyboardControls.Pause;
            interact2Press += keyboardControls.Interact2;
            interact3Press += keyboardControls.Interact3;
            interact4Press += keyboardControls.Interact4;
            option1 += keyboardControls.Option1;
            option2 += keyboardControls.Option2;
            option3 += keyboardControls.Option3;
            option4 += keyboardControls.Option4;
        }
    }

    // This function unsubscribes the appropriate control inputs to the delegates depending on the controller type
    private void UnsubscribeInputs()
    {
        if (controllerType == ControllerType.Gamepad)
        {
            move -= gamepadControls.Move;
            look -= gamepadControls.Look;
            interact1Press -= gamepadControls.InteractPress;
            interact1Hold -= gamepadControls.InteractHold;
            pause -= gamepadControls.Pause;
            interact2Press -= gamepadControls.Interact2;
            interact3Press -= gamepadControls.Interact3;
            interact4Press -= gamepadControls.Interact4;
            option1 -= gamepadControls.Option1;
            option2 -= gamepadControls.Option2;
            option3 -= gamepadControls.Option3;
            option4 -= gamepadControls.Option4;
        }
        else
        {

            move -= keyboardControls.Move;
            look -= keyboardControls.Look;
            interact1Press -= keyboardControls.InteractPress;
            interact1Hold -= keyboardControls.InteractHold;
            pause -= keyboardControls.Pause;
            interact2Press -= keyboardControls.Interact2;
            interact3Press -= keyboardControls.Interact3;
            interact4Press -= keyboardControls.Interact4;
            option1 -= keyboardControls.Option1;
            option2 -= keyboardControls.Option2;
            option3 -= keyboardControls.Option3;
            option4 -= keyboardControls.Option4;
        }
    }



    /*
     *
     *  THE CONTROL CLASSES 
     * 
     */ 

    public interface IControls
    {
        Vector2 Move(PlayerIndex player, Vector3 thePlayerPosition);
        Vector2 Look(PlayerIndex player, Vector3 thePlayerPosition);
        bool InteractPress(PlayerIndex player, float inHoldTime, ref float inHoldTimer, ref bool inStartHoldTimer);
        bool InteractHold(PlayerIndex player, float inHoldTime, ref float inHoldTimer, ref bool inStartHoldTimer);
        bool Interact2(PlayerIndex player);
        bool Interact3(PlayerIndex player);
        bool Interact4(PlayerIndex player);
        bool Pause(PlayerIndex player);
        bool Option1(PlayerIndex player);
        bool Option2(PlayerIndex player);
        bool Option3(PlayerIndex player);
        bool Option4(PlayerIndex player);
    }

    public class KeyboardControls : IControls
    {
        private bool canPress = false;

        public Vector2 Move(PlayerIndex player, Vector3 inPlayerPosition)
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        public Vector2 Look(PlayerIndex player, Vector3 inPlayerPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            Vector2 lookDirection = Vector2.zero;

            if (Physics.Raycast(ray, out hit))
            {
                lookDirection = new Vector2(hit.point.x - inPlayerPosition.x, hit.point.z - inPlayerPosition.z).normalized;
            }

            return lookDirection;
        }

        public bool InteractHold(PlayerIndex player, float inHoldTime, ref float inHoldTimer, ref bool inStartHoldTimer)
        {
            GamePadState state = GamePad.GetState(player);

            float triggerInput = state.Triggers.Right;

            if (Input.GetMouseButton(0))
            {
                inStartHoldTimer = true;
            }
            else
            {
                inStartHoldTimer = false;
                inHoldTimer = 0.0f;
            }

            if (inHoldTimer >= inHoldTime)
            {
                canPress = false;
                return true;
            }

            return false;
        }

        public bool InteractPress(PlayerIndex player, float inHoldTime, ref float inHoldTimer, ref bool inStartHoldTimer)
        {
            GamePadState state = GamePad.GetState(player);

            float triggerInput = state.Triggers.Right;

            if (Input.GetMouseButton(0))
            {
                if (inHoldTimer < inHoldTime)
                {
                    canPress = true;
                }
            }

            if (canPress == true)
            {
                if (!Input.GetMouseButton(0))
                {
                    canPress = false;
                    return true;
                }
            }

            return false;
        }


        public bool Interact2(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }

            return false;
        }

        public bool Interact3(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                return true;
            }

            return false;
        }

        public bool Interact4(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                return true;
            }

            return false;
        }


        public bool Option1(PlayerIndex player)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                return true;
            }

            return false;
        }

        public bool Option2(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return true;
            }

            return false;
        }

        public bool Option3(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                return true;
            }

            return false;
        }

        public bool Option4(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                return true;
            }

            return false;
        }

        public bool Pause(PlayerIndex player)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return true;
            }

            return false;
        }
    }

    public class GamepadControls : IControls
    {
        private float deadZone = 0.1f;

        private bool canPress = false;

        private bool mobileCanPress = false, pauseCanPress = false, option1CanPress = false, option2CanPress = false, option3CanPress = false, option4CanPress = false;

        public GamepadControls(float inDeadZone)
        {
            deadZone = inDeadZone;
        }

        public Vector2 Move(PlayerIndex player, Vector3 inPlayerPosition)
        {
            GamePadState state = GamePad.GetState(player);
            //float x = 0;
            //float y = 0;

            //if (state.ThumbSticks.Left.X < 0) x = -1;
            //else if (state.ThumbSticks.Left.X > 0) x = 1;

            //if (state.ThumbSticks.Left.Y < 0) y = -1;
            //else if (state.ThumbSticks.Left.Y > 0) y = 1;

            return new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        }

        public Vector2 Look(PlayerIndex player, Vector3 inPlayerPosition)
        {
            GamePadState state = GamePad.GetState(player);

            return new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        }

        public bool InteractHold(PlayerIndex player, float inHoldTime, ref float inHoldTimer, ref bool inStartHoldTimer)
        {
            GamePadState state = GamePad.GetState(player);

            float triggerInput = state.Triggers.Right;

            if(triggerInput >= deadZone)
            {
                inStartHoldTimer = true;
            }
            else
            {
                inStartHoldTimer = false;
                inHoldTimer = 0.0f;
            }

            if(inHoldTimer >= inHoldTime)
            {
                canPress = false;
                return true;
            }

            return false;
        }

        public bool InteractPress(PlayerIndex player, float inHoldTime, ref float inHoldTimer, ref bool inStartHoldTimer)
        {
            GamePadState state = GamePad.GetState(player);

            float triggerInput = state.Triggers.Right;

            if(triggerInput >= deadZone)
            {
                if(inHoldTimer < inHoldTime)
                {
                    canPress = true;
                }
            }

            if(canPress == true)
            {
                if(state.Triggers.Right < deadZone)
                {
                    canPress = false;
                    return true;
                }
            }

            return false;
        }

        public bool Interact2(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            float triggerInput = state.Triggers.Left;

            if (triggerInput >= deadZone)
            {
                mobileCanPress = true;
            }

            if(mobileCanPress == true)
            {
                if (triggerInput < deadZone)
                {
                    mobileCanPress = false;
                    return true;
                }    
            }

            return false;
        }

        public bool Interact3(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if (state.Buttons.A == ButtonState.Pressed)
            {
                option1CanPress = true;
            }

            if (option1CanPress == true)
            {
                if (state.Buttons.A == ButtonState.Released)
                {
                    option1CanPress = false;
                    return true;
                }
            }

            return false;

        }

        public bool Interact4(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if (state.Buttons.A == ButtonState.Pressed)
            {
                option1CanPress = true;
            }

            if (option1CanPress == true)
            {
                if (state.Buttons.A == ButtonState.Released)
                {
                    option1CanPress = false;
                    return true;
                }
            }

            return false;

        }

        public bool Option1(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if(state.Buttons.A == ButtonState.Pressed)
            {
                option1CanPress = true;
            }

            if (option1CanPress == true)
            {
                if (state.Buttons.A == ButtonState.Released)
                {
                    option1CanPress = false;
                    return true;
                }
            }

            return false;
           
        }

        public bool Option2(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if (state.Buttons.B == ButtonState.Pressed)
            {
                option2CanPress = true;
            }

            if (option2CanPress == true)
            {
                if (state.Buttons.B == ButtonState.Released)
                {
                    option2CanPress = false;
                    return true;
                }
            }

            return false;
        }

        public bool Option3(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if (state.Buttons.X == ButtonState.Pressed)
            {
                option3CanPress = true;
            }

            if (option3CanPress == true)
            {
                if (state.Buttons.X == ButtonState.Released)
                {
                    option3CanPress = false;
                    return true;
                }
            }

            return false;
        }

        public bool Option4(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if (state.Buttons.Y == ButtonState.Pressed)
            {
                option4CanPress = true;
            }

            if (option4CanPress == true)
            {
                if (state.Buttons.Y == ButtonState.Released)
                {
                    option4CanPress = false;
                    return true;
                }
            }

            return false;
        }

        public bool Pause(PlayerIndex player)
        {
            GamePadState state = GamePad.GetState(player);

            if (state.Buttons.Start == ButtonState.Pressed)
            {
                pauseCanPress = true;
            }

            if (pauseCanPress == true)
            {
                if (state.Buttons.Start == ButtonState.Released)
                {
                    pauseCanPress = false;
                    return true;
                }
            }

            return false;
        }
    }
}
