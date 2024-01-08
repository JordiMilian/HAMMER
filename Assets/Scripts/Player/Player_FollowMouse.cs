using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
//using System.Diagnostics;
using UnityEngine;

public class Player_FollowMouse : MonoBehaviour
{

    public float FollowMouse_Speed = 8f;
    Player_FeedbackManager Player;
    GameObject FocusedEnemy;
    Transform CameraFocusTransform;
    bool IsFocusingEnemy;
    [SerializeField] float FocusMaxDistance;

    GameObject PlayerGO;
    [SerializeField] GameObject BodySprite;
    bool FlipOnce;

    [SerializeField] Generic_FlipSpriteWithFocus spriteFliper;


    List<GameObject> CurrentEnemies = new List<GameObject>();


    CinemachineTargetGroup cinemachineTarget;
    void Start()
    {

        cinemachineTarget = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        Player = GetComponentInParent<Player_FeedbackManager>();
        PlayerGO = Player.gameObject;
        CameraFocusTransform = GameObject.Find("CameraController").transform;
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            CurrentEnemies.Clear();
            CurrentEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

            if(FocusedEnemy != null) FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnUnfocus();

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
        if (FocusedEnemy == null)
        {
            OnLookAtMouse();
        }

        if (IsFocusingEnemy == true) { LookingAtEnemy(); }
        else { LookingAtMouse(); }
    }
    GameObject ClosestEnemyToMouseInRange(float range)
    {
        Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int u = 0;

        List<GameObject> InrangeEnemies = new List<GameObject>();
        List<float> InrangeDistances = new List<float>();

        int minIndex = 0;

        for (int i = 0; i < CurrentEnemies.Count; i++)
        {
            if (Vector2.Distance(mousepos, CurrentEnemies[i].transform.position) < range)
            {
                InrangeEnemies.Add(CurrentEnemies[i]);
                InrangeDistances.Add(Vector2.Distance(mousepos, CurrentEnemies[i].transform.position));
                u++;
            }
        }
        if (InrangeEnemies.Count == 0)
        {
            return null;
        }
        for (int o = 0; o < InrangeDistances.Count; o++)
        {
            if (InrangeDistances[o] < InrangeDistances[minIndex])
            {
                minIndex = o;
            }
        }
        return InrangeEnemies[minIndex];

    }
    void OnLookAtMouse()
    {
       
        IsFocusingEnemy = false;
        cinemachineTarget.m_Targets[1].target = CameraFocusTransform;
        cinemachineTarget.m_Targets[1].weight = 1;

        
    }
    void OnLookAtEnemy()
    {
       
        FocusedEnemy.GetComponent<Enemy_FocusIcon>().OnFocus();

        IsFocusingEnemy = true;
        cinemachineTarget.m_Targets[1].target = FocusedEnemy.transform;
        cinemachineTarget.m_Targets[1].weight = 2;

        
    }
    void LookingAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3.RotateTowards(transform.up, mousePos - new Vector2(transform.position.x, transform.position.y), FollowMouse_Speed*Time.deltaTime, 10f));

        
        spriteFliper.FocusVector = mousePos;
    }
    void LookingAtEnemy()
    {
       
        transform.up = (Vector3.RotateTowards(transform.up, new Vector2(FocusedEnemy.transform.position.x, FocusedEnemy.transform.position.y) - new Vector2(transform.position.x, transform.position.y), FollowMouse_Speed, 10f));

        
        spriteFliper.FocusVector = FocusedEnemy.transform.position;

    }

}
