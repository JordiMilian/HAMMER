using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;
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
    [SerializeField] float ChanceToWalk;
    [SerializeField] float ChanceToChangeDirection;
    [SerializeField] float WalkingSpeed;
    [SerializeField] float WalkingRotationSpeed;
    [SerializeField] float RoamingRadios = 2;
    bool walking = false;

    public Transform RoamingCenter;
    Vector2 CurrentRoamingTarget;
    Vector2 CurrentTurningTarget;
    


    void Start()
    {
        Player = GameObject.Find("MainCharacter").transform;
        SlowSpeedF = BaseSpeed / 3;
        CurrentSpeed = BaseSpeed;
        CurrentRotationSpeed = BaseRotationSpeed;
        Weapon_Pivot = FindGameObjectInChildWithTag(gameObject, "Weapon_Pivot").transform;
        InvokeRepeating("DecideWalk", Random.Range(0.5f,2), 2);
        InvokeRepeating("DecideTurn", 0.5f, 1.1f);
    }

    
    void Update()
    {
        if(IsAgroo)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, CurrentSpeed * Time.deltaTime);
            LookAtPlayer();
        }
        if (IsAgroo == false)
        {
            if (walking == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, CurrentRoamingTarget, WalkingSpeed * Time.deltaTime);
                LookAtRoamingPoint();
            }
            else LookatRandomPoint();
        }
      
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(RoamingCenter.position, RoamingRadios);
        Gizmos.DrawSphere(CurrentRoamingTarget, 0.1f);
        Gizmos.DrawSphere(CurrentTurningTarget, 0.2f);
    }
    void DecideWalk()
    {
        float randomWalk = UnityEngine.Random.Range(0, 100);
        if (randomWalk <= ChanceToWalk) 
        {
            CurrentRoamingTarget = Random.insideUnitCircle * RoamingRadios + new Vector2(RoamingCenter.transform.position.x, RoamingCenter.transform.position.y);
            walking = true;
        }
        if (randomWalk > ChanceToWalk) { walking = false; }

    }
    void DecideTurn()
    {
        float randomTurn = UnityEngine.Random.Range(0, 100);
        if (randomTurn <= ChanceToChangeDirection)
        {
            CurrentTurningTarget = Random.insideUnitCircle * RoamingRadios + new Vector2(RoamingCenter.transform.position.x, RoamingCenter.transform.position.y);
        }
    }
    void LookAtRoamingPoint()
    {
        transform.up = (Vector3.RotateTowards(Weapon_Pivot.transform.up, CurrentRoamingTarget - new Vector2(transform.position.x, transform.position.y), WalkingRotationSpeed * Time.deltaTime, 10));
    }
    void LookatRandomPoint()
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

    public void SlowSpeed()
    {
        CurrentSpeed = SlowSpeedF;
        StartCoroutine(ChangeRotation(CurrentRotationSpeed, SlowRotationSpeed, 0.4f));
    }
    public void ReturnSpeed()
    {
        StartCoroutine(ChangeRotation(CurrentRotationSpeed, BaseRotationSpeed, 0.4f));
        CurrentSpeed = BaseSpeed;
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
