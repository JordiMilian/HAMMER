using Cinemachine;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class Player_FollowMouse_withFocus : MonoBehaviour
{
    
    public float FollowMouse_Speed_Keyboard = 8f;
    public float FollowMouse_Speed_Controller = 8f;
    [SerializeField] float FocusMaxDistance_Keyboard;
    [SerializeField] Vector2 FocusMinMaxDistance_Controller;
    [SerializeField] float FocusDistanceWithController;
    [Header("zoomer")]
    CameraZoomer zoomer;
    [SerializeField] float minZoom;
    [SerializeField] float minDistance;
    [SerializeField] float maxZoom;
    [SerializeField] float maxDistance;
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
    Enemy_EventSystem FocusedEventSystem;
    Camera mainCamera;
    public Vector2 SwordDirection;
    Vector2 lastValidDirection;
    InputDetector inputDetector;

    private void Awake()
    {
        MouseFocusTransform = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
        cinemachineTarget = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        zoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomer>();
        mainCamera = Camera.main;
        inputDetector = InputDetector.Instance;
    }
    private void OnEnable()
    {
        playerRefs.events.OnDeath += unfocusOnDeath;
        inputDetector.OnFocusPressed += AttemptFocus;
    }
    private void OnDisable()
    {
        playerRefs.events.OnDeath -= unfocusOnDeath;
        inputDetector.OnFocusPressed -= AttemptFocus;
    }

    void  Update()
    {
        //Check conditions, depending on which will change the Target to focus attention

        if (distanceToEnemy.Value < 1.5f && !isDistanceToMouse.Value && !IsFocusingEnemy) //Look at closest enemy if everything is alright
        {
            if(ClosestEnemy != null) { PositionToLook = ClosestEnemy.Tf.position; }
        } 
        else if (IsFocusingEnemy == true) //Look at enemy if focusing enemy
        { 
            zoomer.FocusZoom = UpdateZoom(); PositionToLook = FocusedEnemy.transform.position; 
        }
        else //Or look at mouse
        {
            
            if (inputDetector.LookingDirectionInput.sqrMagnitude < 0.5f)
            {
                PositionToLook = inputDetector.PlayerPos + lastValidDirection;
            }
            else
            {
                PositionToLook = GetLookingDirection();
                lastValidDirection = inputDetector.LookingDirectionInput;
            }
        } 

        LookingAtTarget(PositionToLook);

        if(inputDetector.LookingDirectionInput.sqrMagnitude > .7f)
        {
            //TO DO CAMBIAR FOCUS TIPO DARK SOULS
        }
    }
    void AttemptFocus()
    {
        //Restart enemies list and find every enemy in scene
        CurrentEnemies.Clear();
        CurrentEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //Get the currently focused enemy, if the new focuse enemy is the same, unfocus
        GameObject PreviuslyFocusedEnemy = FocusedEnemy;

        //Unfocus old Enemy if they are not null
        if (FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_EventSystem>().OnUnfocused?.Invoke();

        //Find closest enemy
            //If controller detected, the center of search will be slighly forward of the sword
            //If in keyboard, the center will be the mouse position
        if (inputDetector.isControllerDetected) 
        {
            Vector2 playerPos = transform.position;
            float lookingX = inputDetector.LookingDirectionInput.x;
            float absoluteX = Mathf.Abs(lookingX);
            float equivalentRadius = UsefullMethods.equivalentFromAtoB(0, 1, FocusMinMaxDistance_Controller.x, FocusMinMaxDistance_Controller.y, absoluteX);

            Vector2 centerOfDetection_C = playerPos + inputDetector.LookingDirectionInput * equivalentRadius; //use the radius as distance lets see 
            FocusedEnemy = ClosestEnemyToMouseInRange(centerOfDetection_C, equivalentRadius);
        }
        else 
        {
            Vector2 centerOfDetection_K = inputDetector.MousePosition;
            FocusedEnemy = ClosestEnemyToMouseInRange(centerOfDetection_K, FocusMaxDistance_Keyboard);
        }

        //If couldnt find enemy or its the same enemy as before, unfocus
        if (FocusedEnemy == null || FocusedEnemy == PreviuslyFocusedEnemy) 
        {
            FocusedEnemy = null;
            if (!IsFocusingEnemy) { return; } //couldnt find any enemy but we were already not focusing
            OnLookAtMouse();
        }
        else
        {
            OnLookAtEnemy();
        }
    }
    void callOnLookatMouse(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        if (FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();
        if (IsFocusingEnemy)
        {
            FocusedEnemy = null;
            OnLookAtMouse();
        }

    }
    void OnLookAtMouse()
    {
        IsFocusingEnemy = false;

        cinemachineTarget.m_Targets[1].target = MouseFocusTransform;
        cinemachineTarget.m_Targets[1].weight = 1;
        cinemachineTarget.m_Targets[1].radius = 0;

        zoomer.StartFocusOutTransition();

        if (FocusedEventSystem != null) { FocusedEventSystem.OnDeath -= callOnLookatMouse; } //Desubscribe to unfocused enemies Death

        playerRefs.events.OnUnfocusEnemy?.Invoke();
    }
    void OnLookAtEnemy()
    {
        //Subscribe to stuff so it stops focusing if enemy dies
        FocusedEnemy.GetComponent<Enemy_EventSystem>().OnFocused?.Invoke();
        FocusedEventSystem = FocusedEnemy.GetComponent<Enemy_EventSystem>();
        FocusedEventSystem.OnDeath += callOnLookatMouse;

        IsFocusingEnemy = true;
        cinemachineTarget.m_Targets[1].target = FocusedEnemy.transform;
        cinemachineTarget.m_Targets[1].weight = 3;
        cinemachineTarget.m_Targets[1].radius = 2;
        zoomer.StartFocusInTransition();

        playerRefs.events.OnFocusEnemy?.Invoke();
    }
    Vector2 GetLookingDirection()
    {
        Vector2 direction = inputDetector.PlayerPos + inputDetector.LookingDirectionInput;
        return direction;
    }
    void GetFocusedEnemyPosition() //deprecated?? maybe
    {
        if (FocusedEnemy == null)
        {
            IsFocusingEnemy = false;
            return;
        }
        Vector2 focusedEnemyPosition = FocusedEnemy.transform.position;
        LookingAtTarget(focusedEnemyPosition);
        zoomer.FocusZoom = UpdateZoom();
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
    GameObject ClosestEnemyToMouseInRange(Vector2 center, float range)
    {

        List<GameObject> InrangeEnemies = new List<GameObject>();
        List<float> InrangeDistances = new List<float>();
        StartCoroutine(DrawAttemptDebug(center, range, 2f));
        
        //Add to a list every enemy within range and its distance
        for (int i = 0; i < CurrentEnemies.Count; i++)
        {
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
    IEnumerator DrawAttemptDebug(Vector2 center, float radius, float time)
    {
        float timer = 0;
        while (timer < time )
        {
            timer += Time.deltaTime;
            UsefullMethods.DrawPolygon(center, 10, radius);
            yield return null;
        }
    }
    //When player dies
    void unfocusOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        if(IsFocusingEnemy)
        {
            TargetGroupSingleton.Instance.RemoveTarget(FocusedEnemy.transform);
            FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();
            FocusedEnemy = null;
            IsFocusingEnemy = false;
            zoomer.UpdateNewCoroutine();
        }
    }

    float UpdateZoom()
    {
        if(FocusedEnemy == null) { return 0.5f; }
        Vector2 playerPosition = transform.position;
        Vector2 enemyPosition = FocusedEnemy.transform.position;
        float distanceToEnemy = (enemyPosition - playerPosition).magnitude;

        float NormalizedDistance = Mathf.InverseLerp(minDistance, maxDistance, distanceToEnemy);
        float relativeZoom = Mathf.Lerp(minZoom, maxZoom, NormalizedDistance);
        return relativeZoom;
    }


}
