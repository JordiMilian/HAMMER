using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;
using Pathfinding;


public class Enemy_AgrooMovement : MonoBehaviour
{
    [SerializeField] Transform enemyTransform;
    public float CurrentSpeed;
    public float BaseSpeed;
    float SlowSpeedF;

    public float CurrentRotationSpeed;
    public float BaseRotationSpeed;
    public float SlowRotationSpeed;
    Transform PlayerTransform;
    [SerializeField] Transform Weapon_Pivot;

    [SerializeField] Animator EnemyAnimator;
    //UI ALERT EN UN SCRIPT APART PERFA
    [SerializeField] Animator UIAnimator;
    [SerializeField] AIDestinationSetter destinationSetter;
    [SerializeField] AIPath aiPath;
    [SerializeField] Generic_FlipSpriteWithFocus spriteFliper;
    [SerializeField] Enemy_EventSystem eventSystem;

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
        destinationSetter.target = null;
    }
    void StartAgroo()
    {
        PlayerTransform = GameObject.Find("MainCharacter").transform;
        UIAnimator.SetTrigger("AgrooAlert");
        EnemyAnimator.SetBool("Walking", true);
        destinationSetter.target = PlayerTransform;
        aiPath.maxSpeed = BaseSpeed;
        CurrentSpeed = BaseSpeed;
        SlowSpeedF = BaseSpeed / 3;
        CurrentRotationSpeed = BaseRotationSpeed;
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
        enemyTransform.up = (Vector3.RotateTowards(Weapon_Pivot.transform.up, PlayerPos - EnemyPos, CurrentRotationSpeed * Time.deltaTime, 10));
    }

    public void EV_SlowRotationSpeed() { StartCoroutine(ChangeRotation(CurrentRotationSpeed, SlowRotationSpeed, 0.2f)); }
    public void EV_ReturnRotationSpeed() { StartCoroutine(ChangeRotation(CurrentRotationSpeed, BaseRotationSpeed, 0.2f)); }
    public void EV_SlowMovingSpeed() { aiPath.maxSpeed = SlowSpeedF; }
    public void EV_ReturnMovingSpeed() { aiPath.maxSpeed = BaseSpeed; }
    public void EV_ReturnAllSpeed()
    {
        aiPath.maxSpeed = BaseSpeed;
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
