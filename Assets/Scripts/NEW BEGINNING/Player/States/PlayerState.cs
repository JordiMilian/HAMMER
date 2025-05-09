using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected Player_StateMachine stateMachine;
    protected Animator animator;
    protected GameObject rootGameObject;
    protected Player_References playerRefs;
    [SerializeField] protected string AnimatorStateName;

    protected const float transitionTime_short = 0.25f;
    protected const float transitionTime_long = 0.5f;
    protected const float transitionTime_instant = 0f;

    [Header("STAMINA")]
    public bool doesRequireStamina;
    public float StaminaCost;
    public virtual void InitializeState(Player_StateMachine machine, Animator anim)
    {
        stateMachine = machine;
        animator = anim;
        rootGameObject = animator.gameObject;
        playerRefs = rootGameObject.GetComponent<Player_References>();
    }
    public virtual void OnEnable() { subscribeToRequests(); }
    public virtual void OnDisable() { unsubscribeToRequests(); }
    public virtual void Update() { }

    //When implementing this coroutine, remember to stop de coroutine when the state is disabled
    protected IEnumerator AutoTransitionToStateOnAnimationOver(string thisAnimatorStateName, PlayerState stateToChange, float normalizedTransitionDuration)
    {
        animator.CrossFadeInFixedTime(thisAnimatorStateName, normalizedTransitionDuration);
        AnimationClip thisClip = UsefullMethods.GetAnimationClipByStateName(thisAnimatorStateName, animator);
        yield return StartCoroutine(UsefullMethods.WaitForAnimationTime(thisClip));
        stateMachine.ForceChangeState(stateToChange);
    }

    #region ACTION REQUESTS
    //ALL STATES THAT READ INPUT MUST CALL SUBSCRIBE BASE.ONENABLE IT THEY OVERRIDE
    //OVERRIDE ANY REQUEST THAT NEED CHANGE
    protected virtual void subscribeToRequests()
    {
        InputDetector.Instance.OnParryPressed += RequestParry;
        InputDetector.Instance.OnRollPressed += RequestRoll;
        InputDetector.Instance.OnSpecialHealPressed += RequestSpecialHeal;
        InputDetector.Instance.OnSpecialAttackPressed += RequestSpecialAttack;
        InputDetector.Instance.OnAttackPressed += RequestAttack;
        playerRefs.gesturesDetector.OnTapDetected += OnTapDetected;
        playerRefs.gesturesDetector.OnArcDetected += OnArcDetected;

    }
    protected virtual void unsubscribeToRequests()
    {
        InputDetector.Instance.OnParryPressed -= RequestParry;
        InputDetector.Instance.OnRollPressed -= RequestRoll;
        InputDetector.Instance.OnSpecialHealPressed -= RequestSpecialHeal;
        InputDetector.Instance.OnSpecialAttackPressed -= RequestSpecialAttack;
        InputDetector.Instance.OnAttackPressed -= RequestAttack;
        playerRefs.gesturesDetector.OnTapDetected -= OnTapDetected;
        playerRefs.gesturesDetector.OnArcDetected -= OnArcDetected;

    }
    void OnArcDetected( ArcData data)
    {
        //RequestParry();
    }
    void OnTapDetected(TapData data)
    {
        RequestAttack();
    }
    protected virtual void RequestParry() { stateMachine.RequestChangeState(playerRefs.ParryingState); }
    protected virtual void RequestRoll() { stateMachine.RequestChangeState(playerRefs.RollingState); }
    protected virtual void RequestAttack() { stateMachine.RequestChangeState(playerRefs.StartingComboAttackState); }
    protected virtual void RequestSpecialHeal()
    {
        if (Mathf.Approximately(playerRefs.currentStats.CurrentBloodFlow, playerRefs.currentStats.MaxBloodFlow))
        {
            stateMachine.RequestChangeState(playerRefs.SpecialHealState);
        }
    }
    protected virtual void RequestSpecialAttack()
    {
        if (Mathf.Approximately(playerRefs.currentStats.CurrentBloodFlow, playerRefs.currentStats.MaxBloodFlow))
        {
            stateMachine.RequestChangeState(playerRefs.SpecialAttackState);
        }
    }
    #endregion
}
