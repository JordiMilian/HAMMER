using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public StateTags stateTag = StateTags.Unknown;

    protected Player_StateMachine stateMachine;
    protected Animator animator;
    protected GameObject rootGameObject;

    [Header("STAMINA")]
    public bool doesRequireStamina;
    public float StaminaCost;
    public virtual void InitializeState(Player_StateMachine machine, Animator anim)
    {
        stateMachine = machine;
        animator = anim;
        rootGameObject = animator.gameObject;
    }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void Update() { }
}
