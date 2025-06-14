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
    public Action OnSpecialAttackPressed;
    public Action OnSpecialHealPressed;

    public Action OnFocusPressed;
    public Action OnFocusUnpressed;
    public Action OnFocusPressing;

    public Action OnPausePressed;

    public Action OnUpPressed;
    public Action OnDownPressed;
    public Action OnRightPressed;
    public Action OnLeftPressed;

    public Action OnSelectPressed;

    public Vector2 MovementDirectionInput;
    public Vector2 LookingDirectionInput;
    public Vector2 MousePosition;
    public Vector2 MouseDirection;

    public Transform PlayerTf;
    public Vector2 PlayerPos;
    Camera mainCamera;
    bool leftTriggerPressed;
    bool rightTriggerPressed;
    bool madeJoystickMovementInput;

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
        mainCamera = Camera.main;
    }
    void Update()
    {
        CheckForController();
        if(GlobalPlayerReferences.Instance == null) { PlayerTf = Camera.main.transform; }
        else if(GlobalPlayerReferences.Instance.playerTf == null) { PlayerTf = Camera.main.transform; }
        else { PlayerTf = GlobalPlayerReferences.Instance.playerTf; }

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

            //Roll with left trigger TEST
            TriggerInputs(OnRollPressed, OnRollPressing, OnRollUnpressed, "LeftTrigger", ref leftTriggerPressed);

            //Pause with Start
            if (Input.GetKeyDown(KeyCode.JoystickButton7)) { OnPausePressed?.Invoke(); }

           

            //Special attack with right trigger
            TriggerInputs(OnSpecialAttackPressed,OnFocusPressing,OnFocusUnpressed, "RightTrigger", ref rightTriggerPressed);

            //Triangle/Y to Focus
            if (Input.GetKeyDown(KeyCode.JoystickButton3)) { OnFocusPressed?.Invoke(); }

            if (Input.GetKeyDown(KeyCode.JoystickButton2)) { OnSpecialHealPressed?.Invoke(); }

            //Right joystick direction
            LookingDirectionInput = new Vector2(Input.GetAxisRaw("RightJoystickHorizontal"), Input.GetAxisRaw("RightJoystickVertical"));

            //Left joystick direction
            MovementDirectionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //Movement Events
            if (MovementDirectionInput.y > 0.6f)        { if (!madeJoystickMovementInput) { OnUpPressed?.Invoke(); madeJoystickMovementInput = true; } }
            else if (MovementDirectionInput.y < -0.6f)  { if (!madeJoystickMovementInput) { OnDownPressed?.Invoke(); madeJoystickMovementInput = true; } }
            else if (MovementDirectionInput.x > 0.6f)   { if (!madeJoystickMovementInput) { OnRightPressed?.Invoke(); madeJoystickMovementInput = true; } }
            else if (MovementDirectionInput.x < -0.6f)  { if (!madeJoystickMovementInput) { OnLeftPressed?.Invoke(); madeJoystickMovementInput = true; } }
            else { madeJoystickMovementInput = false; }

            //press A to select
            if (Input.GetKeyDown(KeyCode.JoystickButton0)) { OnSelectPressed?.Invoke(); }
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

            //Special Attack with Ctrl
            if(Input.GetKeyDown(KeyCode.LeftControl)) { OnSpecialAttackPressed?.Invoke(); }

            //Special Heal with E
            if (Input.GetKeyDown(KeyCode.E)) { OnSpecialHealPressed?.Invoke(); }

            //movement with WASD or arrows
            MovementDirectionInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //movement actions with WASD
            if (Input.GetKeyDown(KeyCode.W)) { OnUpPressed?.Invoke(); }
            if (Input.GetKeyDown(KeyCode.S)) { OnDownPressed?.Invoke(); }
            if (Input.GetKeyDown(KeyCode.D)) { OnRightPressed?.Invoke(); }
            if (Input.GetKeyDown(KeyCode.A)) { OnLeftPressed?.Invoke(); }

            //Enter to Select
            if (Input.GetKeyDown(KeyCode.Return)){ OnSelectPressed?.Invoke(); }

            //mouse direction
            MousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            LookingDirectionInput = (MousePosition - PlayerPos).normalized;
        }

        
        //Debug.DrawLine(PlayerPos, PlayerPos + MouseDirection);
        
        //Debug.DrawLine(PlayerPos, PlayerPos + LookingDirectionInput);

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
        //Debug.Log(nameof(OnRollPressed));
    }
    void OnFocusPressedDebug()
    {
        //Debug.Log(nameof(OnFocusPressed));
    }
    void OnPausePressedDebug()
    {
        //Debug.Log(nameof(OnPausePressed));
    }
    void OnAttackPresedDebug()
    {
        //Debug.Log(nameof(OnAttackPressed));
    }
}
