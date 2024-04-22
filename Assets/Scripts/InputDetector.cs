using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    public Action OnControllerDetected;
    public Action OnControllerDisconnected;
    bool isControllerDetected;

    public Action OnRollPressed;
    public Action OnRollPressing;
    public Action OnParryPressed;
    public Action OnAttackPressed;
    public Action OnFocusPressed;
    public Action OnFocusPressing;
    public Action OnPausePressed;
    public Vector2 MovementDirectionInput;
    public Vector2 SwordDirectionInput;
    public Vector2 MousePosition;
    public Vector2 MouseDirection;
    Transform playerTF;
    Camera mainCamera;
    bool leftTriggerPressed;
    bool rightTriggerPressed;
    private void Awake()
    {
        OnRollPressed += OnRollPressedDebug;
        OnRollPressing += OnRollPressingDebug;
        OnFocusPressed += OnFocusPressedDebug;
        OnPausePressed += OnPausePressedDebug;
        mainCamera = Camera.main;
        playerTF = GameObject.Find(TagsCollection.MainCharacter).transform;
    }
    void Update()
    {
        CheckForController();

        if(isControllerDetected)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton1)) { OnRollPressed?.Invoke(); }

            if (Input.GetKey(KeyCode.JoystickButton1)) { OnRollPressing?.Invoke(); }
            if (Input.GetKey(KeyCode.JoystickButton5)) { OnAttackPressed?.Invoke(); }
            if (Input.GetKey(KeyCode.JoystickButton7)) { OnPausePressed?.Invoke(); }

            TriggerInputs(OnRollPressed, OnRollPressing, "RightTrigger", rightTriggerPressed);
            TriggerInputs(OnFocusPressed,OnFocusPressing,"LeftTrigger",leftTriggerPressed);
            /*
            if(!leftTriggerPressed)
            {
                if (Input.GetAxis("LeftTrigger") > .5f) 
                {
                    OnFocusPressed?.Invoke();
                    leftTriggerPressed = true;
                }
            }
            else if(leftTriggerPressed)
            {
                if (Input.GetAxis("LeftTrigger") < .5f)
                {
                    leftTriggerPressed = false;
                }
            }

            if(!rightTriggerPressed)
            {
                if(Input.GetAxis("RightTrigger") > .5f)
                {
                    OnRollPressed?.Invoke();
                    rightTriggerPressed = true;
                }
            }
            else if(rightTriggerPressed)
            {
                if (Input.GetAxisRaw("RightTrigger") < .5f)
                {
                    rightTriggerPressed = false;
                }
                OnRollPressing?.Invoke();
            }
            */
            SwordDirectionInput = new Vector2(Input.GetAxisRaw("RightJoystickHorizontal"), Input.GetAxisRaw("RightJoystickVertical"));
            MovementDirectionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        
        void TriggerInputs(Action OnPressed, Action OnPressing, string AxisName, bool OnceBool)
        {
            if (!OnceBool)
            {
                if (Input.GetAxisRaw(AxisName) > .5f)
                {
                    OnPressed?.Invoke();
                    OnceBool = true;
                }
            }
            else if (OnceBool)
            {
                if (Input.GetAxisRaw(AxisName) < .5f)
                {
                    OnceBool = false;
                }
                OnPressing?.Invoke();
            }
            Debug.Log(AxisName + Input.GetAxis(AxisName));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space)) { OnRollPressed?.Invoke(); }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space)) { OnRollPressing?.Invoke(); }
        MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = playerTF.position; 
        MouseDirection = ( MousePosition - playerPos).normalized;

        Debug.DrawLine(playerPos, playerPos + MouseDirection);
        
        Debug.DrawLine(playerPos, playerPos + SwordDirectionInput);

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
    void OnRollPressingDebug()
    {
        Debug.Log(nameof(OnRollPressing));
    }
    void OnFocusPressedDebug()
    {
        Debug.Log(nameof(OnFocusPressed));
    }
    void OnPausePressedDebug()
    {
        Debug.Log(nameof(OnPausePressed));
    }
}
