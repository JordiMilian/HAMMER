using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VFX;

public class TutorialPole_StateMachine : Generic_StateMachine
{
    [SerializeField] List<SpriteRenderer> SpritesToHide = new List<SpriteRenderer>();
    [SerializeField] SpriteRenderer FinePole;
    [SerializeField] SpriteRenderer BrokenPole;
    [SerializeField] Animator TutorialAnimator;
    [SerializeField] Dialoguer dialoguer;
    [SerializeField] VisualEffect breakEffect;

    private void OnEnable()
    {
        eventSystem.OnReceiveDamage += ReceiveDamageFeedback;
        eventSystem.OnDeath += OnDeathState;
    }

    void ReceiveDamageFeedback(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        TutorialAnimator.SetTrigger("Hurt");
        breakEffect.SetFloat("normalizedIntensity", 0.35f);
        breakEffect.Play();
    }
    public override void OnDeathState(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        CurrentState = States.Dead;
        dialoguer.enabled = false; //Make something fun with this pls
        breakEffect.SetFloat("normalizedIntensity", 1.2f);
        breakEffect.Play();

        //Replace all sprites when died
        foreach (SpriteRenderer sprite in SpritesToHide)
        {
            sprite.enabled = false;
        }
        FinePole.enabled = false;
        BrokenPole.enabled = true;
    }
}
