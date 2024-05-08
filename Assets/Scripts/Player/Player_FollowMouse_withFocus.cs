using Cinemachine;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class Player_FollowMouse_withFocus : MonoBehaviour
{
    
    public float FollowMouse_Speed_Keyboard = 8f;
    public float FollowMouse_Speed_Controller = 8f;
    [SerializeField] float FocusMaxDistance_Keyboard;
    [SerializeField] Vector2 FocusMinMaxDistance_Controller;
    [SerializeField] float FocusDistanceWithController;
    [Header("zoomer")]
    CameraZoomController zoomer;

    [Header("references")]
    Transform MouseFocusTransform;
    [SerializeField] Player_References playerRefs;

    CinemachineTargetGroup cinemachineTarget;
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] BoolVariable isDistanceToMouse;
    [SerializeField] TransformVariable ClosestEnemy;

    List<GameObject> CurrentEnemies = new List<GameObject>();
    [HideInInspector] public GameObject FocusedEnemy;
    [HideInInspector] public Vector2 PositionToLook;
    public bool IsFocusingEnemy = false;

    public Vector2 SwordDirection;
    Vector2 lastValidDirection;
    InputDetector inputDetector;
    bool attemptedJoystickRefocus;

    private void Awake()
    {
        MouseFocusTransform = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
        cinemachineTarget = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        zoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>();
        inputDetector = InputDetector.Instance;
    }
    private void OnEnable()
    {
        playerRefs.events.OnDeath += unfocusOnDeath;
        inputDetector.OnFocusPressed += CallNormalFocusAttempt;
    }
    private void OnDisable()
    {
        playerRefs.events.OnDeath -= unfocusOnDeath;
        inputDetector.OnFocusPressed -= CallNormalFocusAttempt;
    }

    void  Update()
    {
        //Check conditions, depending on which will change the Target to focus attention

        if (distanceToEnemy.Value < 2.5f && !isDistanceToMouse.Value && !IsFocusingEnemy) //Look at closest enemy if everything is alright
        {
            if(ClosestEnemy != null) { PositionToLook = ClosestEnemy.Tf.position; }
        } 
        else if (IsFocusingEnemy == true) //Look at enemy if focusing enemy
        { 
            PositionToLook = FocusedEnemy.transform.position; 
        }
        else //Or look at mouse
        {
            if (inputDetector.LookingDirectionInput.sqrMagnitude < 0.5f) //if not enough input from joystick, look at last valid
            {
                PositionToLook = inputDetector.PlayerPos + lastValidDirection;
            }
            else
            {
                PositionToLook = GetLookingDirection();
                lastValidDirection = inputDetector.LookingDirectionInput;
            }
        } 

        LookingAtTarget(PositionToLook); //MAIN METHOD

        //DARKSOULS Joystick focus system
        DarkSoulsFocus();
    }
    void DarkSoulsFocus()
    {
        if (inputDetector.LookingDirectionInput.sqrMagnitude > .8f && IsFocusingEnemy && inputDetector.isControllerDetected)
        {
            if (!attemptedJoystickRefocus)
            {
                AttemptFocus(true, false);
                attemptedJoystickRefocus = true;
            }
        }
        else
        {
            if (attemptedJoystickRefocus) { attemptedJoystickRefocus = false; }
        }
    }
    void CallDeathFocusAttempt(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        AttemptFocus(false, true);
    }
    void CallNormalFocusAttempt()
    {
        AttemptFocus(false, false);
    }
    void AttemptFocus(bool isDarksoulsFocus, bool isDeathFocus)
    {
        //Restart enemies list and find every enemy in scene
        CurrentEnemies.Clear();
        CurrentEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //Get the currently focused enemy, for later uses
        GameObject PreviuslyFocusedEnemy = FocusedEnemy;

        //Unsubscribe to old Enemy if they are not null and Unfocus icon
        if (FocusedEnemy != null) { UnsubscribeToEnemy(FocusedEnemy); }

        //Decide where to put the searcher:
            //If Death Focus, make a big focus around the dead enemy
            //If controller detected, the center of search will be slighly forward of the sword 
            //If in keyboard, the center will be the mouse position
        if(isDeathFocus)
        {
            DeathFocus(PreviuslyFocusedEnemy);
        }
        else if (inputDetector.isControllerDetected) 
        {
            ControllerFocus(PreviuslyFocusedEnemy, isDarksoulsFocus);
        }
        else 
        {
            KeyboardFocus();
        }

        //If couldnt find enemy or its the same enemy as before, unfocus (unless its darksouls joystick focus).
        //when using darksouls joystick focus, if couldnt find an enemy, focus to previusly focused enemy
        //Found a valid enemy
        if (FocusedEnemy == null && !isDarksoulsFocus || FocusedEnemy == PreviuslyFocusedEnemy && !isDarksoulsFocus) 
        {
            FocusedEnemy = null;
            if (!IsFocusingEnemy) { return; } //couldnt find any enemy but we were already not focusing
            OnLookAtMouse(PreviuslyFocusedEnemy);
        }
        else if(FocusedEnemy == null && isDarksoulsFocus)
        {
            FocusedEnemy = PreviuslyFocusedEnemy;
            OnLookAtEnemy();
        }
        else 
        {
            OnLookAtEnemy();
        }
    }
    void DeathFocus(GameObject previusEnemy)
    {
        FocusedEnemy = ClosestEnemyToCenterWithinRange(previusEnemy.transform.position, 5, previusEnemy);
        if( FocusedEnemy == null) { return; }
        if (FocusedEnemy == previusEnemy) { FocusedEnemy = null; }
    }
    void ControllerFocus(GameObject previusenemy, bool isDarkSouls)
    {
        //This is deprecated for now, all the focus attempts are now the max size
        Vector2 playerPos = transform.position;
        float lookingX = inputDetector.LookingDirectionInput.x;
        float absoluteX = Mathf.Abs(lookingX);
        float equivalentRadius = UsefullMethods.equivalentFromAtoB(0, 1, FocusMinMaxDistance_Controller.x, FocusMinMaxDistance_Controller.y, absoluteX);

        //If in darksouls mode, use the focused enemy as the center to get the direction
        //If in regular mode (pressing focus button) use the player as center
        Vector2 centerOfDetection_C = Vector2.zero;
        if (isDarkSouls)
        {
            Vector2 enemyPos = previusenemy.transform.position;
            centerOfDetection_C = enemyPos + inputDetector.LookingDirectionInput * equivalentRadius;
        }
        else 
        { 
            //centerOfDetection_C = playerPos + lastValidDirection * FocusMinMaxDistance_Controller.y * 0.7f;
            centerOfDetection_C = Camera.main.transform.position;
        }

        FocusedEnemy = ClosestEnemyToCenterWithinRange(centerOfDetection_C, FocusMinMaxDistance_Controller.y);
    }
    void KeyboardFocus()
    {
        Vector2 centerOfDetection_K = inputDetector.MousePosition;
        FocusedEnemy = ClosestEnemyToCenterWithinRange(centerOfDetection_K, FocusMaxDistance_Keyboard);
    }
    void UnsubscribeToEnemy(GameObject unsubscribed)
    {
        Enemy_EventSystem unsubscribedEvents = unsubscribed.GetComponent<Enemy_EventSystem>();
        unsubscribedEvents.OnUnfocused?.Invoke();
        unsubscribedEvents.OnDeath -= CallDeathFocusAttempt;
    }
    void SubscribeToEnemy(GameObject subscribed)
    {
        Enemy_EventSystem subscribedEvents = subscribed.GetComponent<Enemy_EventSystem>();
        subscribedEvents.OnFocused?.Invoke();
        subscribedEvents.OnDeath += CallDeathFocusAttempt;
    }
    //Deprecated seems
    /*
    void callOnLookatMouse(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        if (FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();
        if (IsFocusingEnemy)
        {
            FocusedEnemy = null;
            OnLookAtMouse();
        }
    }
    */
    public void OnLookAtMouse(GameObject UnfocusedEnemy)
    {
        if (!IsFocusingEnemy) { return; } //Just in case pero no fa res crec

        //Unsubscribe to everything
        if(UnfocusedEnemy != null) { UnsubscribeToEnemy(UnfocusedEnemy); }

        cinemachineTarget.m_Targets[1].target = MouseFocusTransform;
        cinemachineTarget.m_Targets[1].weight = 1;
        cinemachineTarget.m_Targets[1].radius = 0;

        zoomer.onUnfocusedEnemy(); //Avisar al zoom controller que ja no tenim focus

        //if (FocusedEventSystem != null) { FocusedEventSystem.OnDeath -= callOnLookatMouse; } //Desubscribe to unfocused enemies Death

        playerRefs.events.OnUnfocusEnemy?.Invoke(); //For tutorial room

        IsFocusingEnemy = false;
    }
    void OnLookAtEnemy()
    {
        //Subscribe to stuff so it stops focusing if enemy dies

        SubscribeToEnemy(FocusedEnemy);

        IsFocusingEnemy = true;
        cinemachineTarget.m_Targets[1].target = FocusedEnemy.transform;
        cinemachineTarget.m_Targets[1].weight = 3;
        cinemachineTarget.m_Targets[1].radius = 2;

        zoomer.onFocusedEnemy(); //Avisar al zoomer que tenim un focus

        playerRefs.events.OnFocusEnemy?.Invoke(); //Currently just used for Tutorial Rooms
    }
    Vector2 GetLookingDirection()
    {
        Vector2 direction = inputDetector.PlayerPos + inputDetector.LookingDirectionInput;
        return direction;
    }
    void LookingAtTarget(Vector2 targetPos)
    {
        Vector2 playerPos = transform.position;
        SwordDirection = (targetPos - playerPos).normalized;

        //Change rotation speed depending on controller or kboard
        float actualSpeed = FollowMouse_Speed_Controller;
        if(!InputDetector.Instance.isControllerDetected) { actualSpeed = FollowMouse_Speed_Keyboard; }
        transform.up = (Vector3.RotateTowards(transform.up, SwordDirection, actualSpeed * Time.deltaTime, 10f));

        playerRefs.spriteFliper.FocusVector = targetPos;
    }
    GameObject ClosestEnemyToCenterWithinRange(Vector2 center, float range, GameObject IgnoredEnemy = null)
    {

        List<GameObject> InrangeEnemies = new List<GameObject>();
        List<float> InrangeDistances = new List<float>();

        StartCoroutine(DrawAttemptDebug(center, range, 2f));
        
        //Add to a list every enemy within range and its distance
        for (int i = 0; i < CurrentEnemies.Count; i++)
        {
            if (IgnoredEnemy != null && CurrentEnemies[i] == IgnoredEnemy) { continue; } //Ignore enemy to ignore if not null
            if (Vector2.Distance(center, CurrentEnemies[i].transform.position) < range)
            {
                InrangeEnemies.Add(CurrentEnemies[i]);
                InrangeDistances.Add(Vector2.Distance(center, CurrentEnemies[i].transform.position));
            }
        }
        //If no enemies in range
        if (InrangeEnemies.Count == 0)
        {
            return null;
        }

        //Check which index has the shortest distance and return
        int minIndex = 0;
        for (int o = 0; o < InrangeDistances.Count; o++)
        {
            if (InrangeDistances[o] < InrangeDistances[minIndex])
            {
                minIndex = o;
            }
        }
        return InrangeEnemies[minIndex];
    }
    
    //When player dies
    void unfocusOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        if(IsFocusingEnemy)
        {
            TargetGroupSingleton.Instance.RemoveTarget(FocusedEnemy.transform);
            FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();
            FocusedEnemy = null;
            zoomer.onUnfocusedEnemy();
            IsFocusingEnemy = false;
            //zoomer.CheckLatestZoomInfoAndChange();
        }
    }
    IEnumerator DrawAttemptDebug(Vector2 center, float radius, float time)
    {
        float timer = 0;
        while (timer < time)
        {
            timer += Time.deltaTime;
            UsefullMethods.DrawPolygon(center, 10, radius);
            yield return null;
        }
    }
}
