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
    [SerializeField] CameraZoomer zoomer;
    [SerializeField] float minZoom;
    [SerializeField] float minDistance;
    [SerializeField] float maxZoom;
    [SerializeField] float maxDistance;
    [Header("references")]
    [SerializeField] Transform MouseFocusTransform;
    [SerializeField] Generic_FlipSpriteWithFocus spriteFliper;
    [SerializeField] CinemachineTargetGroup cinemachineTarget;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Player_EventSystem eventSystem;




    List<GameObject> CurrentEnemies = new List<GameObject>();
    GameObject FocusedEnemy;
    public bool IsFocusingEnemy = false;
    Enemy_EventSystem FocusedEventSystem;

    private void Awake()
    {
        MouseFocusTransform = GameObject.Find("MouseCameraTarget").transform;
        cinemachineTarget = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
    }
    private void OnEnable()
    {
        eventSystem.OnDeath += callOnLookatMouse;
    }
    private void OnDisable()
    {
        eventSystem.OnDeath -= callOnLookatMouse;
    }
    void Start()
    {
        OnLookAtMouse(); 
    }
    void  Update()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.F))
        {
            //Restart enemies list
            CurrentEnemies.Clear();
            CurrentEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            //Unfocus old Enemy if they are not null
            if (FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();

            //Find closest enemy
            FocusedEnemy = ClosestEnemyToMouseInRange(FocusMaxDistance);


            if (FocusedEnemy == null)
            {
                OnLookAtMouse();
            }
            else
            {
                OnLookAtEnemy();
            }
        } 

        if (IsFocusingEnemy == true) { LookingAtEnemy(); }
        else { LookingAtMouse(); }
    }
    GameObject ClosestEnemyToMouseInRange(float range)
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
    void callOnLookatMouse(object sender, EventArgs args)
    {
        if (FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();
        if (IsFocusingEnemy)
        {
            FocusedEnemy = null;
            OnLookAtMouse();
        }
        
    }
    void  OnLookAtMouse()
    {
        
        IsFocusingEnemy = false;
        cinemachineTarget.m_Targets[1].target = MouseFocusTransform;
        cinemachineTarget.m_Targets[1].weight = 1;
        cinemachineTarget.m_Targets[1].radius = 0;
        zoomer.StartFocusOutTransition();

        if (FocusedEventSystem != null) { FocusedEventSystem.OnDeath -= callOnLookatMouse; }
    }
    void OnLookAtEnemy()
    {
       
        FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnFocus();
        FocusedEventSystem = FocusedEnemy.GetComponent<Enemy_EventSystem>();
        FocusedEventSystem.OnDeath += callOnLookatMouse;

        IsFocusingEnemy = true;
        cinemachineTarget.m_Targets[1].target = FocusedEnemy.transform;
        cinemachineTarget.m_Targets[1].weight = 3;
        cinemachineTarget.m_Targets[1].radius = 2;
        zoomer.StartFocusInTransition();
    }
    void LookingAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 PivotVector = transform.position;
        transform.up = (Vector3.RotateTowards(transform.up, mousePos - PivotVector, FollowMouse_Speed * Time.deltaTime, 10f));
        spriteFliper.FocusVector = mousePos;
    }
    void LookingAtEnemy()
    {
        if (FocusedEnemy == null)
        {
            IsFocusingEnemy = false; 
            return;
        }
        Vector2 focusedEnemyVector = FocusedEnemy.transform.position;
        Vector2 playerVector = transform.position;
        transform.up = (Vector3.RotateTowards(transform.up,focusedEnemyVector-playerVector, FollowMouse_Speed * Time.deltaTime, 10f));
        spriteFliper.FocusVector = focusedEnemyVector;
        zoomer.FocusZoom = UpdateZoom();
    }
    float UpdateZoom()
    {
        Vector2 playerPosition = transform.position;
        Vector2 enemyPosition = FocusedEnemy.transform.position;
        float distanceToEnemy = (enemyPosition - playerPosition).magnitude;

        float RelativeDistance = Mathf.InverseLerp(minDistance, maxDistance, distanceToEnemy);
        float CurrentLerpedZoom = Mathf.Lerp(minZoom, maxZoom, RelativeDistance);
        return CurrentLerpedZoom;
    }

}
