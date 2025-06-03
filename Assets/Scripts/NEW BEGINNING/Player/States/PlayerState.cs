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
    protected const float transitionTime_instant = 0.15f;

    [Header("STAMINA")]
    public bool doesRequireStamina;
    public float StaminaCost;
    public virtual void InitializeState(Player_StateMachine machine, Animator anim)
    {
        stateMachine = machine;
        animator = anim;
        rootGameObject = animator.gameObject;
        if (playerRefs == null) { playerRefs = rootGameObject.GetComponent<Player_References>(); }

    }
    public virtual void OnEnable() { subscribeToRequests(); }
    public virtual void OnDisable() { unsubscribeToRequests(); }
    public virtual void Update() { }

    //When implementing this coroutine, remember to stop de coroutine when the state is disabled
    protected IEnumerator AutoTransitionToStateOnAnimationOver(string thisAnimatorStateName, PlayerState stateToChange, float normalizedTransitionDuration)
    {
        animator.CrossFadeInFixedTime(thisAnimatorStateName, normalizedTransitionDuration);

        yield return null; //wait one frame so the transition can start
        AnimatorClipInfo[] nextClips = animator.GetNextAnimatorClipInfo(0);
        if(nextClips.Length > 0)
        {
            AnimationClip nextClip = nextClips[0].clip;
            yield return StartCoroutine(UsefullMethods.WaitForAnimationTime(nextClip));
        }
        else { Debug.LogError("ERROR: No transition clip found"); }
        //AnimationClip thisClip = UsefullMethods.GetAnimationClipByStateName(thisAnimatorStateName, animator);
        
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
    }
    protected virtual void unsubscribeToRequests()
    {
        InputDetector.Instance.OnParryPressed -= RequestParry;
        InputDetector.Instance.OnRollPressed -= RequestRoll;
        InputDetector.Instance.OnSpecialHealPressed -= RequestSpecialHeal;
        InputDetector.Instance.OnSpecialAttackPressed -= RequestSpecialAttack;
        InputDetector.Instance.OnAttackPressed -= RequestAttack;


    }
    /*
    void OnArcDetected( ArcData data)
    {
        //RequestParry();
    }
    protected virtual void OnTapDetected(TapData data)
    {
        //we can make more attacks depending on lenght
        //regular attack
        if (data.tapLenght <= 1.1f)
        {
            playerRefs.GestureAttack_Quick01.GetComponent<IGestureAttack>().gestureDirection = data.endDirection;
            stateMachine.RequestChangeState(playerRefs.GestureAttack_Quick01);
        }
        //strong attack
        else
        {
            playerRefs.GestureAttack_Strong.GetComponent<IGestureAttack>().gestureDirection = data.endDirection;
            stateMachine.RequestChangeState(playerRefs.GestureAttack_Strong);
        }
    }
     protected virtual void RequestStrongAttack() { stateMachine.RequestChangeState(playerRefs.GestureAttack_Strong); }
    */ //GESTURES STUFF
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
