using Cinemachine;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class Player_FollowMouse_withFocus : MonoBehaviour
{
    
    public float FollowMouse_Speed = 8f;
    [SerializeField] float FocusMaxDistance;
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
    public Vector2 LookingDirection;


    private void Awake()
    {
        MouseFocusTransform = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
        cinemachineTarget = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        zoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomer>();
        mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        playerRefs.events.OnDeath += unfocusOnDeath;
    }
    private void OnDisable()
    {
        playerRefs.events.OnDeath -= unfocusOnDeath;
    }

    void  Update()
    {
        //Check conditions, depending on which will change the Target to focus attention

        if (distanceToEnemy.Value < 1.5f && !isDistanceToMouse.Value && !IsFocusingEnemy) //Look at closest enemy if everything is alright
        {
            if(ClosestEnemy != null) { PositionToLook = ClosestEnemy.Tf.position; }
        } 
        else if (IsFocusingEnemy == true) //Look at enemy is we are alright
        { 
            zoomer.FocusZoom = UpdateZoom(); PositionToLook = FocusedEnemy.transform.position; 
        } 
        else { PositionToLook = GetMousePosition(); } //Or look at mouse

        LookingAtTarget(PositionToLook);

        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
        {
            AttemptFocus();
        }
    }
    void AttemptFocus()
    {
        //Restart enemies list and find every enemy in scene
        CurrentEnemies.Clear();
        CurrentEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //Unfocus old Enemy if they are not null
        if (FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_EventSystem>().OnUnfocused?.Invoke();

        //Find closest enemy
        FocusedEnemy = ClosestEnemyToMouseInRange(FocusMaxDistance);


        if (FocusedEnemy == null)
        {
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
    Vector2 GetMousePosition()
    {
        return (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
        LookingDirection = (targetPos - playerPos).normalized;
        transform.up = (Vector3.RotateTowards(transform.up, LookingDirection, FollowMouse_Speed * Time.deltaTime, 10f));

        playerRefs.spriteFliper.FocusVector = targetPos;
    }
    GameObject ClosestEnemyToMouseInRange(float range)
    {
        Vector2 mousepos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        List<GameObject> InrangeEnemies = new List<GameObject>();
        List<float> InrangeDistances = new List<float>();

        
        //Add to a list every enemy within range and its distance
        for (int i = 0; i < CurrentEnemies.Count; i++)
        {
            if (Vector2.Distance(mousepos, CurrentEnemies[i].transform.position) < range)
            {
                InrangeEnemies.Add(CurrentEnemies[i]);
                InrangeDistances.Add(Vector2.Distance(mousepos, CurrentEnemies[i].transform.position));
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
