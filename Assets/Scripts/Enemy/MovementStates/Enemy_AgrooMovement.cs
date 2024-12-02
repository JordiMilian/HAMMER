using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;


public class Enemy_AgrooMovement : MonoBehaviour
{

    float SlowSpeedF;

    public float CurrentRotationSpeed;
    float BaseRotationSpeed;
    float SlowRotationSpeed;
    Transform PlayerTransform;
    [SerializeField] Transform Weapon_Pivot;
    //UI ALERT EN UN SCRIPT APART PERFA
    [SerializeField] Animator UIAnimator;
    [SerializeField] Enemy_References enemyRefs;
    public bool DoLooking;

    private void Awake()
    {
        BaseRotationSpeed = CurrentRotationSpeed;
        SlowRotationSpeed = CurrentRotationSpeed / 5;

        SlowSpeedF = enemyRefs.currentEnemyStats.BaseSpeed / 4;
    }
    private void OnEnable()
    {
        enemyRefs.enemyEvents.OnGettingParried += (int i)=>EV_SlowMovingSpeed();
        enemyRefs.enemyEvents.OnEnterIdle += returnSpeedOnIdle;
        StartAgroo();
    }
    private void OnDisable()
    {
        enemyRefs.enemyEvents.OnGettingParried -= (int i) => EV_SlowMovingSpeed();
        enemyRefs.enemyEvents.OnEnterIdle -= returnSpeedOnIdle;
        EndAgroo();
    }
    void EndAgroo()
    {
        enemyRefs.moveToTarget.Target = null;
    }
    void StartAgroo()
    {
        PlayerTransform = GlobalPlayerReferences.Instance.playerTf;
        UIAnimator.SetTrigger("AgrooAlert");

        enemyRefs.moveToTarget.Target = PlayerTransform;
        enemyRefs.moveToTarget.DoMove = true;
    }
    void Update()
    {
        enemyRefs.spriteFliper.FocusVector = PlayerTransform.transform.position;
        if (DoLooking) { LookAtPlayer(); }
        
    }
    
    void LookAtPlayer()
    {
        Vector3 PlayerPos = PlayerTransform.position;
        Vector3 EnemyPos = new Vector3(transform.position.x, transform.position.y);

        Vector3 rotateTowardsVector = Vector3.RotateTowards(Weapon_Pivot.transform.up, PlayerPos - EnemyPos, CurrentRotationSpeed * Time.deltaTime, 10);

        Vector3 planeposition = (Vector3.ProjectOnPlane(rotateTowardsVector, Vector3.forward)).normalized;
        Weapon_Pivot.up = planeposition;
    }

    public void EV_SlowRotationSpeed() { StartCoroutine(ChangeRotation(CurrentRotationSpeed, SlowRotationSpeed, 0.1f)); enemyRefs.spriteFliper.canFlip = false; }
    public void EV_ReturnRotationSpeed() { StartCoroutine(ChangeRotation(CurrentRotationSpeed, BaseRotationSpeed, 0.1f)); enemyRefs.spriteFliper.canFlip = true; }
    public void EV_SlowMovingSpeed() { enemyRefs.currentEnemyStats.Speed = SlowSpeedF; }
    public void EV_ReturnMovingSpeed() { enemyRefs.currentEnemyStats.Speed = enemyRefs.currentEnemyStats.BaseSpeed; }
                                         
    public void EV_ReturnAllSpeed(int i)
    {
        EV_ReturnMovingSpeed();
        EV_ReturnRotationSpeed();
    }
    void returnSpeedOnIdle()
    {
        EV_ReturnAllSpeed(0);
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
