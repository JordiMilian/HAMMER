using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    public Action OnControllerDetected;
    public Action OnControllerDisconnected;
    public bool isControllerDetected;

    public Action OnRollPressed;
    public Action OnRollUnpressed;
    public Action OnRollPressing;

    public Action OnParryPressed;
    public Action OnAttackPressed;

    public Action OnFocusPressed;
    public Action OnFocusUnpressed;
    public Action OnFocusPressing;

    public Action OnPausePressed;

    public Vector2 MovementDirectionInput;
    public Vector2 LookingDirectionInput;
    public Vector2 MousePosition;
    public Vector2 MouseDirection;

    public Transform PlayerTf;
    public Vector2 PlayerPos;
    Camera mainCamera;
    bool leftTriggerPressed;
    bool rightTriggerPressed;

    public static InputDetector Instance;
    private void Awake()
    {   
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        OnRollPressed += OnRollPressedDebug;
        OnFocusPressed += OnFocusPressedDebug;
        OnPausePressed += OnPausePressedDebug;
        OnAttackPressed += OnAttackPresedDebug;
        mainCamera = Camera.main;
        PlayerTf = GameObject.Find(TagsCollection.MainCharacter).transform;
    }
    void Update()
    {
        CheckForController();
        PlayerPos = PlayerTf.position;

        //CONTROLLER STUFF
        if (isControllerDetected)
        {
            //Roll with B
            if (Input.GetKeyDown(KeyCode.JoystickButton1)) { OnRollPressed?.Invoke(); }
            if (Input.GetKey(KeyCode.JoystickButton1)) { OnRollPressing?.Invoke(); }
            if (Input.GetKeyUp(KeyCode.JoystickButton1)) { OnRollUnpressed?.Invoke(); }

            //Attack with RB
            if (Input.GetKeyDown(KeyCode.JoystickButton5)) { OnAttackPressed?.Invoke(); }

            //Parry with LB
            if(Input.GetKeyDown(KeyCode.JoystickButton4)) { OnParryPressed?.Invoke(); }

            //Pause with Start
            if (Input.GetKeyDown(KeyCode.JoystickButton7)) { OnPausePressed?.Invoke(); }

            //Roll with left trigger
            TriggerInputs(OnRollPressed, OnRollPressing, OnRollUnpressed, "LeftTrigger", ref leftTriggerPressed);

            //Focus with right trigger
            TriggerInputs(OnFocusPressed,OnFocusPressing,OnFocusUnpressed, "RightTrigger", ref rightTriggerPressed);

            //Right joystick direction
            LookingDirectionInput = new Vector2(Input.GetAxisRaw("RightJoystickHorizontal"), Input.GetAxisRaw("RightJoystickVertical"));

            //Left joystick direction
            MovementDirectionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        //KEYBOARD STUFF
        else
        {
            //Roll with shoft or space
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space)) { OnRollPressed?.Invoke(); }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space)) { OnRollPressing?.Invoke(); }
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Space)) { OnRollUnpressed?.Invoke(); }

            //Attack with left mouse
            if (Input.GetKeyDown(KeyCode.Mouse0)){ OnAttackPressed?.Invoke(); }

            //Parry with right Mouse
            if (Input.GetKeyDown(KeyCode.Mouse1)) { OnParryPressed?.Invoke(); }

            //Pause with Esc or P
            if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)){ OnPausePressed?.Invoke(); }

            //Focus with midle mouse
            if (Input.GetKeyDown(KeyCode.Mouse2)){ OnFocusPressed?.Invoke(); }

            //movement with WASD or arrows
            MovementDirectionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //mouse direction
            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            LookingDirectionInput = (MousePosition - PlayerPos).normalized;
        }

        
        Debug.DrawLine(PlayerPos, PlayerPos + MouseDirection);
        
        Debug.DrawLine(PlayerPos, PlayerPos + LookingDirectionInput);

    }
    void TriggerInputs(Action OnPressed, Action OnPressing, Action OnUnpressed, string AxisName, ref bool isPressed)
    {
        if (!isPressed)
        {
            if (Input.GetAxisRaw(AxisName) > .5f)
            {
                OnPressed?.Invoke();
                isPressed = true;
            }
        }
        else if (isPressed)
        {
            if (Input.GetAxisRaw(AxisName) < .5f)
            {
                isPressed = false;
                OnUnpressed?.Invoke();
            }
            OnPressing?.Invoke();
        }
        //Debug.Log(AxisName + Input.GetAxis(AxisName));
    }
    void CheckForController()
    {
        //Go throw all the controllers, depending on if we are currently connected to 1 or not, do diferent things when a controller is found

        string[] joystickNames = Input.GetJoystickNames(); //Get the names of all the controllers

        switch (isControllerDetected)
        {
            case true:
                for (int i = 0; i < joystickNames.Length; i++)
                {
                    if (!string.IsNullOrEmpty(joystickNames[i]))
                    {
                        break; //Found a controller, so break
                    }
                    OnControllerDisconnected?.Invoke();
                    isControllerDetected = false;
                    Debug.Log("Controller LOST: " + i);
                }
                break;

            case false:
                for (int i = 0; i < joystickNames.Length; i++)
                {
                    if (!string.IsNullOrEmpty(joystickNames[i]))
                    {
                        OnControllerDetected?.Invoke();
                        isControllerDetected = true;
                        Debug.Log("Controller FOUND: " + i);
                    }
                }
                break;
        }
    }
    void OnRollPressedDebug()
    {
        Debug.Log(nameof(OnRollPressed));
    }
    void OnFocusPressedDebug()
    {
        Debug.Log(nameof(OnFocusPressed));
    }
    void OnPausePressedDebug()
    {
        Debug.Log(nameof(OnPausePressed));
    }
    void OnAttackPresedDebug()
    {
        Debug.Log(nameof(OnAttackPressed));
    }
}
