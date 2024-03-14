using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;


public class Enemy_AgrooMovement : MonoBehaviour
{
    public float CurrentSpeed;
    float BaseSpeed;
    float SlowSpeedF;

    public float CurrentRotationSpeed;
    float BaseRotationSpeed;
    float SlowRotationSpeed;
    [HideInInspector] public static Transform PlayerTransform;
    [SerializeField] Transform Weapon_Pivot;
    //UI ALERT EN UN SCRIPT APART PERFA
    [SerializeField] Animator UIAnimator;
    [SerializeField] Generic_FlipSpriteWithFocus spriteFliper;
    [SerializeField] Enemy_EventSystem eventSystem;
    [SerializeField] Enemy_MoveToTarget moveToTarget;

    private void Awake()
    {
        BaseRotationSpeed = CurrentRotationSpeed;
        SlowRotationSpeed = CurrentRotationSpeed / 5;
        BaseSpeed = CurrentSpeed;
        SlowSpeedF = BaseSpeed / 3;
    }
    private void OnEnable()
    {
        eventSystem.OnGettingParried += EV_ReturnAllSpeed;
        StartAgroo();
    }
    private void OnDisable()
    {
        eventSystem.OnGettingParried -= EV_ReturnAllSpeed;
        EndAgroo();
    }
    void EndAgroo()
    {
        moveToTarget.Target = null;
    }
    void StartAgroo()
    {
        PlayerTransform = GameObject.Find("MainCharacter").transform;
        UIAnimator.SetTrigger("AgrooAlert");

        moveToTarget.Target = PlayerTransform;
        moveToTarget.DoMove = true;
    }
    void Update()
    {
        spriteFliper.FocusVector = PlayerTransform.transform.position;
        LookAtPlayer();
    }
    
    void LookAtPlayer()
    {
        Vector3 PlayerPos = PlayerTransform.position;
        Vector3 EnemyPos = new Vector3(transform.position.x, transform.position.y);
        float DistanceToPlayer = (EnemyPos - PlayerPos).magnitude;
        Weapon_Pivot.up = (Vector3.RotateTowards(Weapon_Pivot.transform.up, PlayerPos - EnemyPos, CurrentRotationSpeed * Time.deltaTime, 10));
    }

    public void EV_SlowRotationSpeed() { StartCoroutine(ChangeRotation(CurrentRotationSpeed, SlowRotationSpeed, 0.2f)); }
    public void EV_ReturnRotationSpeed() { StartCoroutine(ChangeRotation(CurrentRotationSpeed, BaseRotationSpeed, 0.2f)); }
    public void EV_SlowMovingSpeed() { moveToTarget.Velocity = SlowSpeedF; }
    public void EV_ReturnMovingSpeed() { moveToTarget.Velocity = BaseSpeed; }
                                         
    public void EV_ReturnAllSpeed()
    {
       moveToTarget.Velocity = BaseSpeed;
        StartCoroutine(ChangeRotation(CurrentRotationSpeed, BaseRotationSpeed, 0.2f));
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
}
