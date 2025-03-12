using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateTags
{
    Unknown, Idle, Agroo, Dead, Attack, Parried, StanceBroken, BossPhaseTransition, BossIntro
}
public abstract class State : MonoBehaviour
{
    public StateTags stateTag = StateTags.Unknown;

    protected Generic_StateMachine stateMachine;
    protected Animator animator;
    protected GameObject rootGameObject;
    public virtual void InitializeState(Generic_StateMachine machine, Animator anim)
    {
        stateMachine = machine;
        animator = anim;
        rootGameObject = animator.gameObject;
    }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void Update() { }
    
}
