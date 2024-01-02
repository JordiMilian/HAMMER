using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_FollowPlayer : MonoBehaviour
{
    public float CurrentSpeed;
    public float BaseSpeed;
    private float SlowSpeedF;

    public float CurrentRotationSpeed;
    public float BaseRotationSpeed;
    public float SlowRotationSpeed;

    Transform Player;
    Transform Weapon_Pivot;

    public bool IsAgroo = false;
    bool isAgrooOnce;
    [SerializeField] float ChanceToWalk;
    [SerializeField] float ChanceToChangeDirection;
    [SerializeField] float WalkingSpeed;
    [SerializeField] float WalkingRotationSpeed;
    [SerializeField] float RoamingRadios = 2;
    bool walking = false;

    public Animator EnemyAnimator;


    bool RoamingSpawned = false;
     Transform RoamingCenter;
    Vector2 CurrentRoamingVector;
    //Transform CurrentRoamingTransform;
    Vector2 CurrentTurningTarget;

    AIDestinationSetter destinationSetter;
    AIPath aiPath;
    Generic_FlipSpriteWithFocus spriteFliper;

    public bool doDrawGizmo;

    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        GameObject RoamingGO = new GameObject();
        var roamingCenter = Instantiate(RoamingGO, transform.position, transform.rotation);
        RoamingCenter = roamingCenter.transform;
        RoamingSpawned = true;

        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = BaseSpeed;
        Player = GameObject.Find("MainCharacter").transform;
        SlowSpeedF = BaseSpeed / 3;
        CurrentSpeed = BaseSpeed;
        CurrentRotationSpeed = BaseRotationSpeed;
        
        Weapon_Pivot = FindGameObjectInChildWithTag(gameObject, "Weapon_Pivot").transform;
        InvokeRepeating("DecideWalk", Random.Range(0.5f,2), 2);
        InvokeRepeating("DecideTurn", 0.5f, 1.1f);
        destinationSetter = GetComponent<AIDestinationSetter>();
        spriteFliper = GetComponent<Generic_FlipSpriteWithFocus>();
        spriteFliper.FocusVector = CurrentRoamingVector;
    }

    
    void Update()
    {
        if(IsAgroo)
        {
            if (!isAgrooOnce)
            {
                EnemyAnimator.SetBool("Walking", true);
                destinationSetter.target = Player;
                isAgrooOnce = true;
            }
            spriteFliper.FocusVector = Player.transform.position;
            LookAtPlayer();
        }
        if (!IsAgroo)
        {
            if (walking)
            {
                transform.position = Vector2.MoveTowards(transform.position, CurrentRoamingVector, WalkingSpeed * Time.deltaTime);
                LookingAtRoamingPoint();
            }
            else LookingatRandomPoint();
        } 
    }
    
    private void OnDrawGizmos()
    {
        if (doDrawGizmo)
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            if (!RoamingSpawned) Gizmos.DrawSphere(transform.position, RoamingRadios);
            if (RoamingSpawned) Gizmos.DrawSphere(RoamingCenter.position, RoamingRadios);

            Gizmos.DrawSphere(CurrentRoamingVector, 0.1f);
            Gizmos.DrawSphere(CurrentTurningTarget, 0.2f);
        }
        
    }
    
    void DecideWalk()
    {
        float randomWalk = UnityEngine.Random.Range(0, 100);
        if (randomWalk <= ChanceToWalk) 
        {
            CurrentRoamingVector = Random.insideUnitCircle * RoamingRadios + new Vector2(RoamingCenter.transform.position.x, RoamingCenter.transform.position.y);
            walking = true;
            EnemyAnimator.SetBool("Walking", true);
            spriteFliper.FocusVector = CurrentRoamingVector;
        }
        if (randomWalk > ChanceToWalk) { walking = false; EnemyAnimator.SetBool("Walking", false); }

    }
    void DecideTurn()
    {
        float randomTurn = UnityEngine.Random.Range(0, 100);
        if (randomTurn <= ChanceToChangeDirection)
        {
            CurrentTurningTarget = Random.insideUnitCircle * RoamingRadios + new Vector2(RoamingCenter.transform.position.x, RoamingCenter.transform.position.y);
        }
    }
    void LookingAtRoamingPoint()
    {
        transform.up = Vector3.RotateTowards(Weapon_Pivot.transform.up, CurrentRoamingVector - new Vector2(transform.position.x, transform.position.y), WalkingRotationSpeed * Time.deltaTime, 1);
    }
    void LookingatRandomPoint()
    {
        transform.up = Vector3.RotateTowards(Weapon_Pivot.transform.up, CurrentTurningTarget - new Vector2(transform.position.x, transform.position.y), WalkingRotationSpeed * Time.deltaTime, 1);
    }
   
    static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }

        }
        return null;
    }
    void LookAtPlayer()
    {
        Vector3 PlayerPos = (Vector3)Player.position;
        transform.up = (Vector3.RotateTowards(Weapon_Pivot.transform.up, PlayerPos - new Vector3(transform.position.x, transform.position.y), CurrentRotationSpeed * Time.deltaTime, 10));
    }

    public void EV_SlowRotationSpeed()
    {
       
        StartCoroutine(ChangeRotation(CurrentRotationSpeed, SlowRotationSpeed, 0.4f));
    }
    public void EV_ReturnRotationSpeed()
    {
        StartCoroutine(ChangeRotation(CurrentRotationSpeed, BaseRotationSpeed, 0.4f));
    }
    public void EV_SlowMovingSpeed() 
    {
        aiPath.maxSpeed = SlowSpeedF;
    }
    public void EV_ReturnMovingSpeed()
    {
        aiPath.maxSpeed = BaseSpeed;
    }
    IEnumerator ChangeRotation(float v_start, float v_end, float duration)
    {

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            CurrentRotationSpeed = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        CurrentRotationSpeed = v_end;
    }
    IEnumerator ChangeWalkingSpeed(float v_start, float v_end, float duration)
    {

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            WalkingSpeed = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        WalkingSpeed = v_end;
    }
}
